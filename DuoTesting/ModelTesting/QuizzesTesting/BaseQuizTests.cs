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
    // Dummy concrete implementation of Exercise (since Exercise is abstract)
    public class DummyExercise : Exercise
    {
        public DummyExercise(int id, string question, Difficulty difficulty)
            : base(id, question, difficulty)
        { }
    }

    // Dummy concrete subclass of BaseQuiz for testing purposes
    public class TestQuiz : BaseQuiz
    {
        public TestQuiz(int id, int? sectionId, int maxExercises, double passingThreshold)
            : base(id, sectionId, maxExercises, passingThreshold)
        { }
    }

    [TestClass]
    public class BaseQuizTests
    {
        [TestMethod]
        public void AddExercise_UnderLimit_ReturnsTrueAndIncreasesCount()
        {
            // Arrange
            int maxExercises = 5;
            double passingThreshold = 80;
            var quiz = new TestQuiz(1, 1, maxExercises, passingThreshold);
            var exercise = new DummyExercise(1, "Dummy question", Difficulty.Easy);

            // Act
            bool result = quiz.AddExercise(exercise);

            // Assert
            Assert.IsTrue(result, "Adding an exercise under the limit should return true.");
            Assert.AreEqual(1, quiz.ExerciseList.Count, "ExerciseList count should be incremented.");
        }

        [TestMethod]
        public void AddExercise_AtLimit_ReturnsFalse()
        {
            // Arrange
            int maxExercises = 3;
            var quiz = new TestQuiz(1, 1, maxExercises, 80);

            // Add maximum exercises
            for (int i = 0; i < maxExercises; i++)
            {
                bool added = quiz.AddExercise(new DummyExercise(i, $"Question {i}", Difficulty.Normal));
                Assert.IsTrue(added, $"Exercise {i + 1} should be added.");
            }

            // Act
            bool result = quiz.AddExercise(new DummyExercise(100, "Extra question", Difficulty.Hard));

            // Assert
            Assert.IsFalse(result, "Adding an exercise beyond the limit should return false.");
            Assert.AreEqual(maxExercises, quiz.ExerciseList.Count, "ExerciseList count should equal maxExercises.");
        }

        [TestMethod]
        public void RemoveExercise_RemovesExistingExercise_ReturnsTrue()
        {
            // Arrange
            var quiz = new TestQuiz(1, 1, 5, 80);
            var exercise = new DummyExercise(1, "Test question", Difficulty.Normal);
            quiz.AddExercise(exercise);

            // Act
            bool removed = quiz.RemoveExercise(exercise);

            // Assert
            Assert.IsTrue(removed, "Removing an existing exercise should return true.");
            Assert.AreEqual(0, quiz.ExerciseList.Count, "ExerciseList should be empty after removal.");
        }

        [TestMethod]
        public void RemoveExercise_NonExistingExercise_ReturnsFalse()
        {
            // Arrange
            var quiz = new TestQuiz(1, 1, 5, 80);
            var exercise = new DummyExercise(1, "Test question", Difficulty.Normal);

            // Act
            bool removed = quiz.RemoveExercise(exercise);

            // Assert
            Assert.IsFalse(removed, "Removing a non-existing exercise should return false.");
        }

        [TestMethod]
        public void IsValid_ReturnsTrue_WhenCountEqualsMaxExercises()
        {
            // Arrange
            int maxExercises = 4;
            var quiz = new TestQuiz(1, 1, maxExercises, 80);
            for (int i = 0; i < maxExercises; i++)
            {
                quiz.AddExercise(new DummyExercise(i, $"Question {i}", Difficulty.Normal));
            }

            // Act
            bool isValid = quiz.IsValid();

            // Assert
            Assert.IsTrue(isValid, "Quiz is valid only when exercise count equals maxExercises.");
        }

        [TestMethod]
        public void IsValid_ReturnsFalse_WhenCountNotEqualMaxExercises()
        {
            // Arrange
            int maxExercises = 4;
            var quiz = new TestQuiz(1, 1, maxExercises, 80);
            for (int i = 0; i < maxExercises - 1; i++)
            {
                quiz.AddExercise(new DummyExercise(i, $"Question {i}", Difficulty.Normal));
            }

            // Act
            bool isValid = quiz.IsValid();

            // Assert
            Assert.IsFalse(isValid, "Quiz is not valid if exercise count does not equal maxExercises.");
        }

        [TestMethod]
        public void GetPassingThreshold_ReturnsConstructorValue()
        {
            // Arrange
            double threshold = 85;
            var quiz = new TestQuiz(1, 1, 5, threshold);

            // Act
            double result = quiz.GetPassingThreshold();

            // Assert
            Assert.AreEqual(threshold, result, "Passing threshold should match the constructor value.");
        }

        [TestMethod]
        public void GetNumberOfAnswersGiven_ReturnsExerciseCount()
        {
            // Arrange
            var quiz = new TestQuiz(1, 1, 5, 80);
            int numExercises = 3;
            for (int i = 0; i < numExercises; i++)
            {
                quiz.AddExercise(new DummyExercise(i, $"Question {i}", Difficulty.Easy));
            }

            // Act
            int count = quiz.GetNumberOfAnswersGiven();

            // Assert
            Assert.AreEqual(numExercises, count, "GetNumberOfAnswersGiven should return the count of exercises.");
        }

        [TestMethod]
        public void GetNumberOfCorrectAnswers_InitiallyZero_ThenIncrements()
        {
            // Arrange
            var quiz = new TestQuiz(1, 1, 5, 80);
            Assert.AreEqual(0, quiz.GetNumberOfCorrectAnswers(), "Initial correct answer count should be 0.");

            // Act
            quiz.IncrementCorrectAnswers();
            quiz.IncrementCorrectAnswers();

            // Assert
            Assert.AreEqual(2, quiz.GetNumberOfCorrectAnswers(), "Correct answer count should increment appropriately.");
        }

        [TestMethod]
        public void ToString_ReturnsExpectedFormat()
        {
            // Arrange
            int maxExercises = 3;
            var quiz = new TestQuiz(1, 2, maxExercises, 80);
            // No answers given, so progress should be "Not started"
            for (int i = 0; i < 2; i++)
            {
                quiz.AddExercise(new DummyExercise(i, $"Question {i}", Difficulty.Normal));
            }
            string expected = $"Quiz {quiz.Id} (Section: {quiz.SectionId ?? 0}) - {quiz.ExerciseList.Count}/{maxExercises} exercises - Not started";

            // Act
            string result = quiz.ToString();

            // Assert
            Assert.AreEqual(expected, result, "ToString should return the expected string format.");
        }
    }
}
