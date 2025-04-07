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
        private FillInTheBlankExercise? exercise;
        private readonly IExerciseService exerciseService;
        private ObservableCollection<string>? userAnswers;
        public ObservableCollection<string>? UserAnswers
        {
            get { return userAnswers; }
            set { SetProperty(ref userAnswers, value); }
        }

        public FillInTheBlankExerciseViewModel(IExerciseService service)
        {
            exerciseService = service;
        }

        public async Task GetExercise(int id)
        {
            Exercise exercise = await exerciseService.GetExerciseById(id);
            if (exercise is FillInTheBlankExercise fillInTheBlankExercise)
            {
                this.exercise = fillInTheBlankExercise;
            }
            else
            {
                throw new Exception("Invalid exercise type given to viewModel");
            }

            userAnswers =
                [.. Enumerable.Repeat(string.Empty, this.exercise.PossibleCorrectAnswers.Count)];
        }

        public bool VerifyIfAnswerIsCorrect()
        {
            if (exercise == null || userAnswers == null)
            {
                throw new InvalidOperationException("Exercise or UserAnswers is not initialized.");
            }

            return exercise.ValidateAnswer(userAnswers.ToList());
        }
    }
}
