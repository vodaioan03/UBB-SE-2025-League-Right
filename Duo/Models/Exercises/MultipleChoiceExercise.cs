using System;
using System.Collections.Generic;

namespace Duo.Models.Exercises;

public class MultipleChoiceExercise : Exercise
{
    public List<(string Answer, bool IsCorrect)> Answers { get; }
    public string CorrectAnswer { get; }

    public MultipleChoiceExercise(int id, string question, Difficulty difficulty, List<(string, bool)> answers, string correctAnswer)
        : base(id, question, difficulty)
    {
        Answers = answers;
        CorrectAnswer = correctAnswer;
    }
}