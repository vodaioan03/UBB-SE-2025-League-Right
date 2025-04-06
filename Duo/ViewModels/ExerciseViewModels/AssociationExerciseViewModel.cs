using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Duo.Models.Exercises;
using Duo.Services;
using Duo.ViewModels.Base;

namespace Duo.ViewModels.ExerciseViewModels
{
    public class AssociationExerciseViewModel : ViewModelBase
    {
        private readonly ExerciseService _exerciseService;
        private AssociationExercise? _exercise;
        private ObservableCollection<(string, string)>? _userAnswers;

        public ObservableCollection<(string, string)>? UserAnswers
        {
            get { return _userAnswers; }
            set { SetProperty(ref _userAnswers, value); }
        }

        public AssociationExerciseViewModel(ExerciseService exerciseService)
        {
            _exerciseService = exerciseService;
        }

        public async Task GetExercise(int id)
        {
            Exercise exercise = await _exerciseService.GetExerciseById(id);
            if (exercise is AssociationExercise associationExercise)
                _exercise = associationExercise;
            else
                throw new Exception("Invalid exercise type given to viewModel");

            _userAnswers = [];
        }

        public bool VerifyIfAnswerIsCorrect()
        {
            if (_exercise == null || _userAnswers == null)
                return false;
            return _exercise.ValidateAnswer([.. _userAnswers]);
        }
    }
}
