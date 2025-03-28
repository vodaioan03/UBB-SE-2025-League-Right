using Duo.Models;
using System;

namespace Duo.Services;

public class UserService
{
    public User GetById(int userId)
    {
        throw new NotImplementedException();
    }

    public User GetByUsername(string username)
    {
        throw new NotImplementedException();
    }

    public void CreateUser(User user)
    {
        throw new NotImplementedException();
    }

    public int GetLastCompletedSection(int userId)
    {
        throw new NotImplementedException();
    }

    public int GetLastCompletedQuiz(int userId)
    {
        throw new NotImplementedException();
    }

    public void UpdateUserSectionProgress(int userId, int newLastCompletedId)
    {
        throw new NotImplementedException();
    }

    public void UpdateUserQuizProgress(int userId, int newLastQuizCompleted)
    {
        throw new NotImplementedException();
    }
}