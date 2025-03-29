namespace Duo.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public int LastCompletedSectionId { get; set; }
    public int LastCompletedQuizId { get; set; }
    
    public User(int id, string username, int lastCompletedSectionId = 0, int lastCompletedQuizId = 0)
    {
        Id = id;
        Username = username;
        LastCompletedSectionId = lastCompletedSectionId;
        LastCompletedQuizId = lastCompletedQuizId;
    }

    public User(string username)
    {
        Username = username;
    }
} 