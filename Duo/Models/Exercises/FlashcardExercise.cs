using System;
using Duo.ViewModels.Base;

namespace Duo.Models.Exercises
{
    public class FlashcardExercise : ViewModelBase, IExercise
    {
        public int Id { get; }
        public Difficulty ExerciseDifficulty { get; }
        public string Type { get; } = "Flashcard";

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
            Id = id;
            _topic = topic;
            _question = question;
            _answer = answer;
            ExerciseDifficulty = difficulty;
        }
    }
} 