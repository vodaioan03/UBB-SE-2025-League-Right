using System;
using Duo.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DuoTesting.Commands
{
    [TestClass]
    public class RelayCommandTests
    {
        [TestMethod]
        public void Execute_CallsAction()
        {
            // Arrange
            bool wasExecuted = false;
            var command = new RelayCommand(() => wasExecuted = true);

            // Act
            command.Execute(null);

            // Assert
            Assert.IsTrue(wasExecuted);
        }

        [TestMethod]
        public void CanExecute_ReturnsTrue_WhenNoCanExecuteProvided()
        {
            // Arrange
            var command = new RelayCommand(() => { });

            // Act
            var result = command.CanExecute(null);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CanExecute_UsesProvidedCanExecute()
        {
            // Arrange
            var command = new RelayCommand(() => { }, () => false);

            // Act
            var result = command.CanExecute(null);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Constructor_ThrowsArgumentNullException_WhenExecuteIsNull()
        {
            // Arrange, Act & Assert
            var ex = Assert.ThrowsException<ArgumentNullException>(() => new RelayCommand(null));
            Assert.AreEqual("execute", ex.ParamName);
        }

        [TestMethod]
        public void RaiseCanExecuteChanged_InvokesEvent()
        {
            // Arrange
            var command = new RelayCommand(() => { });
            bool eventRaised = false;

            command.CanExecuteChanged += (sender, args) => eventRaised = true;

            // Act
            command.RaiseCanExecuteChanged();

            // Assert
            Assert.IsTrue(eventRaised);
        }
    }
}
