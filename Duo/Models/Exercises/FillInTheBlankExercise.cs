using System;
using System.Collections.Generic;
using System.Linq;

namespace Duo.Models.Exercises;

public class FillInTheBlankExercise : Exercise
{
    public List<string> PossibleCorrectAnswers { get; set; }

    public FillInTheBlankExercise(int id, string question, Difficulty difficulty, List<string> possibleCorrectAnswers)
        : base(id, question, difficulty)
    {
        if (possibleCorrectAnswers == null || possibleCorrectAnswers.Count == 0)
        {
            throw new ArgumentException("Answers cannot be empty", nameof(possibleCorrectAnswers));
        }

        PossibleCorrectAnswers = possibleCorrectAnswers;
    }

    public bool ValidateAnswer(List<string> userAnswers)
    {
        if (userAnswers == null || userAnswers.Count != PossibleCorrectAnswers.Count)
            return false;

        for (int i = 0; i < PossibleCorrectAnswers.Count; i++)
        {
            if (!string.Equals(userAnswers[i].Trim(), PossibleCorrectAnswers[i].Trim(), StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
        }
        return true;
    }

    public override string ToString()
    {
        var answers = string.Join(", ", PossibleCorrectAnswers);
        return $"{base.ToString()} [Fill in the Blank] Correct Answers: {answers}";
    }
}