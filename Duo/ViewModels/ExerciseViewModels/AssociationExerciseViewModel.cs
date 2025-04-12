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
        private readonly ExerciseService exerciseService;
        private AssociationExercise? exercise;
        private ObservableCollection<(string, string)>? userAnswers;

        public ObservableCollection<(string, string)>? UserAnswers
        {
            get { return userAnswers; }
            set { SetProperty(ref userAnswers, value); }
        }

        public AssociationExerciseViewModel(ExerciseService exerciseService)
        {
            this.exerciseService = exerciseService;
        }

        public async Task GetExercise(int id)
        {
            Exercise exercise = await exerciseService.GetExerciseById(id);
            if (exercise is AssociationExercise associationExercise)
            {
                this.exercise = associationExercise;
            }
            else
            {
                throw new Exception("Invalid exercise type given to viewModel");
            }

            userAnswers = new ObservableCollection<(string, string)>();
        }

        public bool VerifyIfAnswerIsCorrect()
        {
            if (exercise == null || userAnswers == null)
            {
                return false;
            }
            return exercise.ValidateAnswer([.. userAnswers]);
        }
    }
}
