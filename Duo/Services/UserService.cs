using Duo.Data;
using Duo.Models;
using Duo.Repositories;
using System;
using System.Threading.Tasks;

namespace Duo.Services;

public class UserService
{
    private readonly UserRepository _userRepository;

    public UserService(UserRepository userRepository)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    public async Task<User> GetByIdAsync(int userId)
    {
        if (userId <= 0)
        {
            throw new ArgumentException("User ID must be greater than 0.", nameof(userId));
        }

        return await _userRepository.GetByIdAsync(userId);
    }

    public async Task<User> GetByUsernameAsync(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new ArgumentException("Username cannot be null or empty.", nameof(username));
        }

        return await _userRepository.GetByUsernameAsync(username);
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

        return await _userRepository.CreateUserAsync(user);
    }

    // maybe deprecate
    public async Task<int> GetLastCompletedSectionAsync(int userId)
    {
        if (userId <= 0)
        {
            throw new ArgumentException("User ID must be greater than 0.", nameof(userId));
        }

        var user = await _userRepository.GetByIdAsync(userId);
        return user.NumberOfCompletedSections;
    }

    // maybe deprecate
    public async Task<int> GetLastCompletedQuizAsync(int userId)
    {
        if (userId <= 0)
        {
            throw new ArgumentException("User ID must be greater than 0.", nameof(userId));
        }

        var user = await _userRepository.GetByIdAsync(userId);
        return user.NumberOfCompletedQuizzesInSection;
    }

    public async Task UpdateUserSectionProgressAsync(int userId, int newNrOfSectionsCompleted, int newNrOfQuizzesInSectionCompleted)
    {
        if (userId <= 0)
        {
            throw new ArgumentException("User ID must be greater than 0.", nameof(userId));
        }

        await _userRepository.UpdateUserProgressAsync(userId, newNrOfSectionsCompleted, newNrOfQuizzesInSectionCompleted);
    }

    public async Task IncrementUserProgressAsync(int userId)
    {
        if (userId <= 0)
        {
            throw new ArgumentException("User ID must be greater than 0.", nameof(userId));
        }

        await _userRepository.IncrementUserProgressAsync(userId);
    }
}