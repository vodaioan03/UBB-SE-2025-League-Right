using Duo.Models;
using Duo.Models.Exercises;
using Duo.Services;
using Duo.ViewModels.ExerciseViewModels;
using Moq;
using System.Collections.ObjectModel;

namespace DuoTesting.ViewModels.ExerciseViewModels
{
    [TestClass]
    public class FillInTheBlankExerciseViewModelUT
    {
        [TestMethod]
        public async Task GetExercise_WithValidFillInTheBlankExercise_DoesNotThrowExceptionAsync()
        {
            // Arrange
            var mockExerciseService = new Mock<IExerciseService>();
            var exercise = new FillInTheBlankExercise(1, "Question", Difficulty.Easy, new List<string> { "Answer1", "Answer2" });
            mockExerciseService.Setup(s => s.GetExerciseById(It.IsAny<int>())).ReturnsAsync(exercise);
            var viewModel = new FillInTheBlankExerciseViewModel(mockExerciseService.Object);

            // Act & Assert
            try
            {
                await viewModel.GetExercise(1);
                Assert.IsNotNull(viewModel.UserAnswers);
                Assert.AreEqual(viewModel.UserAnswers.Count, exercise.PossibleCorrectAnswers.Count);
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
            var viewModel = new FillInTheBlankExerciseViewModel(mockExerciseService.Object);

            // Act & Assert - the exception is expected
            await viewModel.GetExercise(1);
        }

        [TestMethod]
        public void VerifyIfAnswerIsCorrect_WithValidAnswers_ReturnsTrue()
        {
            // Arrange
            var mockExerciseService = new Mock<IExerciseService>();
            var exercise = new FillInTheBlankExercise(1, "Question", Difficulty.Easy, new List<string> { "Answer1", "Answer2" });
            mockExerciseService.Setup(s => s.GetExerciseById(It.IsAny<int>())).ReturnsAsync(exercise);
            var viewModel = new FillInTheBlankExerciseViewModel(mockExerciseService.Object);
            viewModel.GetExercise(1).Wait();
            viewModel.UserAnswers = new ObservableCollection<string> { "Answer1", "Answer2" };
            
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
            var exercise = new FillInTheBlankExercise(1, "Question", Difficulty.Easy, new List<string> { "Answer1", "Answer2" });
            mockExerciseService.Setup(s => s.GetExerciseById(It.IsAny<int>())).ReturnsAsync(exercise);
            var viewModel = new FillInTheBlankExerciseViewModel(mockExerciseService.Object);
            viewModel.GetExercise(1).Wait();
            viewModel.UserAnswers = new ObservableCollection<string> { "WrongAnswer1", "WrongAnswer2" };

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
            var viewModel = new FillInTheBlankExerciseViewModel(mockExerciseService.Object);

            // Act
            viewModel.VerifyIfAnswerIsCorrect(); // This should throw an exception
        }
    }
}