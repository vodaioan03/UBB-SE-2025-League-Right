using Microsoft.VisualStudio.TestTools.UnitTesting;
using Duo.Models;
using Duo.Models.Exercises;
using Duo.Repositories;
using DuoTesting.MockClasses;
using DuoTesting.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace DuoTesting.Repositories
{
    [TestClass]
    public class ExerciseRepositoryUT
    {
        private IExerciseRepository _repository = null!;

        [TestInitialize]
        public void Setup()
        {
            _repository = new InMemoryExerciseRepository();
        }

        [TestMethod]
        public async Task AddGetDelete_MultipleChoiceExercise_WorksCorrectly()
        {
            var exercise = new MultipleChoiceExercise(
                0,
                "What is 2 + 2?",
                Difficulty.Easy,
                new List<MultipleChoiceAnswerModel>
                {
                    new("4", true),
                    new("3", false),
                    new("5", false)
                });

            int id = await _repository.AddExerciseAsync(exercise);
            var result = await _repository.GetByIdAsync(id);

            var expected = new MultipleChoiceExercise(id, exercise.Question, exercise.Difficulty, exercise.Choices);
            Assert.IsTrue(new ExerciseComparer().Equals(expected, result));

            await _repository.DeleteExerciseAsync(id);
        }

        [TestMethod]
        public async Task AddDelete_FlashcardExercise_ShouldWork()
        {
            var exercise = new FlashcardExercise(0, "Capital of France?", "Paris", Difficulty.Easy);
            int id = await _repository.AddExerciseAsync(exercise);

            var result = await _repository.GetByIdAsync(id);
            var expected = new FlashcardExercise(id, exercise.Question, exercise.Answer, exercise.TimeInSeconds, exercise.Difficulty);

            Assert.IsTrue(new ExerciseComparer().Equals(expected, result));
            await _repository.DeleteExerciseAsync(id);
        }

        [TestMethod]
        public async Task AddDelete_FillInTheBlankExercise_ShouldWork()
        {
            var exercise = new FillInTheBlankExercise(0, "____ is the largest planet.", Difficulty.Easy, new List<string> { "Jupiter" });

            int id = await _repository.AddExerciseAsync(exercise);
            var result = await _repository.GetByIdAsync(id);

            var expected = new FillInTheBlankExercise(id, exercise.Question, exercise.Difficulty, exercise.PossibleCorrectAnswers);
            Assert.IsTrue(new ExerciseComparer().Equals(expected, result));

            await _repository.DeleteExerciseAsync(id);
        }

        [TestMethod]
        public async Task AddDelete_AssociationExercise_ShouldWork()
        {
            var exercise = new AssociationExercise(
                0,
                "Match countries to capitals",
                Difficulty.Hard,
                new List<string> { "Germany" },
                new List<string> { "Berlin" });

            int id = await _repository.AddExerciseAsync(exercise);
            var result = await _repository.GetByIdAsync(id);

            var expected = new AssociationExercise(id, exercise.Question, exercise.Difficulty, exercise.FirstAnswersList, exercise.SecondAnswersList);
            Assert.IsTrue(new ExerciseComparer().Equals(expected, result));

            await _repository.DeleteExerciseAsync(id);
        }

        [TestMethod]
        public async Task AddExerciseAsync_Null_ShouldThrow()
        {
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => _repository.AddExerciseAsync(null!));
        }

        [TestMethod]
        public async Task GetByIdAsync_Invalid_ShouldThrow()
        {
            await Assert.ThrowsExceptionAsync<ArgumentException>(() => _repository.GetByIdAsync(0));
        }

        [TestMethod]
        public async Task DeleteExerciseAsync_Invalid_ShouldThrow()
        {
            await Assert.ThrowsExceptionAsync<ArgumentException>(() => _repository.DeleteExerciseAsync(0));
        }
    }
}
