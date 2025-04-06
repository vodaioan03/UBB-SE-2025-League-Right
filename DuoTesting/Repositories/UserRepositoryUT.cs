using Microsoft.VisualStudio.TestTools.UnitTesting;
using Duo.Repositories;
using Duo.Models;
using DuoTesting.Helper;
using System;
using System.Threading.Tasks;

namespace DuoTesting.Repositories
{
    [TestClass]
    public class UserRepositoryUT : TestBase
    {
        private IUserRepository _repository = null!;
        private int _userId;
        private string _username;

        [TestInitialize]
        public async Task Setup()
        {
            base.BaseSetup();
            _repository = new UserRepository(DbConnection);
            _username = $"TestUser_{Guid.NewGuid()}";
            _userId = await _repository.CreateUserAsync(new User(0, _username, 0, 0));
        }

        [TestCleanup]
        public async Task Cleanup()
        {
            using var conn = await DbConnection.CreateConnectionAsync();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM Users WHERE Username = @username";
            cmd.Parameters.AddWithValue("@username", _username);
            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        [TestMethod]
        public async Task GetByUsername_ShouldReturnCorrectUser()
        {
            var user = await _repository.GetByUsernameAsync(_username);
            Assert.AreEqual(_username, user.Username);
        }

        [TestMethod]
        public async Task GetById_ShouldReturnCorrectUser()
        {
            var user = await _repository.GetByIdAsync(_userId);
            Assert.AreEqual(_userId, user.Id);
        }

        [TestMethod]
        public async Task UpdateUserProgress_ShouldUpdateCorrectly()
        {
            await _repository.UpdateUserProgressAsync(_userId, 3, 5);
            var updated = await _repository.GetByIdAsync(_userId);
            Assert.AreEqual(3, updated.NumberOfCompletedSections);
            Assert.AreEqual(5, updated.NumberOfCompletedQuizzesInSection);
        }

        [TestMethod]
        public async Task IncrementUserProgress_ShouldIncrement()
        {
            await _repository.IncrementUserProgressAsync(_userId);
            var user = await _repository.GetByIdAsync(_userId);
            Assert.AreEqual(1, user.NumberOfCompletedSections); // default is 0 → +1
        }

        [TestMethod]
        public async Task CreateUser_DuplicateUsername_ShouldThrow()
        {
            var duplicateUser = new User(0, _username, 0, 0);

            await Assert.ThrowsExceptionAsync<InvalidOperationException>(() =>
                _repository.CreateUserAsync(duplicateUser));
        }

        [TestMethod]
        public async Task GetByUsername_Invalid_ShouldThrow()
        {
            await Assert.ThrowsExceptionAsync<KeyNotFoundException>(() =>
                _repository.GetByUsernameAsync("nonexistent_user"));
        }

        [TestMethod]
        public async Task GetById_Invalid_ShouldThrow()
        {
            await Assert.ThrowsExceptionAsync<ArgumentException>(() =>
                _repository.GetByIdAsync(0));
        }

        [TestMethod]
        public async Task UpdateUserProgress_InvalidId_ShouldThrow()
        {
            await Assert.ThrowsExceptionAsync<ArgumentException>(() =>
                _repository.UpdateUserProgressAsync(0, 1, 1));
        }

        [TestMethod]
        public async Task IncrementUserProgress_InvalidId_ShouldThrow()
        {
            await Assert.ThrowsExceptionAsync<ArgumentException>(() =>
                _repository.IncrementUserProgressAsync(0));
        }
    }
}
