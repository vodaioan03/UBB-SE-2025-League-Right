using Duo.Models.Exercises;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuoTesting.ModelTesting.ExercisesTesting
{
    [TestClass]
    public class MultipleChoiceAnswerModelTests
    {
        [TestMethod]
        public void ParameterizedConstructor_SetsProperties()
        {
            // Arrange
            string answer = "Option A";
            bool isCorrect = true;

            // Act
            var model = new MultipleChoiceAnswerModel(answer, isCorrect);

            // Assert
            Assert.AreEqual(answer, model.Answer);
            Assert.IsTrue(model.IsCorrect);
        }

        [TestMethod]
        public void DefaultConstructor_AllowsPropertySetters()
        {
            // Arrange
            var model = new MultipleChoiceAnswerModel();
            model.Answer = "Option B";
            model.IsCorrect = false;

            // Assert
            Assert.AreEqual("Option B", model.Answer);
            Assert.IsFalse(model.IsCorrect);
        }

        [TestMethod]
        public void ToString_ReturnsExpectedFormat_WhenCorrect()
        {
            // Arrange
            var model = new MultipleChoiceAnswerModel("Option C", true);

            // Act
            string result = model.ToString();

            // Assert
            StringAssert.Contains(result, "Option C");
            StringAssert.Contains(result, "(Correct)");
        }

        [TestMethod]
        public void ToString_ReturnsExpectedFormat_WhenNotCorrect()
        {
            // Arrange
            var model = new MultipleChoiceAnswerModel("Option D", false);

            // Act
            string result = model.ToString();

            // Assert
            StringAssert.Contains(result, "Option D");
            Assert.IsFalse(result.Contains("(Correct)"));
        }
    }
}
