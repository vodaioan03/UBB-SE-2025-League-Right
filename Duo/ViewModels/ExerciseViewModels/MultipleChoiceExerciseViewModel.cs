using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Duo.Models.Exercises;
using Duo.Services;
using Duo.ViewModels.Base;

namespace Duo.ViewModels.ExerciseViewModels
{
    public class MultipleChoiceExerciseViewModel : ViewModelBase
    {
        private MultipleChoiceExercise? exercise;
        private ObservableCollection<string>? userChoices;
        private readonly ExerciseService exerciseService;
        public ObservableCollection<string>? UserChoices
        {
            get
            {
                return userChoices;
            }
            set
            {
                SetProperty(ref userChoices, value);
            }
        }

        public MultipleChoiceExerciseViewModel(ExerciseService service)
        {
            exerciseService = service;
        }

        public async Task GetExercise(int id)
        {
            Exercise exercise = await exerciseService.GetExerciseById(id);
            if (exercise is MultipleChoiceExercise multipleChoiceExercise)
            {
                this.exercise = multipleChoiceExercise;
            }
            else
            {
                throw new Exception("Invalid exercise type given to viewModel");
            }

            userChoices =
                [];
        }

        public bool VerifyIfAnswerIsCorrect()
        {
            if (exercise == null || userChoices == null)
            {
                throw new InvalidOperationException("Exercise or UserAnswers is not initialized.");
            }

            return exercise.ValidateAnswer([.. userChoices]);
        }
    }
}
