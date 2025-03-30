using System;
using Duo.ViewModels.Base;

namespace Duo.Models.Exercises
{
    public class FlashcardExercise : ViewModelBase, IExercise
    {
        public int Id { get; }
        public Difficulty ExerciseDifficulty { get; }
        public string Type { get; } = "Flashcard";
        public int TimeInSeconds { get; }

        private string _topic;
        public string Topic
        {
            get => _topic;
            set => SetProperty(ref _topic, value);
        }

        private string _question;
        public string Question
        {
            get => _question;
            set => SetProperty(ref _question, value);
        }

        private string _answer;
        public string Answer
        {
            get => _answer;
            set => SetProperty(ref _answer, value);
        }

        private TimeSpan _elapsedTime;
        public TimeSpan ElapsedTime
        {
            get => _elapsedTime;
            set => SetProperty(ref _elapsedTime, value);
        }

        public FlashcardExercise(int id, string question, string answer, Difficulty difficulty = Difficulty.Normal) 
            : this(id, null, question, answer, difficulty)
        {
        }
        
        public FlashcardExercise(int id, string topic, string question, string answer, Difficulty difficulty = Difficulty.Normal) 
        {
            // Validation from develop branch
            if (string.IsNullOrWhiteSpace(question))
                throw new ArgumentException("Question cannot be empty", nameof(question));
            if (string.IsNullOrWhiteSpace(answer))
                throw new ArgumentException("Answer cannot be empty", nameof(answer));
                
            Id = id;
            _topic = topic;
            _question = question;
            _answer = answer;
            ExerciseDifficulty = difficulty;
            
            // Default time based on difficulty
            TimeInSeconds = GetDefaultTimeForDifficulty(difficulty);
        }
        
        // Helper method to determine default time based on difficulty
        private int GetDefaultTimeForDifficulty(Difficulty difficulty)
        {
            return difficulty switch
            {
                Difficulty.Easy => 15,
                Difficulty.Normal => 30,
                Difficulty.Hard => 45,
                _ => 30
            };
        }
        
        // Validation method from develop branch
        public bool ValidateAnswer(string userAnswer)
        {
            if (string.IsNullOrWhiteSpace(userAnswer))
                return false;

            return userAnswer.Trim().Equals(Answer.Trim(), StringComparison.OrdinalIgnoreCase);
        }

        public override string ToString()
        {
            return $"Id: {Id}, Topic: {Topic}, Difficulty: {ExerciseDifficulty}, Time: {TimeInSeconds}s";
        }
    }
} 