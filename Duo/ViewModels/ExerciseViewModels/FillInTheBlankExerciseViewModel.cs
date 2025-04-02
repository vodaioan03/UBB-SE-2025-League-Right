using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Duo.Models.Exercises;
using Duo.Services;
using Duo.ViewModels.Base;

namespace Duo.ViewModels.ExerciseViewModels
{
    public class FillInTheBlankExerciseViewModel : ViewModelBase
    {
        private FillInTheBlankExercise? _exercise;
        private readonly ExerciseService _exerciseService;
        private ObservableCollection<string>? _userAnswers;
        public ObservableCollection<string>? UserAnswers
        {
            get { return _userAnswers; }
            set { SetProperty(ref _userAnswers, value); }
        }

        public FillInTheBlankExerciseViewModel(ExerciseService service)
        {
            _exerciseService = service;
        }

        public async Task GetExercise(int id)
        {
            Exercise exercise = await _exerciseService.GetExerciseById(id);
            if (exercise is FillInTheBlankExercise fillInTheBlankExercise)
                _exercise = fillInTheBlankExercise;
            else
                throw new Exception("Invalid exercise type given to viewModel");

            _userAnswers = [.. Enumerable.Repeat("", _exercise.PossibleCorrectAnswers.Count)];
        }

        public bool VerifyIfAnswerIsCorrect()
        {
            if (_exercise == null || _userAnswers == null)
                throw new InvalidOperationException("Exercise or UserAnswers is not initialized.");

            return _exercise.ValidateAnswer(_userAnswers.ToList());
        }

    }
}
