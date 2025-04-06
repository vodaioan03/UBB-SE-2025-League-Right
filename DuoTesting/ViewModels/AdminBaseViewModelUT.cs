using Duo.ViewModels;

namespace DuoTesting.ViewModels
{

    [TestClass]
    public class AdminBaseViewModelUT
    {
        [TestMethod]
        public void GoBack_ShouldInvokeRequestGoBack()
        {
            // Arrange
            var viewModel = new AdminBaseViewModel();
            bool eventInvoked = false;
            viewModel.RequestGoBack += (sender, args) => eventInvoked = true;
            // Act
            viewModel.GoBack();
            // Assert
            Assert.IsTrue(eventInvoked);
        }

        [TestMethod]
        public void RaiseErrorMessage_ShouldInvokeShowErrorMessageRequested()
        {
            // Arrange
            var viewModel = new AdminBaseViewModel();
            bool eventInvoked = false;
            string expectedTitle = "Error";
            string expectedMessage = "An error occurred.";
            viewModel.ShowErrorMessageRequested += (sender, args) =>
            {
                eventInvoked = true;
                Assert.AreEqual(expectedTitle, args.Title);
                Assert.AreEqual(expectedMessage, args.Message);
            };
            // Act
            viewModel.RaiseErrorMessage(expectedTitle, expectedMessage);
            // Assert
            Assert.IsTrue(eventInvoked);
        }
    }
}
