using Duo.Models;
using System;
using System.Collections.Generic;

namespace Duo.Models.Exercises
{
    public class FlashcardExercise : Exercise
    {
        public string Answer { get; set; }
        public int TimeInSeconds => GetTimeInSeconds();

        public FlashcardExercise(int id, string question, string answer, Difficulty difficulty)
            : base(id, question, difficulty)
        {
            if (string.IsNullOrWhiteSpace(question))
                throw new ArgumentException("Question cannot be empty", nameof(question));
            if (string.IsNullOrWhiteSpace(answer))
                throw new ArgumentException("Answer cannot be empty", nameof(answer));

            Answer = answer;
        }

        private int GetTimeInSeconds()
        {
            return Difficulty switch
            {
                Difficulty.Easy => 45,
                Difficulty.Normal => 30,
                Difficulty.Hard => 15,
                _ => throw new ArgumentException($"Unknown difficulty level: {Difficulty}")
            };
        }

        public bool ValidateAnswer(string userAnswer)
        {
            if (string.IsNullOrWhiteSpace(userAnswer))
                return false;

            return userAnswer.Trim().Equals(Answer.Trim(), StringComparison.OrdinalIgnoreCase);
        }

        public override string ToString()
        {
            return $"{base.ToString()} [Flashcard] Time: {TimeInSeconds}s Answer: {Answer}";
        }
    }
} 