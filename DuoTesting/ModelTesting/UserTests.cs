using Duo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuoTesting.ModelTesting
{
    [TestClass]
    public class UserTests
    {
        [TestMethod]
        public void Constructor_WithAllParameters_ShouldAssignValues()
        {
            // Arrange
            int expectedId = 10;
            string expectedUsername = "TestUser";
            int expectedCompletedSections = 5;
            int expectedCompletedQuizzes = 3;

            // Act
            User user = new User(expectedId, expectedUsername, expectedCompletedSections, expectedCompletedQuizzes);

            // Assert
            Assert.AreEqual(expectedId, user.Id);
            Assert.AreEqual(expectedUsername, user.Username);
            Assert.AreEqual(expectedCompletedSections, user.NumberOfCompletedSections);
            Assert.AreEqual(expectedCompletedQuizzes, user.NumberOfCompletedQuizzesInSection);
        }

        [TestMethod]
        public void Constructor_WithUsernameAndId_ShouldAssignUsernameAndDefaults()
        {
            // Arrange
            string expectedUsername = "SimpleUser";
            int expectedId = 10;

            // Act
            User user = new User(expectedId, expectedUsername);

            // Assert
            Assert.AreEqual(expectedId, user.Id);
            Assert.AreEqual(expectedUsername, user.Username);
            Assert.AreEqual(0, user.NumberOfCompletedSections);
            Assert.AreEqual(0, user.NumberOfCompletedQuizzesInSection);
        }

        [TestMethod]
        public void Properties_CanBeModified_AfterConstruction()
        {
            // Arrange
            User user = new User("InitialUser");

            // Act
            user.Id = 99;
            user.Username = "ModifiedUser";
            user.NumberOfCompletedSections = 7;
            user.NumberOfCompletedQuizzesInSection = 4;

            // Assert
            Assert.AreEqual(99, user.Id);
            Assert.AreEqual("ModifiedUser", user.Username);
            Assert.AreEqual(7, user.NumberOfCompletedSections);
            Assert.AreEqual(4, user.NumberOfCompletedQuizzesInSection);
        }
    }
}
