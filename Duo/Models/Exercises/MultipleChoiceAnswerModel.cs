using System;

namespace Duo.Models.Exercises;

public class MultipleChoiceAnswerModel
{
    public required string Answer { get; set; }
    public required bool IsCorrect { get; set; }
}