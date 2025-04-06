using Duo.Models.Exercises;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuoTesting.ModelTesting.ExercisesTesting
{
    [TestClass]
    public class ExerciseTypesTests
    {
        [TestMethod]
        public void EXERCISE_TYPES_ContainsExpectedValues()
        {
            // Arrange
            var expected = new List<string>
            {
                "Association",
                "Fill in the blank",
                "Multiple Choice",
                "Flashcard"
            };

            // Act
            var actual = ExerciseTypes.EXERCISE_TYPES;

            // Assert
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
