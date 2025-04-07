using Microsoft.VisualStudio.TestTools.UnitTesting;
using Duo.Services;
using Duo.Repositories;
using Duo.Models;
using Duo.Models.Exercises;
using DuoTesting.Helper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DuoTesting.Services
{
    [TestClass]
    public class ExerciseServiceUT : TestBase
    {
        private ExerciseService _service = null!;
        private ExerciseRepository _repository = null!;
        private int _createdExerciseId;

        [TestInitialize]
        public void Setup()
        {
            base.BaseSetup();
            _repository = new ExerciseRepository(DbConnection);
            _service = new ExerciseService(_repository);
        }

        [TestCleanup]
        public async Task Cleanup()
        {
            if (_createdExerciseId > 0)
            {
                await _service.DeleteExercise(_createdExerciseId);
            }
        }

        private Exercise CreateSampleExercise()
        {
            return new FlashcardExercise(0, "What is the capital of Norway?", "Oslo", Difficulty.Easy);

        }

        [TestMethod]
        public async Task CreateExercise_ShouldAddExercise()
        {
            var exercise = CreateSampleExercise();
            await _service.CreateExercise(exercise);

            var all = await _service.GetAllExercises();
            var added = all.Find(e => e.Question == exercise.Question);
            Assert.IsNotNull(added);

            _createdExerciseId = added.Id;
        }

        [TestMethod]
        public async Task GetAllExercises_ShouldReturnList()
        {
            var list = await _service.GetAllExercises();
            Assert.IsNotNull(list);
            Assert.IsInstanceOfType(list, typeof(List<Exercise>));
        }

        [TestMethod]
        public async Task GetExerciseById_ShouldReturnCorrect()
        {
            var exercise = CreateSampleExercise();
            await _service.CreateExercise(exercise);

            var all = await _service.GetAllExercises();
            var added = all.Find(e => e.Question == exercise.Question);
            Assert.IsNotNull(added);

            _createdExerciseId = added.Id;

            var fetched = await _service.GetExerciseById(_createdExerciseId);
            Assert.AreEqual("What is the capital of Norway?", fetched.Question);
        }

        [TestMethod]
        public async Task DeleteExercise_ShouldRemoveExercise()
        {
            var exercise = CreateSampleExercise();
            await _service.CreateExercise(exercise);

            var all = await _service.GetAllExercises();
            var added = all.Find(e => e.Question == exercise.Question);
            Assert.IsNotNull(added);

            _createdExerciseId = added.Id;

            await _service.DeleteExercise(_createdExerciseId);

            var after = await _service.GetAllExercises();
            Assert.IsNull(after.Find(e => e.Id == _createdExerciseId));
            _createdExerciseId = 0;
        }

        [TestMethod]
        public async Task GetAllExercisesFromQuiz_ShouldReturn()
        {
            var result = await _service.GetAllExercisesFromQuiz(1);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task GetAllExercisesFromExam_ShouldReturn()
        {
            var result = await _service.GetAllExercisesFromExam(1);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task CreateExercise_InvalidType_ShouldThrow()
        {
            var invalid = new InvalidExercise(0, "Invalid?", Difficulty.Normal);

            await Assert.ThrowsExceptionAsync<ValidationException>(() => _service.CreateExercise(invalid));
        }

        // Helper class for invalid test case
        private class InvalidExercise : Exercise
        {
            public InvalidExercise(int id, string question, Difficulty difficulty)
                : base(id, question, difficulty)
            {
            }
        }
    }
}
