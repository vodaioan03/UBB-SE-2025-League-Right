using System;
using System.Collections.Generic;
using System.Linq;

namespace Duo.Models.Exercises;

public class AssociationExercise : Exercise
{
    public List<string> FirstAnswersList { get; set; }
    public List<string> SecondAnswersList { get; set; }

    public AssociationExercise(int id, string question, Difficulty difficulty, List<string> firstAnswers, List<string> secondAnswers)
        : base(id, question, difficulty)
    {
        if (firstAnswers == null || secondAnswers == null || firstAnswers.Count != secondAnswers.Count)
        {
            throw new ArgumentException("Answer lists must have the same length");
        }

        FirstAnswersList = firstAnswers;
        SecondAnswersList = secondAnswers;
    }

    public bool ValidateAnswer(List<(string, string)> userPairs)
    {
        if (userPairs == null || userPairs.Count != FirstAnswersList.Count)
            return false;

        foreach (var (userA, userB) in userPairs)
        {
            int index = FirstAnswersList.IndexOf(userA);
            if (index == -1 || SecondAnswersList[index] != userB)
                return false;
        }

        return true;
    }
}