using Duo.Models.Exercises;
using Duo.Models.Quizzes;
using Duo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuoTesting.ModelTesting.QuizzesTesting
{
    [TestClass]
    public class QuizTests
    {
        [TestMethod]
        public void Quiz_AddExercise_UnderLimit_ReturnsTrue()
        {
            // Arrange
            var quiz = new Quiz(1, 1, 5); // Quiz(id, sectionId, orderNumber)
            var exercise = new DummyExercise(1, "Dummy question", Difficulty.Easy);

            // Act
            bool result = quiz.AddExercise(exercise);

            // Assert
            Assert.IsTrue(result, "Should be able to add an exercise under the limit.");
            Assert.AreEqual(1, quiz.ExerciseList.Count);
        }

        [TestMethod]
        public void Quiz_AddExercise_AtLimit_ReturnsFalse()
        {
            // Arrange
            var quiz = new Quiz(1, 1, 5);
            // Add maximum (10) exercises for Quiz
            for (int i = 0; i < 10; i++)
            {
                bool added = quiz.AddExercise(new DummyExercise(i, $"Question {i}", Difficulty.Normal));
                Assert.IsTrue(added, $"Exercise {i + 1} should be added successfully.");
            }

            // Act
            bool result = quiz.AddExercise(new DummyExercise(100, "Extra question", Difficulty.Hard));

            // Assert
            Assert.IsFalse(result, "Adding an exercise beyond the limit should fail.");
            Assert.AreEqual(10, quiz.ExerciseList.Count);
        }

        [TestMethod]
        public void Quiz_IsValid_ReturnsFalse_WhenNotFull()
        {
            // Arrange
            var quiz = new Quiz(1, 1, 5);
            for (int i = 0; i < 5; i++)
            {
                quiz.AddExercise(new DummyExercise(i, $"Question {i}", Difficulty.Normal));
            }

            // Act
            bool isValid = quiz.IsValid();

            // Assert
            Assert.IsFalse(isValid, "Quiz is valid only if exactly 10 exercises are added.");
        }

        [TestMethod]
        public void Quiz_IsValid_ReturnsTrue_WhenFull()
        {
            // Arrange
            var quiz = new Quiz(1, 1, 5);
            for (int i = 0; i < 10; i++)
            {
                quiz.AddExercise(new DummyExercise(i, $"Question {i}", Difficulty.Normal));
            }

            // Act
            bool isValid = quiz.IsValid();

            // Assert
            Assert.IsTrue(isValid, "Quiz should be valid when 10 exercises have been added.");
        }

        [TestMethod]
        public void Quiz_GetPassingThreshold_Returns75()
        {
            // Arrange
            var quiz = new Quiz(1, 1, 5);

            // Act
            double threshold = quiz.GetPassingThreshold();

            // Assert
            Assert.AreEqual(75, threshold, "Quiz passing threshold should be 75.");
        }

        [TestMethod]
        public void Quiz_IncrementCorrectAnswers_IncrementsCorrectCount()
        {
            // Arrange
            var quiz = new Quiz(1, 1, 5);
            Assert.AreEqual(0, quiz.GetNumberOfCorrectAnswers());

            // Act
            quiz.IncrementCorrectAnswers();
            quiz.IncrementCorrectAnswers();

            // Assert
            Assert.AreEqual(2, quiz.GetNumberOfCorrectAnswers(), "Correct answer count should be incremented.");
        }

        [TestMethod]
        public void Quiz_ToString_ReturnsExpectedFormat()
        {
            // Arrange
            int id = 1;
            int sectionId = 2;
            int orderNumber = 5;
            var quiz = new Quiz(id, sectionId, orderNumber);
            // Add a few exercises (e.g. 3 exercises)
            for (int i = 0; i < 3; i++)
            {
                quiz.AddExercise(new DummyExercise(i, $"Question {i}", Difficulty.Normal));
            }
            // Expected format:
            // "Quiz {id} (Section: {sectionId}) - {currentCount}/10 exercises - Not started - Order: {orderNumber}"
            string expected = $"Quiz {id} (Section: {sectionId}) - {quiz.ExerciseList.Count}/10 exercises - Not started - Order: {orderNumber}";

            // Act
            string result = quiz.ToString();

            // Assert
            Assert.AreEqual(expected, result);
        }
    }
}
