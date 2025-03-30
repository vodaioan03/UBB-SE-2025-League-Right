using System;

namespace Duo.Models.Exercises;

public abstract class Exercise
{
    public int Id { get; set; }
    public string Question { get; set; }
    public Difficulty Difficulty { get; set; }

    protected Exercise(int id, string question, Difficulty difficulty)
    {
        Id = id;
        Question = question;
        Difficulty = difficulty;
    }
}