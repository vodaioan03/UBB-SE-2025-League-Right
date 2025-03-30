using System;

namespace Duo.Models.Exercises;

public class MultipleChoiceAnswerModel
{
    public string Answer { get; set; }
    public bool IsCorrect { get; set; }

    public override string ToString()
    {
        return $"{Answer}{(IsCorrect ? " (Correct)" : "")}";
    }
}