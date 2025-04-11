using Duo.Helpers;
using Duo.Models;
using Duo.Models.Exercises;
using Duo.Services;
using Duo.ViewModels;
using Duo.ViewModels.CreateExerciseViewModels;
using Moq;
using static Duo.ViewModels.CreateExerciseViewModels.CreateMultipleChoiceExerciseViewModel;

namespace DuoTesting.ViewModels.CreateExerciseViewModels
{

    [TestClass]
    public class CreateMultipleChoiceExerciseViewModelUT
    {
        [TestMethod]
        public void AddNewAnswerCommand_ValidInput_AddsNewAnswer()
        {
            // Arrange
            var exerciseService = new Mock<IExerciseService>();
            var exerciseViewFactory = new Mock<IExerciseViewFactory>();
            var parentViewModel = new Mock<ExerciseCreationViewModel>(exerciseService.Object, exerciseViewFactory.Object);
            var viewModel = new CreateMultipleChoiceExerciseViewModel(parentViewModel.Object);
            int initialCount = viewModel.Answers.Count;
            // Act
            viewModel.AddNewAnswerCommand.Execute(null);
            // Assert
            Assert.AreEqual(initialCount + 1, viewModel.Answers.Count);
        }

        [TestMethod]
        public void AddNewAnswerCommand_ExceedingMaxAnswers_ShowsErrorMessage()
        {
            // Arrange
            var exerciseService = new Mock<IExerciseService>();
            var exerciseViewFactory = new Mock<IExerciseViewFactory>();
            var parentViewModel = new Mock<ExerciseCreationViewModel>(exerciseService.Object, exerciseViewFactory.Object);
            parentViewModel.Setup(p => p.RaiseErrorMessage(It.IsAny<string>(), It.IsAny<string>()));
            var viewModel = new CreateMultipleChoiceExerciseViewModel(parentViewModel.Object);
            // Fill the answers to the maximum
            for (int i = 0; i < CreateMultipleChoiceExerciseViewModel.MAXIMUM_ANSWERS; i++)
            {
                viewModel.Answers.Add(new Answer(string.Empty, true));
            }
            // Act
            viewModel.AddNewAnswerCommand.Execute(null);
            // Assert
            parentViewModel.Verify(p => p.RaiseErrorMessage("Cannot add more answers", $"Maximum number of answers ({CreateMultipleChoiceExerciseViewModel.MAXIMUM_ANSWERS}) reached."), Times.Once);
        }

        [TestMethod]
        public void GenerateAnswerModelList_ValidInput_ReturnsCorrectList()
        {
            // Arrange
            var exerciseService = new Mock<IExerciseService>();
            var exerciseViewFactory = new Mock<IExerciseViewFactory>();
            var parentViewModel = new Mock<ExerciseCreationViewModel>(exerciseService.Object, exerciseViewFactory.Object);
            var viewModel = new CreateMultipleChoiceExerciseViewModel(parentViewModel.Object);
            viewModel.Answers.Add(new Answer("Answer1", true));
            viewModel.Answers.Add(new Answer("Answer2", false));
            viewModel.Answers.Add(new Answer("Answer3", false));
            // Act
            var result = viewModel.GenerateAnswerModelList();
            // Assert
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual("Answer1", result[0].Answer);
            Assert.IsTrue(result[0].IsCorrect);
        }

        [TestMethod]
        public void CreateExercise_ValidInput_ReturnsCorrectExercise()
        {
            // Arrange
            var exerciseService = new Mock<IExerciseService>();
            var exerciseViewFactory = new Mock<IExerciseViewFactory>();
            var parentViewModel = new Mock<ExerciseCreationViewModel>(exerciseService.Object, exerciseViewFactory.Object);
            var viewModel = new CreateMultipleChoiceExerciseViewModel(parentViewModel.Object);
            string questionText = "What is the capital of France?";
            Difficulty difficulty = Difficulty.Easy;
            viewModel.Answers.Add(new Answer("Paris", true));
            viewModel.Answers.Add(new Answer("London", false));
            // Act
            var result = viewModel.CreateExercise(questionText, difficulty);
            // Assert
            Assert.IsInstanceOfType(result, typeof(MultipleChoiceExercise));
            Assert.AreEqual(questionText, result.Question);
            Assert.AreEqual(difficulty, result.Difficulty);
        }

        [TestMethod]
        public void UpdateSelectedAnswerCommand_ValidInput_UpdatesSelectedAnswer()
        {
            // Arrange
            var exerciseService = new Mock<IExerciseService>();
            var exerciseViewFactory = new Mock<IExerciseViewFactory>();
            var parentViewModel = new Mock<ExerciseCreationViewModel>(exerciseService.Object, exerciseViewFactory.Object);
            var viewModel = new CreateMultipleChoiceExerciseViewModel(parentViewModel.Object);
            string answerToSelect = "Answer1";
            viewModel.Answers.Add(new Answer(answerToSelect, true));
            // Act
            viewModel.UpdateSelectedAnswerComand.Execute(answerToSelect);
            // Assert
            Assert.AreEqual(answerToSelect, viewModel.SelectedAnswer);
        }
    }
}