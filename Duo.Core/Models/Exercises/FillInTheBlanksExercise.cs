using System;
using System.Collections.Generic;

namespace Duo.Models.Exercises;
public class FillInTheBlankExercise : Exercise
{
    public List<string> PossibleCorrectAnswers { get; }

    public FillInTheBlankExercise(int id, string question, Difficulty difficulty, List<string> possibleCorrectAnswers)
        : base(id, question, difficulty)
    {
        PossibleCorrectAnswers = possibleCorrectAnswers;
    }
}