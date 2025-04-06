using Duo.Commands;
using System;
using Xunit;

using XunitAssert = Xunit.Assert;

namespace DuoTesting.Commands
{
    public class RelayCommandWithParameterTests
    {
        [Fact]
        public void Constructor_ThrowsArgumentNullException_WhenExecuteIsNull()
        {
            // Arrange + Act + Assert: Ensure ArgumentNullException is thrown when action is null
            var ex = XunitAssert.Throws<ArgumentNullException>(() =>
                new RelayCommandWithParameter<string>(null));
            XunitAssert.Equal("execute", ex.ParamName);
        }

        [Fact]
        public void CanExecute_ReturnsTrue_WhenCanExecuteIsNull()
        {
            // Arrange: Create command without predicate
            var command = new RelayCommandWithParameter<string>(_ => { });

            // Act: Call CanExecute with any input
            var result = command.CanExecute("test");

            // Assert: Should return true by default
            XunitAssert.True(result);
        }

        [Fact]
        public void CanExecute_UsesProvidedPredicate()
        {
            // Arrange: Only allow "allowed" as valid input
            var command = new RelayCommandWithParameter<string>(_ => { }, param => param == "allowed");

            // Assert: "allowed" is valid, others aren't
            XunitAssert.True(command.CanExecute("allowed"));
            XunitAssert.False(command.CanExecute("denied"));
        }

        [Fact]
        public void CanExecute_ReturnsFalse_WhenValueTypeParameterIsNull()
        {
            // Arrange: For value types like int, null input is invalid
            var command = new RelayCommandWithParameter<int>(_ => { }, _ => true);

            // Act: Call CanExecute with null
            var result = command.CanExecute(null);

            // Assert: Should return false to prevent crash on casting
            XunitAssert.False(result);
        }

        [Fact]
        public void Execute_CallsAction_WithCorrectParameter()
        {
            // Arrange: Capture the parameter passed to the action
            string captured = null;
            var command = new RelayCommandWithParameter<string>(param => captured = param);

            // Act: Execute with "hello"
            command.Execute("hello");

            // Assert: Action was called with correct argument
            XunitAssert.Equal("hello", captured);
        }

        [Fact]
        public void Execute_ThrowsArgumentException_WhenParameterIsInvalid()
        {
            // Arrange: Command expects an int, not a string
            var command = new RelayCommandWithParameter<int>(_ => { });

            // Act + Assert: Should throw due to wrong type casting
            var ex = XunitAssert.Throws<ArgumentException>(() => command.Execute("not an int"));
            XunitAssert.Contains("Invalid parameter type", ex.Message);
        }

        [Fact]
        public void RaiseCanExecuteChanged_InvokesEvent()
        {
            // Arrange: Subscribe to CanExecuteChanged event
            var command = new RelayCommandWithParameter<string>(_ => { });
            bool wasRaised = false;
            command.CanExecuteChanged += (_, _) => wasRaised = true;

            // Act: Raise event
            command.RaiseCanExecuteChanged();

            // Assert: Event handler was triggered
            XunitAssert.True(wasRaised);
        }
    }
}