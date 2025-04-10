using System;
using System.Threading.Tasks;
using Duo.Data;
using Duo.Models;
using Duo.Repositories;

namespace Duo.Services;

public class UserService : IUserService
{
    private readonly IUserRepository userRepository;

    public UserService(IUserRepository userRepository)
    {
        this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    public async Task<User> GetByIdAsync(int userId)
    {
        if (userId <= 0)
        {
            throw new ArgumentException("User ID must be greater than 0.", nameof(userId));
        }

        return await userRepository.GetByIdAsync(userId);
    }

    public async Task<User> GetByUsernameAsync(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new ArgumentException("Username cannot be null or empty.", nameof(username));
        }

        return await userRepository.GetByUsernameAsync(username);
    }

    public async Task<int> CreateUserAsync(User user)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        if (string.IsNullOrWhiteSpace(user.Username))
        {
            throw new ArgumentException("Username cannot be null or empty.", nameof(user));
        }

        return await userRepository.CreateUserAsync(user);
    }

    public async Task UpdateUserSectionProgressAsync(int userId, int newNrOfSectionsCompleted, int newNrOfQuizzesInSectionCompleted)
    {
        if (userId <= 0)
        {
            throw new ArgumentException("User ID must be greater than 0.", nameof(userId));
        }

        await userRepository.UpdateUserProgressAsync(userId, newNrOfSectionsCompleted, newNrOfQuizzesInSectionCompleted);
    }

    public async Task IncrementUserProgressAsync(int userId)
    {
        if (userId <= 0)
        {
            throw new ArgumentException("User ID must be greater than 0.", nameof(userId));
        }

        await userRepository.IncrementUserProgressAsync(userId);
    }
}