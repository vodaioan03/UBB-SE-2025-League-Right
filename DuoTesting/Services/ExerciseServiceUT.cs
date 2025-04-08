using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Duo.Services;
using Duo.Repositories;
using Duo.Models;
using Duo.Models.Exercises;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DuoTesting.Services
{
    [TestClass]
    public class ExerciseServiceUT
    {
        private ExerciseService _service = null!;
        private Mock<IExerciseRepository> _mockRepo = null!;
        private int _createdExerciseId;

        [TestInitialize]
        public void Setup()
        {
            _mockRepo = new Mock<IExerciseRepository>();
            _service = new ExerciseService(_mockRepo.Object);
        }

        private Exercise CreateSampleExercise()
        {
            return new FlashcardExercise(0, "What is the capital of Norway?", "Oslo", Difficulty.Easy);
        }

        [TestMethod]
        public async Task CreateExercise_ShouldAddExercise()
        {
            var exercise = CreateSampleExercise();
            _mockRepo.Setup(r => r.AddExerciseAsync(exercise)).ReturnsAsync(1);

            await _service.CreateExercise(exercise);

            _mockRepo.Verify(r => r.AddExerciseAsync(exercise), Times.Once);
        }

        [TestMethod]
        public async Task GetAllExercises_ShouldReturnList()
        {
            var exercises = new List<Exercise> { CreateSampleExercise() };
            _mockRepo.Setup(r => r.GetAllExercisesAsync()).ReturnsAsync(exercises);

            var result = await _service.GetAllExercises();

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public async Task GetExerciseById_ShouldReturnCorrect()
        {
            var exercise = CreateSampleExercise();
            _mockRepo.Setup(r => r.GetByIdAsync(42)).ReturnsAsync(exercise);

            var result = await _service.GetExerciseById(42);

            Assert.AreEqual("What is the capital of Norway?", result.Question);
        }

        [TestMethod]
        public async Task DeleteExercise_ShouldRemoveExercise()
        {
            _mockRepo.Setup(r => r.DeleteExerciseAsync(42)).Returns(Task.CompletedTask);

            await _service.DeleteExercise(42);

            _mockRepo.Verify(r => r.DeleteExerciseAsync(42), Times.Once);
        }

        [TestMethod]
        public async Task GetAllExercisesFromQuiz_ShouldReturn()
        {
            var exercises = new List<Exercise> { CreateSampleExercise() };
            _mockRepo.Setup(r => r.GetQuizExercisesAsync(1)).ReturnsAsync(exercises);

            var result = await _service.GetAllExercisesFromQuiz(1);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public async Task GetAllExercisesFromExam_ShouldReturn()
        {
            var exercises = new List<Exercise> { CreateSampleExercise() };
            _mockRepo.Setup(r => r.GetExamExercisesAsync(1)).ReturnsAsync(exercises);

            var result = await _service.GetAllExercisesFromExam(1);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
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
                : base(id, question, difficulty) { }
        }
    }
}
