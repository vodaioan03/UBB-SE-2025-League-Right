using Duo.Models;
using Duo.Models.Exercises;
using Duo.Services;
using Duo.ViewModels.ExerciseViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace DuoTesting.ViewModels.ExerciseViewModels
{
    [TestClass]
    public class AssociationExerciseViewModelTests
    {
        [TestMethod]
        public async Task GetExercise_WithValidAssociationExercise_DoesNotThrowExceptionAsync()
        {
            // Arrange
            var mockExerciseService = new Mock<IExerciseService>();
            var exercise = new AssociationExercise(1, string.Empty, Difficulty.Easy, new List<string> { "A", "B" }, new List<string> { "1", "2" });
            mockExerciseService.Setup(s => s.GetExerciseById(It.IsAny<int>())).ReturnsAsync(exercise);
            var viewModel = new AssociationExerciseViewModel(mockExerciseService.Object);

            // Act & Assert
            try
            {
                await viewModel.GetExercise(1); 
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
            var viewModel = new AssociationExerciseViewModel(mockExerciseService.Object);

            // Act & Assert - the exception is expected
            await viewModel.GetExercise(1);
        }

        [TestMethod]
        public void VerifyIfAnswerIsCorrect_WithValidAnswers_ReturnsTrue()
        {
            // Arrange
            var mockExerciseService = new Mock<IExerciseService>();
            var exercise = new AssociationExercise(1, string.Empty, Difficulty.Easy, new List<string> { "A", "B" }, new List<string> { "1", "2" });
            mockExerciseService.Setup(s => s.GetExerciseById(It.IsAny<int>())).ReturnsAsync(exercise);
            var viewModel = new AssociationExerciseViewModel(mockExerciseService.Object);
            viewModel.GetExercise(1).Wait();
            viewModel.UserAnswers = new ObservableCollection<(string, string)>
            {
                ("A", "1"),
                ("B", "2")
            };

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
            var exercise = new AssociationExercise(1, string.Empty, Difficulty.Easy, new List<string> { "A", "B" }, new List<string> { "1", "2" });
            mockExerciseService.Setup(s => s.GetExerciseById(It.IsAny<int>())).ReturnsAsync(exercise);
            var viewModel = new AssociationExerciseViewModel(mockExerciseService.Object);
            viewModel.GetExercise(1).Wait();
            viewModel.UserAnswers = new ObservableCollection<(string, string)>
            {
                ("A", "2"),
                ("B", "1")
            };
            // Act
            bool result = viewModel.VerifyIfAnswerIsCorrect();
            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void VerifyIfAnswerIsCorrect_WithNullUserAnswers_ReturnsFalse()
        {
            // Arrange
            var mockExerciseService = new Mock<IExerciseService>();
            var exercise = new AssociationExercise(1, string.Empty, Difficulty.Easy, new List<string> { "A", "B" }, new List<string> { "1", "2" });
            mockExerciseService.Setup(s => s.GetExerciseById(It.IsAny<int>())).ReturnsAsync(exercise);
            var viewModel = new AssociationExerciseViewModel(mockExerciseService.Object);
            viewModel.GetExercise(1).Wait();
            viewModel.UserAnswers = null;

            // Act
            bool result = viewModel.VerifyIfAnswerIsCorrect();

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void VerifyIfAnswerIsCorrect_WithEmptyUserAnswers_ReturnsFalse()
        {
            // Arrange
            var mockExerciseService = new Mock<IExerciseService>();
            var exercise = new AssociationExercise(1, string.Empty, Difficulty.Easy, new List<string> { "A", "B" }, new List<string> { "1", "2" });
            mockExerciseService.Setup(s => s.GetExerciseById(It.IsAny<int>())).ReturnsAsync(exercise);
            var viewModel = new AssociationExerciseViewModel(mockExerciseService.Object);
            viewModel.GetExercise(1).Wait();
            viewModel.UserAnswers = new ObservableCollection<(string, string)>();

            // Act
            bool result = viewModel.VerifyIfAnswerIsCorrect();

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void VerifyIfAnswerIsCorrect_WithDifferentLengthUserAnswers_ReturnsFalse()
        {
            // Arrange
            var mockExerciseService = new Mock<IExerciseService>();
            var exercise = new AssociationExercise(1, string.Empty, Difficulty.Easy, new List<string> { "A", "B" }, new List<string> { "1", "2" });
            mockExerciseService.Setup(s => s.GetExerciseById(It.IsAny<int>())).ReturnsAsync(exercise);
            var viewModel = new AssociationExerciseViewModel(mockExerciseService.Object);
            viewModel.GetExercise(1).Wait();
            viewModel.UserAnswers = new ObservableCollection<(string, string)>
            {
                ("A", "1")
            };
            // Act
            bool result = viewModel.VerifyIfAnswerIsCorrect();
            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void VerifyIfAnswerIsCorrect_WithNullExercise_ReturnsFalse()
        {
            // Arrange
            var mockExerciseService = new Mock<IExerciseService>();
            var viewModel = new AssociationExerciseViewModel(mockExerciseService.Object);
            viewModel.UserAnswers = new ObservableCollection<(string, string)>
            {
                ("A", "1"),
                ("B", "2")
            };

            // Act
            bool result = viewModel.VerifyIfAnswerIsCorrect();

            // Assert
            Assert.IsFalse(result);
        }
    }
}