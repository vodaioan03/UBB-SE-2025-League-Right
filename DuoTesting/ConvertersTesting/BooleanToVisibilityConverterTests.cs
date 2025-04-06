using Microsoft.VisualStudio.TestTools.UnitTesting;
using Duo.Converters;
using Microsoft.UI.Xaml;
using System;

namespace DuoTesting.Converters
{
    [TestClass]
    public class BooleanToVisibilityConverterTests
    {
        private BooleanToVisibilityConverter _converter;

        [TestInitialize]
        public void Setup()
        {
            _converter = new BooleanToVisibilityConverter();
        }

        [TestMethod]
        public void Convert_ReturnsVisible_WhenValueIsTrue()
        {
            // Act
            var result = _converter.Convert(true, null, null, null);

            // Assert
            Assert.AreEqual(Visibility.Visible, result);
        }

        [TestMethod]
        public void Convert_ReturnsCollapsed_WhenValueIsFalse()
        {
            // Act
            var result = _converter.Convert(false, null, null, null);

            // Assert
            Assert.AreEqual(Visibility.Collapsed, result);
        }

        [TestMethod]
        public void Convert_ReturnsCollapsed_WhenValueIsNotBoolean()
        {
            // Act
            var result = _converter.Convert("not_bool", null, null, null);

            // Assert
            Assert.AreEqual(Visibility.Collapsed, result);
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
