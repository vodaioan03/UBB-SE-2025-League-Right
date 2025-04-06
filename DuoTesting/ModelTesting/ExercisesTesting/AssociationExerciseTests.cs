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
    public class AssociationExerciseTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_NullFirstAnswers_ThrowsArgumentException()
        {
            // Arrange
            List<string> secondAnswers = new List<string> { "B" };

            // Act
            var exercise = new AssociationExercise(1, "Test Question", Difficulty.Easy, null, secondAnswers);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_NullSecondAnswers_ThrowsArgumentException()
        {
            // Arrange
            List<string> firstAnswers = new List<string> { "A" };

            // Act
            var exercise = new AssociationExercise(1, "Test Question", Difficulty.Easy, firstAnswers, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_MismatchedAnswerCounts_ThrowsArgumentException()
        {
            // Arrange
            List<string> firstAnswers = new List<string> { "A", "B" };
            List<string> secondAnswers = new List<string> { "X" };

            // Act
            var exercise = new AssociationExercise(1, "Test Question", Difficulty.Easy, firstAnswers, secondAnswers);
        }

        [TestMethod]
        public void ValidateAnswer_NullUserPairs_ReturnsFalse()
        {
            // Arrange
            List<string> firstAnswers = new List<string> { "A", "B" };
            List<string> secondAnswers = new List<string> { "X", "Y" };
            var exercise = new AssociationExercise(1, "Test Question", Difficulty.Easy, firstAnswers, secondAnswers);

            // Act
            bool result = exercise.ValidateAnswer(null);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidateAnswer_IncorrectUserPairsCount_ReturnsFalse()
        {
            // Arrange
            List<string> firstAnswers = new List<string> { "A", "B" };
            List<string> secondAnswers = new List<string> { "X", "Y" };
            var exercise = new AssociationExercise(1, "Test Question", Difficulty.Easy, firstAnswers, secondAnswers);
            var userPairs = new List<(string, string)> { ("A", "X") }; // missing one pair

            // Act
            bool result = exercise.ValidateAnswer(userPairs);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidateAnswer_IncorrectPair_ReturnsFalse()
        {
            // Arrange
            List<string> firstAnswers = new List<string> { "A", "B" };
            List<string> secondAnswers = new List<string> { "X", "Y" };
            var exercise = new AssociationExercise(1, "Test Question", Difficulty.Easy, firstAnswers, secondAnswers);
            var userPairs = new List<(string, string)>
            {
                ("A", "X"),
                ("B", "Z") // wrong match for "B"
            };

            // Act
            bool result = exercise.ValidateAnswer(userPairs);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidateAnswer_AllPairsCorrect_ReturnsTrue()
        {
            // Arrange
            List<string> firstAnswers = new List<string> { "A", "B" };
            List<string> secondAnswers = new List<string> { "X", "Y" };
            var exercise = new AssociationExercise(1, "Test Question", Difficulty.Easy, firstAnswers, secondAnswers);
            var userPairs = new List<(string, string)>
            {
                ("A", "X"),
                ("B", "Y")
            };

            // Act
            bool result = exercise.ValidateAnswer(userPairs);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ToString_ReturnsExpectedFormat()
        {
            // Arrange
            List<string> firstAnswers = new List<string> { "A", "B" };
            List<string> secondAnswers = new List<string> { "X", "Y" };
            var exercise = new AssociationExercise(1, "Test Question", Difficulty.Easy, firstAnswers, secondAnswers);

            // Act
            string result = exercise.ToString();

            // Assert
            StringAssert.Contains(result, "[Association]");
            StringAssert.Contains(result, "A ↔ X");
            StringAssert.Contains(result, "B ↔ Y");
        }
    }
}
