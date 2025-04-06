using Duo.Models;
using Duo.Models.Exercises;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuoTesting.ModelTesting.ExercisesTesting
{
    [TestClass]
    public class FlashcardExerciseTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_EmptyAnswer_ThrowsArgumentException()
        {
            // Act
            var exercise = new FlashcardExercise(1, "Question", "   ", Difficulty.Normal);
        }

        [TestMethod]
        public void Constructor_SetsDefaultTimeBasedOnDifficulty_Easy()
        {
            // Arrange & Act
            var exercise = new FlashcardExercise(1, "Question", "Answer", Difficulty.Easy);

            // Assert
            Assert.AreEqual(15, exercise.TimeInSeconds);
        }

        [TestMethod]
        public void Constructor_SetsDefaultTimeBasedOnDifficulty_Normal()
        {
            // Arrange & Act
            var exercise = new FlashcardExercise(1, "Question", "Answer", Difficulty.Normal);

            // Assert
            Assert.AreEqual(30, exercise.TimeInSeconds);
        }

        [TestMethod]
        public void Constructor_SetsDefaultTimeBasedOnDifficulty_Hard()
        {
            // Arrange & Act
            var exercise = new FlashcardExercise(1, "Question", "Answer", Difficulty.Hard);

            // Assert
            Assert.AreEqual(45, exercise.TimeInSeconds);
        }

        [TestMethod]
        public void SecondConstructor_SetsTimeExplicitly()
        {
            // Arrange
            int customTime = 50;
            var exercise = new FlashcardExercise(1, "Sentence", "Answer", customTime, Difficulty.Normal);

            // Act & Assert
            Assert.AreEqual(customTime, exercise.TimeInSeconds);
        }

        [TestMethod]
        public void ValidateAnswer_NullOrWhitespace_ReturnsFalse()
        {
            // Arrange
            var exercise = new FlashcardExercise(1, "Question", "Answer", Difficulty.Normal);

            // Act & Assert
            Assert.IsFalse(exercise.ValidateAnswer(null));
            Assert.IsFalse(exercise.ValidateAnswer("   "));
        }

        [TestMethod]
        public void ValidateAnswer_CorrectAnswer_IgnoresCaseAndWhitespace_ReturnsTrue()
        {
            // Arrange
            var exercise = new FlashcardExercise(1, "Question", "Answer", Difficulty.Normal);

            // Act
            bool result = exercise.ValidateAnswer("  aNswer ");

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void GetCorrectAnswer_ReturnsStoredAnswer()
        {
            // Arrange
            var exercise = new FlashcardExercise(1, "Question", "CorrectAnswer", Difficulty.Normal);

            // Act
            string correct = exercise.GetCorrectAnswer();

            // Assert
            Assert.AreEqual("CorrectAnswer", correct);
        }

        [TestMethod]
        public void ToString_ReturnsExpectedFormat()
        {
            // Arrange
            var exercise = new FlashcardExercise(1, "Question", "Answer", Difficulty.Normal);
            // Expected format: "Id: {Id},  Difficulty: {Difficulty}, Time: {TimeInSeconds}s"
            string expected = $"Id: {exercise.Id},  Difficulty: {exercise.Difficulty}, Time: {exercise.TimeInSeconds}s";

            // Act
            string result = exercise.ToString();

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Sentence_Property_ReturnsQuestion()
        {
            // Arrange
            var exercise = new FlashcardExercise(1, "TestSentence", "Answer", Difficulty.Normal);

            // Act & Assert
            Assert.AreEqual(exercise.Question, exercise.Sentence);
        }
    }
}
