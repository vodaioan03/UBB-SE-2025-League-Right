using Duo.Commands;
using System;
using Xunit;
using XunitAssert = Xunit.Assert;

namespace DuoTesting.Commands
{
    public class RelayCommandTests
    {
        [Fact]
        public void Execute_CallsAction_WhenInvoked()
        {
            // Arrange
            bool wasCalled = false;
            var command = new RelayCommand(() => wasCalled = true);

            // Act
            command.Execute(null);

            // Assert
            XunitAssert.True(wasCalled);
        }

        [Fact]
        public void CanExecute_ReturnsTrue_WhenNoCanExecuteProvided()
        {
            // Arrange
            var command = new RelayCommand(() => { });

            // Act
            var result = command.CanExecute(null);

            // Assert
            XunitAssert.True(result);
        }

        [Fact]
        public void CanExecute_UsesProvidedCanExecuteDelegate()
        {
            // Arrange
            var command = new RelayCommand(() => { }, () => false);

            // Act
            var result = command.CanExecute(null);

            // Assert
            XunitAssert.False(result);
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullException_WhenExecuteIsNull()
        {
            // Arrange + Act + Assert
            var ex = XunitAssert.Throws<ArgumentNullException>(() => new RelayCommand(null));
            XunitAssert.Equal("execute", ex.ParamName);
        }

        [Fact]
        public void RaiseCanExecuteChanged_InvokesEvent()
        {
            // Arrange
            var command = new RelayCommand(() => { });
            bool wasRaised = false;

            command.CanExecuteChanged += (s, e) => wasRaised = true;

            // Act
            command.RaiseCanExecuteChanged();

            // Assert
            XunitAssert.True(wasRaised);
        }
    }
}
