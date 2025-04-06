using Duo.Models.Exercises;
using Duo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuoTesting.ModelTesting.ExercisesTesting
{
    [TestClass]
    public class FillInTheBlankExerciseTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_NullPossibleCorrectAnswers_ThrowsArgumentException()
        {
            // Act
            var exercise = new FillInTheBlankExercise(1, "Fill in the blank question", Difficulty.Normal, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_EmptyPossibleCorrectAnswers_ThrowsArgumentException()
        {
            // Act
            var exercise = new FillInTheBlankExercise(1, "Fill in the blank question", Difficulty.Normal, new List<string>());
        }

        [TestMethod]
        public void ValidateAnswer_NullUserAnswers_ReturnsFalse()
        {
            // Arrange
            var exercise = new FillInTheBlankExercise(1, "Question", Difficulty.Normal, new List<string> { "answer" });

            // Act
            bool result = exercise.ValidateAnswer(null);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidateAnswer_CountMismatch_ReturnsFalse()
        {
            // Arrange
            var exercise = new FillInTheBlankExercise(1, "Question", Difficulty.Normal, new List<string> { "answer1", "answer2" });
            var userAnswers = new List<string> { "answer1" };

            // Act
            bool result = exercise.ValidateAnswer(userAnswers);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidateAnswer_IncorrectAnswer_ReturnsFalse()
        {
            // Arrange
            var exercise = new FillInTheBlankExercise(1, "Question", Difficulty.Normal, new List<string> { "answer" });
            var userAnswers = new List<string> { "wrong answer" };

            // Act
            bool result = exercise.ValidateAnswer(userAnswers);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidateAnswer_CorrectAnswer_IgnoresCaseAndWhitespace_ReturnsTrue()
        {
            // Arrange
            var exercise = new FillInTheBlankExercise(1, "Question", Difficulty.Normal, new List<string> { "answer" });
            var userAnswers = new List<string> { "  AnSwEr  " };

            // Act
            bool result = exercise.ValidateAnswer(userAnswers);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ToString_ReturnsExpectedFormat()
        {
            // Arrange
            var correctAnswers = new List<string> { "ans1", "ans2" };
            var exercise = new FillInTheBlankExercise(1, "Question", Difficulty.Normal, correctAnswers);

            // Act
            string result = exercise.ToString();

            // Assert
            StringAssert.Contains(result, "[Fill in the Blank]");
            StringAssert.Contains(result, "ans1");
            StringAssert.Contains(result, "ans2");
        }
    }
}
