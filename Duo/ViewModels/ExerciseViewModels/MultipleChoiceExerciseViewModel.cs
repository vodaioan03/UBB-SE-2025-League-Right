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
        private MultipleChoiceExercise? _exercise;
        private ObservableCollection<string>? _userChoices;
        private readonly ExerciseService _exerciseService;
        public ObservableCollection<string>? UserChoices
        {
            get
            {
                return _userChoices;
            }
            set
            {
                SetProperty(ref _userChoices, value);
            }
        }

        public MultipleChoiceExerciseViewModel (ExerciseService service)
        {
            _exerciseService = service;
        }

        public async Task GetExercise(int id)
        {
            Exercise exercise = await _exerciseService.GetExerciseById(id);
            if (exercise is MultipleChoiceExercise multipleChoiceExercise)
                _exercise = multipleChoiceExercise;
            else
                throw new Exception("Invalid exercise type given to viewModel");

            _userChoices = [];
        }

        public bool VerifyIfAnswerIsCorrect()
        {
            if (_exercise == null || _userChoices == null)
                throw new InvalidOperationException("Exercise or UserAnswers is not initialized.");

            return _exercise.ValidateAnswer([.. _userChoices]);
        }
    }
}
