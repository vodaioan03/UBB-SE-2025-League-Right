using Duo.Models;
using Duo.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DuoTesting.MockClasses
{
    public class InMemoryUserRepository : IUserRepository
    {
        private readonly Dictionary<int, User> _users = new();
        private readonly HashSet<string> _usernames = new();
        private int _nextId = 1;

        public Task<int> CreateUserAsync(User user)
        {
            if (user is null)
                throw new ArgumentNullException(nameof(user));

            if (_usernames.Contains(user.Username))
                throw new Exception("Username already exists.");

            var newUser = new User(_nextId++, user.Username)
            {
                NumberOfCompletedSections = user.NumberOfCompletedSections,
                NumberOfCompletedQuizzesInSection = user.NumberOfCompletedQuizzesInSection
            };

            _users[newUser.Id] = newUser;
            _usernames.Add(newUser.Username);
            return Task.FromResult(newUser.Id);
        }



        public Task<User> GetByUsernameAsync(string username)
        {
            var user = _users.Values.FirstOrDefault(u => u.Username == username);
            if (user == null)
                throw new KeyNotFoundException("User not found.");
            return Task.FromResult(user);
        }

        public Task<User> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid ID.");

            if (!_users.TryGetValue(id, out var user))
                throw new KeyNotFoundException("User not found.");

            return Task.FromResult(user);
        }

        public Task IncrementUserProgressAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid ID.");

            if (!_users.TryGetValue(id, out var user))
                throw new KeyNotFoundException("User not found.");

            user.NumberOfCompletedQuizzesInSection += 1;
            return Task.CompletedTask;
        }

        public Task UpdateUserProgressAsync(int id, int lastCompletedSectionId, int lastCompletedQuizId)
        {
            if (!_users.TryGetValue(id, out var user))
                throw new KeyNotFoundException("User not found.");

            user.NumberOfCompletedSections = lastCompletedSectionId;
            user.NumberOfCompletedQuizzesInSection = lastCompletedQuizId;
            return Task.CompletedTask;
        }
    }
}
