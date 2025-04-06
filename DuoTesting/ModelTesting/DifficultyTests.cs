using Duo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuoTesting.ModelTesting
{
    [TestClass]
    public class DifficultyTests
    {
        [TestMethod]
        public void Enum_Values_AreCorrect()
        {
            // Assert that each enum value has the expected integer value.
            Assert.AreEqual(1, (int)Difficulty.Easy, "Difficulty.Easy should have the value 1");
            Assert.AreEqual(2, (int)Difficulty.Normal, "Difficulty.Normal should have the value 2");
            Assert.AreEqual(3, (int)Difficulty.Hard, "Difficulty.Hard should have the value 3");
        }

        [TestMethod]
        public void DifficultyList_Contains_CorrectDifficulties()
        {
            // Arrange
            var expectedDifficulties = new List<string> { "Easy", "Normal", "Hard" };

            // Act
            List<string> actualDifficulties = DifficultyList.DIFFICULTIES;

            // Assert
            CollectionAssert.AreEqual(expectedDifficulties, actualDifficulties, "The DIFFICULTIES list does not match the expected values");
        }
    }
}
