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
            var uniqueUsername = $"progressUser_{Guid.NewGuid()}";
            var user = new User(0, uniqueUsername, 0, 0);
            int userId = await _repository.CreateUserAsync(user);

            using var conn = await DbConnection.CreateConnectionAsync();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "sp_UpdateUserProgress";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", userId);
            cmd.Parameters.AddWithValue("@LastCompletedSectionId", 2); 
            cmd.Parameters.AddWithValue("@LastCompletedQuizId", 3);   

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();

            var updated = await _repository.GetByIdAsync(userId);
            Assert.AreEqual(2, updated.NumberOfCompletedSections);
            Assert.AreEqual(3, updated.NumberOfCompletedQuizzesInSection);
        }


        [TestMethod]
        public async Task IncrementUserProgress_ShouldIncrement()
        {
            var uniqueUsername = $"progressTest_{Guid.NewGuid()}";
            var user = new User(0, uniqueUsername, 0, 0);
            int userId = await _repository.CreateUserAsync(user);

            await _repository.IncrementUserProgressAsync(userId);

            var fetched = await _repository.GetByIdAsync(userId);
            Assert.AreEqual(0, fetched.NumberOfCompletedSections);
        }

        [TestMethod]
        public async Task CreateUser_DuplicateUsername_ShouldThrow()
        {
            var uniqueUsername = $"duplicateuser_{Guid.NewGuid()}";
            var user = new User(0, uniqueUsername, 0, 0);
            int id = await _repository.CreateUserAsync(user);

            var duplicate = new User(0, uniqueUsername, 0, 0);

            await Assert.ThrowsExceptionAsync<Exception>(() => _repository.CreateUserAsync(duplicate));

            var fetched = await _repository.GetByIdAsync(id);
            Assert.AreEqual(uniqueUsername, fetched.Username);
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
        public async Task IncrementUserProgress_InvalidId_ShouldThrow()
        {
            await Assert.ThrowsExceptionAsync<ArgumentException>(() =>
                _repository.IncrementUserProgressAsync(0));
        }
    }
}
