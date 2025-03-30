using Duo.Models;
using System;
using System.Collections.Generic;

namespace Duo.Models.Exercises
{
    public class FlashcardExercise : Exercise
    {
        public string Sentence { get; set; }
        public string Answer { get; set; }
        public int TimeInSeconds { get; set; }

        public FlashcardExercise(int id, string sentence, string answer, int timeInSeconds, Difficulty difficulty)
            : base(id, sentence, difficulty)
        {
            if (string.IsNullOrWhiteSpace(sentence))
                throw new ArgumentException("Sentence cannot be empty", nameof(sentence));
            if (string.IsNullOrWhiteSpace(answer))
                throw new ArgumentException("Answer cannot be empty", nameof(answer));
            if (timeInSeconds <= 0)
                throw new ArgumentException("Time in seconds must be greater than 0", nameof(timeInSeconds));

            Sentence = sentence;
            Answer = answer;
            TimeInSeconds = timeInSeconds;
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