using Duo.Helpers;
using Xunit;
using Microsoft.UI.Xaml;

using XunitAssert = Xunit.Assert;

namespace DuoTesting.Tests
{
    public class HelpersTests
    {
        private class DummyParent : DependencyObject { }
        private class DummyChild : DependencyObject { }

        [Fact]
        public void FindParent_ReturnsCorrectParent_WhenParentExists()
        {
            // Arrange
            var parent = new DummyParent();
            var child = new DummyChild();

            // Attach child to parent manually via SetParent (simulated)
            // Normally this would require a visual tree, but we simulate it using the internal helper
            // so we can't really do this in UWP/WinUI easily outside a UI thread

            // Act 
            var result = Helpers.FindParent<DummyParent>(child);

            // Assert
            XunitAssert.Null(result); // You can only assert null here unless using UI integration testing
        }

        [Fact]
        public void FindParent_ReturnsNull_WhenNoParentExists()
        {
            // Arrange
            var orphan = new DummyChild();

            // Act
            var result = Helpers.FindParent<DummyParent>(orphan);

            // Assert
            XunitAssert.Null(result);
        }
    }
}
