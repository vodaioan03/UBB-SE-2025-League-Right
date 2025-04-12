using Duo.Models;
using Duo.Services;
using Duo.ViewModels;
using Duo.ViewModels.CreateExerciseViewModels;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Duo.ViewModels.CreateExerciseViewModels.CreateFillInTheBlankExerciseViewModel;
using Duo.Models.Exercises;
using Duo.Helpers;

namespace DuoTesting.ViewModels.CreateExerciseViewModels
{
    [TestClass]
    public class CreateFillInTheBlankExerciseVmUT
    {
        [TestMethod]
        public void AddNewAnswer_ValidInput_AddsNewAnswer()
        {
            // Arrange
            var exerciseService = new Mock<IExerciseService>();
            var exerciseViewFactory = new Mock<IExerciseViewFactory>();
            var parentViewModel = new Mock<ExerciseCreationViewModel>(exerciseService.Object, exerciseViewFactory.Object);
            var viewModel = new CreateFillInTheBlankExerciseViewModel(parentViewModel.Object);
            int initialCount = viewModel.Answers.Count;
            // Act using the AddNewAnswerCommand
            viewModel.AddNewAnswerCommand.Execute(null);
            // Assert
            Assert.AreEqual(initialCount + 1, viewModel.Answers.Count);
        }


        [TestMethod]
        public void AddNewAnswer_ExceedingMaxAnswers_ShowsErrorMessage()
        {
            // Arrange
            var exerciseService = new Mock<IExerciseService>();
            var exerciseViewFactory = new Mock<IExerciseViewFactory>();
            var parentViewModel = new Mock<ExerciseCreationViewModel>(exerciseService.Object, exerciseViewFactory.Object);
            parentViewModel.Setup(p => p.RaiseErrorMessage(It.IsAny<string>(), It.IsAny<string>()));
            var viewModel = new CreateFillInTheBlankExerciseViewModel(parentViewModel.Object);
            // Fill the answers to the maximum
            for (int i = 0; i < CreateFillInTheBlankExerciseViewModel.MAX_ANSWERS; i++)
            {
                viewModel.Answers.Add(new Answer(string.Empty));
            }
            // Act
            viewModel.AddNewAnswerCommand.Execute(null);
            // Assert
            parentViewModel.Verify(p => p.RaiseErrorMessage("You can only have 3 answers for a fill in the blank exercise.", string.Empty), Times.Once);
        }

        [TestMethod]
        public void GenerateAnswerList_ValidInput_ReturnsCorrectList()
        {
            // Arrange
            var exerciseService = new Mock<IExerciseService>();
            var exerciseViewFactory = new Mock<IExerciseViewFactory>();
            var parentViewModel = new Mock<ExerciseCreationViewModel>(exerciseService.Object, exerciseViewFactory.Object);
            var viewModel = new CreateFillInTheBlankExerciseViewModel(parentViewModel.Object);
            viewModel.Answers.Add(new Answer("Answer1"));
            viewModel.Answers.Add(new Answer("Answer2"));
            viewModel.Answers.Add(new Answer("Answer3"));
            // Act
            var result = viewModel.GenerateAnswerList(viewModel.Answers);
            // Assert
            Assert.AreEqual(4, result.Count);
        }

        [TestMethod]
        public void CreateExercise_ValidInput_ReturnsCorrectExercise()
        {
            // Arrange
            var exerciseService = new Mock<IExerciseService>();
            var exerciseViewFactory = new Mock<IExerciseViewFactory>();
            var parentViewModel = new Mock<ExerciseCreationViewModel>(exerciseService.Object, exerciseViewFactory.Object);
            var viewModel = new CreateFillInTheBlankExerciseViewModel(parentViewModel.Object);
            string question = "What is the capital of France?";
            Difficulty difficulty = Difficulty.Easy;
            viewModel.Answers.Add(new Answer("Paris"));
            viewModel.Answers.Add(new Answer("Berlin"));
            // Act
            var result = viewModel.CreateExercise(question, difficulty);
            // Assert
            Assert.IsInstanceOfType(result, typeof(FillInTheBlankExercise));
            Assert.AreEqual(question, result.Question);
            Assert.AreEqual(difficulty, result.Difficulty);
        }
    }
}