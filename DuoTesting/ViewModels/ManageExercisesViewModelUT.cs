using Duo;
using Duo.Models.Exercises;
using Duo.Services;
using Duo.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;

namespace DuoTesting.ViewModels
{
    [TestClass]
    public class ManageExercisesViewModelUT
    {
        private Mock<IServiceProvider> _serviceProviderMock;
        private Mock<IExerciseService> _exerciseServiceMock;
        private FlashcardExercise exercise1;
        private FlashcardExercise exercise2;

        [TestInitialize]
        public void Initialize()
        {
            _serviceProviderMock = new Mock<IServiceProvider>();
            App.ServiceProvider = _serviceProviderMock.Object;

            _exerciseServiceMock = new Mock<IExerciseService>();
            _serviceProviderMock.Setup(sp => sp.GetService(typeof(IExerciseService)))
                .Returns(_exerciseServiceMock.Object);

            exercise1 = new FlashcardExercise(1, "question1", "answer1");
            exercise2 = new FlashcardExercise(2, "question2", "answer2");
        }

        [TestMethod]
        public async Task LoadExercisesAsync_ShouldLoadExercisesAsync()
        {
            // Arrange
            var exercises = new List<Exercise> { exercise1, exercise2 };
            _exerciseServiceMock.Setup(es => es.GetAllExercises())
                .ReturnsAsync(exercises);
            // Act
            var viewModel = new ManageExercisesViewModel();
            // Assert
            Assert.AreEqual(2, viewModel.Exercises.Count);
        }

        [TestMethod]
        public async Task DeleteExercise_ShouldRemoveExercise()
        {
            // Arrange
            var exercises = new List<Exercise> { exercise1, exercise2 };
            _exerciseServiceMock.Setup(es => es.GetAllExercises())
                .ReturnsAsync(exercises);
            var viewModel = new ManageExercisesViewModel();
            _exerciseServiceMock.Setup(es => es.DeleteExercise(It.IsAny<int>()))
                .Callback<int>(id => exercises.Remove(viewModel.Exercises.First(e => e.Id == id)))
                .Returns(Task.CompletedTask);
            // Act
            await viewModel.DeleteExercise(exercise1);
            // Assert
            Assert.AreEqual(1, viewModel.Exercises.Count);
        }
    }
}