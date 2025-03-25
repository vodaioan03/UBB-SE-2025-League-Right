using System;
using System.Collections.Generic;

namespace Duo.Models.Exercises;

public class AssociationExercise : Exercise
{
    public List<string> FirstAnswersList { get; set;  }
    public List<string> SecondAnswersList { get; set; }

    public AssociationExercise(int id, string question, Difficulty difficulty, List<string> firstAnswers, List<string> secondAnswers)
        : base(id, question, difficulty)
    {
        FirstAnswersList = firstAnswers;
        SecondAnswersList = secondAnswers;
    }
}