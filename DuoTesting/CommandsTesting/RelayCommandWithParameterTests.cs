using System;
using Duo.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DuoTesting.Commands
{
    [TestClass]
    public class RelayCommandWithParameterTests
    {
        [TestMethod]
        public void Execute_CallsAction_WithCorrectParameter()
        {
            // Arrange
            int receivedValue = 0;
            var command = new RelayCommandWithParameter<int>(x => receivedValue = x);

            // Act
            command.Execute(42);

            // Assert
            Assert.AreEqual(42, receivedValue);
        }

        [TestMethod]
        public void CanExecute_ReturnsTrue_WhenNoPredicateProvided()
        {
            // Arrange
            var command = new RelayCommandWithParameter<string>(s => { });

            // Act
            var result = command.CanExecute("hello");

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CanExecute_UsesPredicate()
        {
            // Arrange
            var command = new RelayCommandWithParameter<int>(x => { }, x => x > 10);

            // Act
            var result1 = command.CanExecute(5);
            var result2 = command.CanExecute(15);

            // Assert
            Assert.IsFalse(result1);
            Assert.IsTrue(result2);
        }

        [TestMethod]
        public void Constructor_ThrowsArgumentNullException_WhenExecuteIsNull()
        {
            // Arrange, Act & Assert
            var ex = Assert.ThrowsException<ArgumentNullException>(() => new RelayCommandWithParameter<int>(null));
            Assert.AreEqual("execute", ex.ParamName);
        }

        [TestMethod]
        public void RaiseCanExecuteChanged_InvokesEvent()
        {
            // Arrange
            var command = new RelayCommandWithParameter<int>(_ => { });
            bool eventRaised = false;

            command.CanExecuteChanged += (s, e) => eventRaised = true;

            // Act
            command.RaiseCanExecuteChanged();

            // Assert
            Assert.IsTrue(eventRaised);
        }

        [TestMethod]
        public void CanExecute_ReturnsFalse_ForInvalidParameterType()
        {
            // Arrange
            var command = new RelayCommandWithParameter<int>(_ => { }, _ => true);

            // Act
            var result = command.CanExecute("not an int");

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Execute_ThrowsArgumentException_ForInvalidParameterType()
        {
            // Arrange
            var command = new RelayCommandWithParameter<int>(_ => { });

            // Act & Assert
            var ex = Assert.ThrowsException<ArgumentException>(() => command.Execute("invalid"));
            StringAssert.Contains(ex.Message, "Invalid parameter type");
        }

        [TestMethod]
        public void Execute_WithNull_Parameter_ForReferenceType()
        {
            // Arrange
            string captured = "initial";
            var command = new RelayCommandWithParameter<string>(s => captured = s);

            // Act
            command.Execute(null);

            // Assert
            Assert.IsNull(captured);
        }

        [TestMethod]
        public void CanExecute_WithNull_Parameter_ForReferenceType()
        {
            // Arrange
            var command = new RelayCommandWithParameter<string>(s => { }, s => s == null);

            // Act
            var result = command.CanExecute(null);

            // Assert
            Assert.IsTrue(result);
        }
    }
}
