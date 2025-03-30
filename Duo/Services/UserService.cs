using Duo.Data;
using Duo.Models;
using Duo.Repositories;
using System;
using System.Threading.Tasks;

namespace Duo.Services;

public class UserService
{
    private readonly UserRepository _userRepository;

    public UserService(DatabaseConnection databaseConnection)
    {
        _userRepository = new UserRepository(databaseConnection);
    }

    public Task<User> GetById(int userId)
    {
        return  _userRepository.GetByIdAsync(userId);
    }

    public Task<User> GetByUsername(string username)
    {
        return _userRepository.GetByUsernameAsync(username);
    }

    public Task<bool> CreateUser(User user)
    {
        return _userRepository.CreateUserAsync(user);
    }

    public async Task<int> GetLastCompletedSection(int userId)
    {
        User user = await _userRepository.GetByIdAsync(userId);
        return user.LastCompletedSectionId;
    }

    public async Task<int> GetLastCompletedQuiz(int userId)
    {
        User user = await _userRepository.GetByIdAsync(userId);
        return user.LastCompletedQuizId;
    }

    public async void UpdateUserSectionProgress(int userId, int newLastCompletedId)
    {
        User user = await _userRepository.GetByIdAsync(userId);
        await _userRepository.UpdateUserSectionProgressAsync(user, newLastCompletedId);
    }

    public async void UpdateUserQuizProgress(int userId, int newLastQuizCompleted)
    {
        User user = await _userRepository.GetByIdAsync(userId);
        await _userRepository.UpdateUserQuizProgressAsync(user, newLastQuizCompleted);
    }
}