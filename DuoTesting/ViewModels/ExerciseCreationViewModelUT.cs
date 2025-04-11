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
using Duo.Helpers;
using Microsoft.UI.Xaml;
using Windows.UI.Core;

namespace DuoTesting.ViewModels
{

    [TestClass]
    public class ExerciseCreationViewModelTests
    {
        private Mock<IExerciseService> _mockExerciseService;
        private Mock<IExerciseViewFactory> _mockExerciseViewFactory;
        private ExerciseCreationViewModel _viewModel;
        private MainWindow _window;

        [TestInitialize]
        public void Initialize()
        {
            // Mock the IExerciseService
            _mockExerciseService = new Mock<IExerciseService>();

            // Mock the IExerciseViewFactory
            _mockExerciseViewFactory = new Mock<IExerciseViewFactory>();

            // Instantiate the ViewModel with mocked services
            _viewModel = new ExerciseCreationViewModel(_mockExerciseService.Object, _mockExerciseViewFactory.Object);
        }

        [UITestMethod]
        public void UpdateExerciseContent_ShouldSetSelectedExerciseContent_WhenExerciseTypeIsAssociation()
        {
            // Arrange
            var mockAssociationExerciseView = new CreateAssociationExercise();
            _mockExerciseViewFactory
                .Setup(factory => factory.CreateExerciseView("Association"))
                .Returns(mockAssociationExerciseView);

            // Act
            _viewModel.SelectedExerciseType = "Association";

            // Assert
            Assert.IsInstanceOfType(_viewModel.SelectedExerciseContent, typeof(CreateAssociationExercise));
            _mockExerciseViewFactory.Verify(factory => factory.CreateExerciseView("Association"), Times.Once);
        }

        [UITestMethod]
        public void UpdateExerciseContent_ShouldSetSelectedExerciseContent_WhenExerciseTypeIsFillInTheBlank()
        {
            // Arrange
            var mockFillInTheBlankExerciseView = new CreateFillInTheBlankExercise();
            _mockExerciseViewFactory
                .Setup(factory => factory.CreateExerciseView("Fill in the blank"))
                .Returns(mockFillInTheBlankExerciseView);

            // Act
            _viewModel.SelectedExerciseType = "Fill in the blank";

            // Assert
            Assert.IsInstanceOfType(_viewModel.SelectedExerciseContent, typeof(CreateFillInTheBlankExercise));
            _mockExerciseViewFactory.Verify(factory => factory.CreateExerciseView("Fill in the blank"), Times.Once);
        }

        [UITestMethod]
        public void UpdateExerciseContent_ShouldSetSelectedExerciseContent_WhenExerciseTypeIsMultipleChoice()
        {
            // Arrange
            var mockMultipleChoiceExerciseView = new CreateMultipleChoiceExercise();
            _mockExerciseViewFactory
                .Setup(factory => factory.CreateExerciseView("Multiple Choice"))
                .Returns(mockMultipleChoiceExerciseView);

            // Act
            _viewModel.SelectedExerciseType = "Multiple Choice";

            // Assert
            Assert.IsInstanceOfType(_viewModel.SelectedExerciseContent, typeof(CreateMultipleChoiceExercise));
            _mockExerciseViewFactory.Verify(factory => factory.CreateExerciseView("Multiple Choice"), Times.Once);
        }

        [UITestMethod]
        public void UpdateExerciseContent_ShouldSetSelectedExerciseContent_WhenExerciseTypeIsFlashcard()
        {
            // Arrange
            var mockFlashcardExerciseView = new CreateFlashcardExercise();
            _mockExerciseViewFactory
                .Setup(factory => factory.CreateExerciseView("Flashcard"))
                .Returns(mockFlashcardExerciseView);

            // Act
            _viewModel.SelectedExerciseType = "Flashcard";

            // Assert
            Assert.IsInstanceOfType(_viewModel.SelectedExerciseContent, typeof(CreateFlashcardExercise));
            _mockExerciseViewFactory.Verify(factory => factory.CreateExerciseView("Flashcard"), Times.Once);
        }


        [TestMethod]
        public void Constructor_ShouldInitialize_Properties()
        {
            IExerciseService exerciseService = new Mock<IExerciseService>().Object;
            IExerciseViewFactory exerciseViewFactory = new Mock<IExerciseViewFactory>().Object;

            var vm = new ExerciseCreationViewModel(exerciseService, exerciseViewFactory);

            // Verify that ExerciseTypes and Difficulties are populated
            Assert.IsNotNull(vm.ExerciseTypes);
            Assert.IsNotNull(vm.Difficulties);
        }
    }
}
