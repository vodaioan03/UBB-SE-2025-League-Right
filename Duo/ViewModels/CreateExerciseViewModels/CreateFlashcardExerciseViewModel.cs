using Duo.Models;
using Duo.Models.Exercises;
using Duo.ViewModels.ExerciseViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Duo.ViewModels.CreateExerciseViewModels
{
    class CreateFlashcardExerciseViewModel : CreateExerciseViewModelBase
    {
        private string _answer;

        public CreateFlashcardExerciseViewModel()
        {

        }
        public string Answer
        {
            get => _answer;
            set
            {
                if (_answer != value)
                {
                    _answer = value;
                    OnPropertyChanged(nameof(Answer)); // Notify UI
                }
            }
        }
        public override Exercise CreateExercise(string question, Difficulty difficulty)
        {
            return new FlashcardExercise(0,question,Answer,difficulty);
        }
    }
}
