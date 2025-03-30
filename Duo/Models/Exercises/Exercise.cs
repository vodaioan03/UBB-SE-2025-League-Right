using System;

namespace Duo.Models.Exercises;

public abstract class Exercise
{
    public required int Id { get; set; }
    public required string Question { get; set; }
    public required Difficulty Difficulty { get; set; }

    protected Exercise(int id, string question, Difficulty difficulty)
    {
        Id = id;
        Question = question;
        Difficulty = difficulty;
    }
}