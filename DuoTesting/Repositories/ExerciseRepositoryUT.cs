using Microsoft.VisualStudio.TestTools.UnitTesting;
using Duo.Models;
using Duo.Models.Exercises;
using Duo.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using DuoTesting.Helper;
using System.Data.Common;

namespace DuoTesting.Repositories
{
    [TestClass]
    public class ExerciseRepositoryUT : TestBase
    {
        private IExerciseRepository _repository = null!;

        [TestInitialize]
        public void Setup()
        {
            _repository = new ExerciseRepository(DbConnection);
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
                    new MultipleChoiceAnswerModel("4", true),
                    new MultipleChoiceAnswerModel("3", false),
                    new MultipleChoiceAnswerModel("5", false)
                });

            int id = await _repository.AddExerciseAsync(exercise);
            Assert.IsTrue(id > 0);

            var fromDb = await _repository.GetByIdAsync(id);
            Assert.AreEqual("What is 2 + 2?", fromDb.Question);
            Assert.IsInstanceOfType(fromDb, typeof(MultipleChoiceExercise));

            await _repository.DeleteExerciseAsync(id);
        }

        [TestMethod]
        public async Task AddDelete_FlashcardExercise_ShouldWork()
        {
            var exercise = new FlashcardExercise(0, "Capital of France?", "Paris", Difficulty.Easy);
            int id = await _repository.AddExerciseAsync(exercise);

            var fetched = await _repository.GetByIdAsync(id);
            Assert.AreEqual("Paris", ((FlashcardExercise)fetched).Answer);

            await _repository.DeleteExerciseAsync(id);
        }

        [TestMethod]
        public async Task AddDelete_FillInTheBlankExercise_ShouldWork()
        {
            var exercise = new FillInTheBlankExercise(
                0,
                "____ is the largest planet.",
                Difficulty.Easy,
                new List<string> { "Jupiter" });

            int id = await _repository.AddExerciseAsync(exercise);
            var result = await _repository.GetByIdAsync(id);
            Assert.IsInstanceOfType(result, typeof(FillInTheBlankExercise));

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
            Assert.IsInstanceOfType(result, typeof(AssociationExercise));

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
