using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Duo.ViewModels;
using Duo.Services;
using System;
using System.Threading.Tasks;
using Duo.Models.Exercises;
using Duo.Views.Components.CreateExerciseComponents;
using Duo;
using Duo.Commands;
using Duo.ViewModels.CreateExerciseViewModels;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using DuoTesting.Helper;
using System.Diagnostics;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer;

namespace DuoTesting.ViewModels
{

    [TestClass]
    public class ExerciseCreationViewModelTests
    {
        private Mock<IExerciseService> mockExerciseService;
        private ExerciseCreationViewModel viewModel;

        [TestInitialize]
        public void SetUp()
        {
            // Initialize the mock services
            mockExerciseService = new Mock<IExerciseService>();
            // Create the ViewModel with mocked service
            viewModel = new ExerciseCreationViewModel(mockExerciseService.Object);
        }

        [TestMethod]
        public async Task CreateExercise_ShouldCallCreateExercise_WhenMultipleChoiceIsSelected()
        {
            // Arrange: Set up necessary data for Multiple Choice exercise
            viewModel.SelectedExerciseType = "Multiple Choice";
            viewModel.SelectedDifficulty = "Easy";
            viewModel.QuestionText = "What is 2 + 2?";

            mockExerciseService.Setup(service => service.CreateExercise(It.IsAny<Exercise>()))
                .Returns(Task.CompletedTask); // Mocking the service method

            // Act: Call the CreateExercise method
            viewModel.CreateExercise();

            // Assert: Verify CreateExercise is called for MultipleChoiceExercise
            mockExerciseService.Verify(service => service.CreateExercise(It.IsAny<Exercise>()), Times.Once);
        }

        [UITestMethod]
        public async Task CreateExercise_ShouldCallCreateExercise_WhenAssociationIsSelected()
        {
            // Arrange: Set up necessary data for Association exercise
            viewModel.SelectedExerciseType = "Association";
            viewModel.SelectedDifficulty = "Normal";
            viewModel.QuestionText = "Match the pairs";

            mockExerciseService.Setup(service => service.CreateExercise(It.IsAny<Exercise>()))
                .Returns(Task.CompletedTask); // Mocking the service method

            // Act: Call the CreateExercise method
            await viewModel.CreateExercise();

            // Assert: Verify CreateExercise is called for AssociationExercise
            mockExerciseService.Verify(service => service.CreateExercise(It.IsAny<Exercise>()), Times.Once);
        }

        [UITestMethod]
        public async Task CreateExercise_ShouldCallCreateExercise_WhenFlashcardIsSelected()
        {
            // Arrange: Set up necessary data for Flashcard exercise
            viewModel.SelectedExerciseType = "Flashcard";
            viewModel.SelectedDifficulty = "Hard";
            viewModel.QuestionText = "What is the capital of France?";

            mockExerciseService.Setup(service => service.CreateExercise(It.IsAny<Exercise>()))
                .Returns(Task.CompletedTask); // Mocking the service method

            // Act: Call the CreateExercise method
            await viewModel.CreateExercise();

            // Assert: Verify CreateExercise is called for FlashcardExercise
            mockExerciseService.Verify(service => service.CreateExercise(It.IsAny<Exercise>()), Times.Once);
        }

        [UITestMethod]
        public async Task CreateExercise_ShouldCallCreateExercise_WhenFillInTheBlankIsSelected()
        {
            // Arrange: Set up necessary data for Fill in the Blank exercise
            viewModel.SelectedExerciseType = "Fill in the blank";
            viewModel.SelectedDifficulty = "Easy";
            viewModel.QuestionText = "The capital of Japan is ____.";

            mockExerciseService.Setup(service => service.CreateExercise(It.IsAny<Exercise>()))
                .Returns(Task.CompletedTask); // Mocking the service method

            // Act: Call the CreateExercise method
            await viewModel.CreateExercise();

            // Assert: Verify CreateExercise is called for FillInTheBlankExercise
            mockExerciseService.Verify(service => service.CreateExercise(It.IsAny<Exercise>()), Times.Once);
        }

        [TestMethod]
        public async Task CreateExercise_ShouldHandleError_WhenServiceFails()
        {
            // Arrange: Set up the service to throw an exception
            viewModel.SelectedExerciseType = "Multiple Choice";
            viewModel.SelectedDifficulty = "Normal";
            viewModel.QuestionText = "What is 2 + 2?";

            mockExerciseService.Setup(service => service.CreateExercise(It.IsAny<Exercise>()))
                .ThrowsAsync(new Exception("Error creating exercise"));

            // Act & Assert: Verify that an exception is handled gracefully
            await Assert.ThrowsExceptionAsync<Exception>(() => viewModel.CreateExercise());
        }

        [TestMethod]
        public void Constructor_ShouldInitialize_Properties()
        {
            IExerciseService exerciseService = new Mock<IExerciseService>().Object;

            var vm = new ExerciseCreationViewModel(exerciseService);

            // Verify that ExerciseTypes and Difficulties are populated
            Assert.IsNotNull(vm.ExerciseTypes);
            Assert.IsNotNull(vm.Difficulties);
        }
    }
}
