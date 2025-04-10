using System;

namespace Duo.Models.Exercises;

public class MultipleChoiceAnswerModel
{
    public string Answer { get; set; }
    public bool IsCorrect { get; set; }

    public MultipleChoiceAnswerModel()
    {
    }

    public MultipleChoiceAnswerModel(string answer, bool isCorrect)
    {
        Answer = answer;
        IsCorrect = isCorrect;
    }

    public override string ToString()
    {
        return $"{Answer}{(IsCorrect ? " (Correct)" : string.Empty)}";
    }
}