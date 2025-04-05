using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Duo.Models;
using Duo.Models.Exercises;
using Duo.ViewModels.ExerciseViewModels;

namespace Duo.ViewModels.CreateExerciseViewModels
{
    partial class CreateFlashcardExerciseViewModel : CreateExerciseViewModelBase
    {
        private string answer = string.Empty;

        public CreateFlashcardExerciseViewModel()
        {
        }

        public string Answer
        {
            get => answer;
            set
            {
                if (answer != value)
                {
                    answer = value;
                    OnPropertyChanged(nameof(Answer)); // Notify UI
                }
            }
        }
        public override Exercise CreateExercise(string question, Difficulty difficulty)
        {
            return new FlashcardExercise(0, question, Answer, difficulty);
        }
    }
}
