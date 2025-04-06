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
    public class MultipleChoiceExerciseTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_NullChoices_ThrowsArgumentException()
        {
            // Act
            var exercise = new MultipleChoiceExercise(1, "Question", Difficulty.Normal, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_NoCorrectChoice_ThrowsArgumentException()
        {
            // Arrange
            var choices = new List<MultipleChoiceAnswerModel>
            {
                new MultipleChoiceAnswerModel("Option 1", false),
                new MultipleChoiceAnswerModel("Option 2", false)
            };

            // Act
            var exercise = new MultipleChoiceExercise(1, "Question", Difficulty.Normal, choices);
        }

        [TestMethod]
        public void ValidateAnswer_NullUserAnswers_ReturnsFalse()
        {
            // Arrange
            var choices = new List<MultipleChoiceAnswerModel>
            {
                new MultipleChoiceAnswerModel("Option 1", true),
                new MultipleChoiceAnswerModel("Option 2", false)
            };
            var exercise = new MultipleChoiceExercise(1, "Question", Difficulty.Normal, choices);

            // Act
            bool result = exercise.ValidateAnswer(null);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidateAnswer_EmptyUserAnswers_ReturnsFalse()
        {
            // Arrange
            var choices = new List<MultipleChoiceAnswerModel>
            {
                new MultipleChoiceAnswerModel("Option 1", true),
                new MultipleChoiceAnswerModel("Option 2", false)
            };
            var exercise = new MultipleChoiceExercise(1, "Question", Difficulty.Normal, choices);

            // Act
            bool result = exercise.ValidateAnswer(new List<string>());

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidateAnswer_IncorrectSelection_ReturnsFalse()
        {
            // Arrange
            var choices = new List<MultipleChoiceAnswerModel>
            {
                new MultipleChoiceAnswerModel("Option A", true),
                new MultipleChoiceAnswerModel("Option B", false),
                new MultipleChoiceAnswerModel("Option C", true)
            };
            var exercise = new MultipleChoiceExercise(1, "Question", Difficulty.Normal, choices);

            // Act
            // Provide only one of the two correct answers
            bool result = exercise.ValidateAnswer(new List<string> { "Option A" });

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidateAnswer_CorrectSelection_DifferentOrder_ReturnsTrue()
        {
            // Arrange
            var choices = new List<MultipleChoiceAnswerModel>
            {
                new MultipleChoiceAnswerModel("Option A", true),
                new MultipleChoiceAnswerModel("Option B", false),
                new MultipleChoiceAnswerModel("Option C", true)
            };
            var exercise = new MultipleChoiceExercise(1, "Question", Difficulty.Normal, choices);

            // Act
            // Provide correct answers in reverse order; sorting should make order irrelevant.
            bool result = exercise.ValidateAnswer(new List<string> { "Option C", "Option A" });

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ToString_ReturnsExpectedFormat()
        {
            // Arrange
            var choices = new List<MultipleChoiceAnswerModel>
            {
                new MultipleChoiceAnswerModel("Option A", true),
                new MultipleChoiceAnswerModel("Option B", false)
            };
            var exercise = new MultipleChoiceExercise(1, "Question", Difficulty.Normal, choices);

            // Act
            string result = exercise.ToString();

            // Assert
            StringAssert.Contains(result, "[Multiple Choice]");
            StringAssert.Contains(result, "Option A (Correct)");
            StringAssert.Contains(result, "Option B");
        }
    }
}
