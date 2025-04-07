using Duo.Models;
using Duo.Repositories;
using Duo.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;

namespace DuoTesting.Services
{
    [TestClass]
    public class UserServiceUT
    {
        private Mock<IUserRepository> mockRepository;  // Mock the interface, not the concrete class
        private UserService service;
        public class TestUser : User
        {
            public TestUser(int id = 1, string username = "testuser", int completedSections = 0, int completedQuizzes = 0)
                : base(id, username, completedSections, completedQuizzes)
            {
            }
        }
        [TestInitialize]
        public void Setup()
        {
            mockRepository = new Mock<IUserRepository>();  // Use interface
            service = new UserService(mockRepository.Object);
        }

        [TestMethod]
        public async Task GetByIdAsync_ValidId_ReturnsUser()
        {
            var user = new TestUser();
            mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(user);

            var result = await service.GetByIdAsync(1);

            Assert.AreEqual(user, result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task GetByIdAsync_InvalidId_ThrowsException()
        {
            await service.GetByIdAsync(0);
        }

        [TestMethod]
        public async Task GetByUsernameAsync_ValidUsername_ReturnsUser()
        {
            var user = new TestUser();
            mockRepository.Setup(r => r.GetByUsernameAsync("testuser")).ReturnsAsync(user);

            var result = await service.GetByUsernameAsync("testuser");

            Assert.AreEqual(user, result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task GetByUsernameAsync_InvalidUsername_ThrowsException()
        {
            await service.GetByUsernameAsync("");
        }

        [TestMethod]
        public async Task CreateUserAsync_ValidUser_ReturnsUserId()
        {
            var user = new TestUser();
            mockRepository.Setup(r => r.CreateUserAsync(user)).ReturnsAsync(1);

            var result = await service.CreateUserAsync(user);

            Assert.AreEqual(1, result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task CreateUserAsync_UserWithoutUsername_ThrowsException()
        {
            var user = new TestUser(1, "");
            await service.CreateUserAsync(user);
        }

        [TestMethod]
        public async Task UpdateUserSectionProgressAsync_ValidParams_CallsRepository()
        {
            await service.UpdateUserSectionProgressAsync(1, 2, 3);
            mockRepository.Verify(r => r.UpdateUserProgressAsync(1, 2, 3), Times.Once);
        }

        [TestMethod]
        public async Task IncrementUserProgressAsync_ValidId_CallsRepository()
        {
            await service.IncrementUserProgressAsync(1);
            mockRepository.Verify(r => r.IncrementUserProgressAsync(1), Times.Once);
        }
    }
}
