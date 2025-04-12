using Duo.Models;
using Duo.Models.Exercises;
using Duo.Services;
using Duo.ViewModels.ExerciseViewModels;
using Moq;
using System;
using System.Collections.ObjectModel;

namespace DuoTesting.ViewModels.ExerciseViewModels
{
    [TestClass]
    public class MultipleChoiceExerciseViewModelUT
    {
        [TestMethod]
        public async Task GetExercise_WithValidMultipleChoiceExercise_DoesNotThrowExceptionAsync()
        {
            // Arrange
            var mockExerciseService = new Mock<IExerciseService>();
            var exercise = new MultipleChoiceExercise(
                1, 
                "Question", 
                Difficulty.Easy, 
                new List<MultipleChoiceAnswerModel>
                {
                    new MultipleChoiceAnswerModel("Answer1", true),
                    new MultipleChoiceAnswerModel("Answer2", false)
                }
            );
            mockExerciseService.Setup(s => s.GetExerciseById(It.IsAny<int>())).ReturnsAsync(exercise);
            var viewModel = new MultipleChoiceExerciseViewModel(mockExerciseService.Object);

            // Act & Assert
            try
            {
                await viewModel.GetExercise(1);
                Assert.IsNotNull(viewModel.UserChoices);
            }
            catch (Exception)
            {
                Assert.Fail("Exception was thrown when it should not have been.");
            }
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task GetExercise_WithInvalidExerciseType_ThrowsExceptionAsync()
        {
            // Arrange
            var mockExerciseService = new Mock<IExerciseService>();
            var exercise = new FlashcardExercise(1, "Question", "Answer", Difficulty.Easy);
            mockExerciseService.Setup(s => s.GetExerciseById(It.IsAny<int>())).ReturnsAsync(exercise);
            var viewModel = new MultipleChoiceExerciseViewModel(mockExerciseService.Object);

            // Act & Assert - the exception is expected
            await viewModel.GetExercise(1);
        }

        [TestMethod]
        public void VerifyIfAnswerIsCorrect_WithValidAnswers_ReturnsTrue()
        {
            // Arrange
            var mockExerciseService = new Mock<IExerciseService>();
            var exercise = new MultipleChoiceExercise(
                1,
                "Question",
                Difficulty.Easy,
                new List<MultipleChoiceAnswerModel>
                {
                    new MultipleChoiceAnswerModel("Answer1", true),
                    new MultipleChoiceAnswerModel("Answer2", false)
                }
            );
            mockExerciseService.Setup(s => s.GetExerciseById(It.IsAny<int>())).ReturnsAsync(exercise);
            var viewModel = new MultipleChoiceExerciseViewModel(mockExerciseService.Object);
            viewModel.GetExercise(1).Wait();
            viewModel.UserChoices = new ObservableCollection<string> { "Answer1" };

            // Act
            bool result = viewModel.VerifyIfAnswerIsCorrect();
            
            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void VerifyIfAnswerIsCorrect_WithInvalidAnswers_ReturnsFalse()
        {
            // Arrange
            var mockExerciseService = new Mock<IExerciseService>();
            var exercise = new MultipleChoiceExercise(
                1,
                "Question",
                Difficulty.Easy,
                new List<MultipleChoiceAnswerModel>
                {
                    new MultipleChoiceAnswerModel("Answer1", true),
                    new MultipleChoiceAnswerModel("Answer2", false)
                }
            );
            mockExerciseService.Setup(s => s.GetExerciseById(It.IsAny<int>())).ReturnsAsync(exercise);
            var viewModel = new MultipleChoiceExerciseViewModel(mockExerciseService.Object);
            viewModel.GetExercise(1).Wait();
            viewModel.UserChoices = new ObservableCollection<string> { "Answer2" };

            // Act
            bool result = viewModel.VerifyIfAnswerIsCorrect();

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void VerifyIfAnswerIsCorrect_WithoutInitialization_ThrowsException()
        {
            // Arrange
            var mockExerciseService = new Mock<IExerciseService>();
            var viewModel = new MultipleChoiceExerciseViewModel(mockExerciseService.Object);
            
            // Act
            viewModel.VerifyIfAnswerIsCorrect();

            // Assert - exception is expected
        }
    }
}