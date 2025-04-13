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

        [TestMethod]
        public async Task GetByIdAsync_NonexistentId_ShouldThrow()
        {
            await Assert.ThrowsExceptionAsync<KeyNotFoundException>(() =>
                _repository.GetByIdAsync(999));
        }

        [TestMethod]
        public async Task ClearAll_ShouldResetRepository()
        {
            var exercise = new FlashcardExercise(0, "Q?", "A", Difficulty.Easy);
            int id = await _repository.AddExerciseAsync(exercise);

            ((InMemoryExerciseRepository)_repository).ClearAll();

            await Assert.ThrowsExceptionAsync<KeyNotFoundException>(() =>
                _repository.GetByIdAsync(id));
        }

        private class UnknownExercise : Exercise
        {
            public UnknownExercise() : base(0, "?", Difficulty.Easy) { }
        }

        [TestMethod]
        public void AddExerciseAsync_UnsupportedType_ShouldThrow()
        {
            var repo = new InMemoryExerciseRepository();
            Assert.ThrowsExceptionAsync<NotSupportedException>(() =>
                repo.AddExerciseAsync(new UnknownExercise()));
        }

        [TestMethod]
        public async Task GetAllExercisesAsync_ShouldReturnAllExercises()
        {
            var ex1 = new FlashcardExercise(0, "Q1", "A1", Difficulty.Easy);
            var ex2 = new FlashcardExercise(0, "Q2", "A2", Difficulty.Hard);

            int id1 = await _repository.AddExerciseAsync(ex1);
            int id2 = await _repository.AddExerciseAsync(ex2);

            var all = await _repository.GetAllExercisesAsync();

            Assert.AreEqual(2, all.Count);
        }

        [TestMethod]
        public async Task GetExamAndQuizExercises_ShouldReturnAll()
        {
            var ex = new FillInTheBlankExercise(0, "Largest ocean?", Difficulty.Easy, new List<string> { "Pacific" });
            await _repository.AddExerciseAsync(ex);

            var quizExercises = await _repository.GetQuizExercisesAsync(1);
            var examExercises = await _repository.GetExamExercisesAsync(1);

            Assert.AreEqual(1, quizExercises.Count);
            Assert.AreEqual(1, examExercises.Count);
        }

    }
}
