using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Duo.ViewModels;
using Duo.ViewModels.CreateExerciseViewModels;
using Duo.Services;
using System.Diagnostics;
using static Duo.ViewModels.CreateExerciseViewModels.CreateAssociationExerciseViewModel;
using Duo.Models;
using Duo.Models.Exercises;

namespace DuoTesting.ViewModels.CreateExerciseViewModels
{
    [TestClass]
    public class CreateAssociationExerciseVmUT
    {
        [TestMethod]
        public void AddNewAnswer_ValidInput_AddsNewAnswer()
        {
            // Arrange
            var exerciseService = new Mock<IExerciseService>();
            var parentViewModel = new Mock<ExerciseCreationViewModel>(exerciseService.Object);
            var viewModel = new CreateAssociationExerciseViewModel(parentViewModel.Object);
            int initialLeftCount = viewModel.LeftSideAnswers.Count;
            int initialRightCount = viewModel.RightSideAnswers.Count;
        
            // Act using the AddNewAnswerCommand
            viewModel.AddNewAnswerCommand.Execute(null);

            // Assert
            Assert.AreEqual(initialLeftCount + 1, viewModel.LeftSideAnswers.Count);
            Assert.AreEqual(initialRightCount + 1, viewModel.RightSideAnswers.Count);
        }

        [TestMethod]
        public void AddNewAnswer_ExceedingMaxAnswers_ShowsErrorMessage()
        {
            // Arrange
            var exerciseService = new Mock<IExerciseService>();

            var parentViewModel = new Mock<ExerciseCreationViewModel>(exerciseService.Object);
            parentViewModel.Setup(p => p.RaiseErrorMessage(It.IsAny<string>(), It.IsAny<string>()));

            var viewModel = new CreateAssociationExerciseViewModel(parentViewModel.Object);

            // Fill the answers to the maximum
            for (int i = 0; i < CreateAssociationExerciseViewModel.MAXIMUM_ANSWERS; i++)
            {
                viewModel.LeftSideAnswers.Add(new Answer(string.Empty));
                viewModel.RightSideAnswers.Add(new Answer(string.Empty));
            }

            // Act
            viewModel.AddNewAnswerCommand.Execute(null);

            // Assert
            parentViewModel.Verify(p => p.RaiseErrorMessage("You can only have up to 5 answers", string.Empty), Times.Once);
        }

        [TestMethod]
        public void GenerateAnswerList_ValidInput_ReturnsCorrectList()
        {
            // Arrange
            var exerciseService = new Mock<IExerciseService>();
            var parentViewModel = new Mock<ExerciseCreationViewModel>(exerciseService.Object);
            var viewModel = new CreateAssociationExerciseViewModel(parentViewModel.Object);
            viewModel.LeftSideAnswers.Add(new Answer("Answer 1"));
            viewModel.LeftSideAnswers.Add(new Answer("Answer 2"));
            viewModel.RightSideAnswers.Add(new Answer("Answer A"));
            viewModel.RightSideAnswers.Add(new Answer("Answer B"));


            // Act
            var leftAnswers = viewModel.GenerateAnswerList(viewModel.LeftSideAnswers);
            var rightAnswers = viewModel.GenerateAnswerList(viewModel.RightSideAnswers);


            // Assert
            Assert.AreEqual(3, leftAnswers.Count);
            Assert.AreEqual(3, rightAnswers.Count);
            Assert.AreEqual("Answer 1", leftAnswers[1]);
            Assert.AreEqual("Answer A", rightAnswers[1]);
        }

        [TestMethod]
        public void CreateExercise_Executes()
        {
            // Arrange
            var exerciseService = new Mock<IExerciseService>();
            var parentViewModel = new Mock<ExerciseCreationViewModel>(exerciseService.Object);
            var viewModel = new CreateAssociationExerciseViewModel(parentViewModel.Object);

            string question = "Sample Question";
            Difficulty difficulty = Difficulty.Easy;

            // Act
            var exercise = viewModel.CreateExercise(question, difficulty);

            // Assert
            Assert.IsNotNull(exercise);
            Assert.IsInstanceOfType(exercise, typeof(AssociationExercise));
            Assert.AreEqual(question, exercise.Question);
            Assert.AreEqual(difficulty, exercise.Difficulty);
        }
    }
}
