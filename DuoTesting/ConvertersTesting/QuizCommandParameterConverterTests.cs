using Microsoft.VisualStudio.TestTools.UnitTesting;
using Duo.Converters;
using System;

namespace DuoTesting.Converters
{
    [TestClass]
    public class QuizCommandParameterConverterTests
    {
        private QuizCommandParameterConverter _converter;

        [TestInitialize]
        public void Setup()
        {
            _converter = new QuizCommandParameterConverter();
        }

        [TestMethod]
        public void Convert_ReturnsTuple_WhenValueAndParameterAreCorrectTypes()
        {
            // Arrange
            int quizId = 42;
            bool isExam = true;

            // Act
            var result = _converter.Convert(quizId, null, isExam, null);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ValueTuple<int, bool>));
            var (id, exam) = ((int, bool))result;
            Assert.AreEqual(quizId, id);
            Assert.AreEqual(isExam, exam);
        }

        [TestMethod]
        public void Convert_ReturnsNull_WhenTypesDoNotMatch()
        {
            // Arrange
            string quizId = "not an int";
            string isExam = "not a bool";

            // Act
            var result = _converter.Convert(quizId, null, isExam, null);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void ConvertBack_ThrowsNotImplementedException()
        {
            // Act
            _converter.ConvertBack(null, null, null, null);
        }
    }
}
