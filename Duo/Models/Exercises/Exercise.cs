using System;

namespace Duo.Models.Exercises;

public abstract class Exercise: IExercise
{
    public int Id { get; }
    public string Question { get; }
    public Difficulty ExerciseDifficulty { get; } = Difficulty.Normal;

    protected Exercise(int id, string question, Difficulty difficulty)
    {
        Id = id;
        Question = question;
        ExerciseDifficulty = difficulty;
    }
}