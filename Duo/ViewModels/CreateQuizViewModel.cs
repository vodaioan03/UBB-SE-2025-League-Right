using Duo.Commands;
using Duo.Models;
using Duo.Models.Exercises;
using Duo.Services;
using Duo.ViewModels.Base;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Duo.Models.Quizzes;

namespace Duo.ViewModels
{
    internal class CreateQuizViewModel: AdminBaseViewModel
    {
        private readonly QuizService _quizService;
        private readonly ExerciseService _exerciseService;
        private readonly List<Exercise> _availableExercises;
        public ObservableCollection<Exercise> Exercises { get; set; } = new ObservableCollection<Exercise>();
        public ObservableCollection<Exercise> SelectedExercises { get; private set; } = new ObservableCollection<Exercise>();

        public event Action<List<Exercise>> ShowListViewModal;


        private const int MAX_EXERCISES = 10;
        private const int NO_ORDER_NUMBER = -1;
        private const int NO_SECTION_ID = -1;

        public ICommand RemoveExerciseCommand { get; }
        public ICommand SaveButtonCommand { get; }
        public ICommand OpenSelectExercisesCommand { get; }

        public CreateQuizViewModel()
        {
            try
            {
                _quizService = (QuizService)App.serviceProvider.GetService(typeof(QuizService));
                _exerciseService = (ExerciseService)App.serviceProvider.GetService(typeof(ExerciseService));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            LoadExercisesAsync();
            SaveButtonCommand = new RelayCommand(CreateQuiz);
            OpenSelectExercisesCommand = new RelayCommand(openSelectExercises);
            RemoveExerciseCommand = new RelayCommandWithParameter<Exercise>(RemoveExercise);
        }

        private async void LoadExercisesAsync()
        {
            Exercises.Clear(); // Clear the ObservableCollection
            var exercises = await _exerciseService.GetAllExercises();
            foreach (var exercise in exercises)
            {
                Debug.WriteLine(exercise); // Add each exercise to the ObservableCollection
                Exercises.Add(exercise);
            }
        }

        public void openSelectExercises()
        {
            Debug.WriteLine("Opening select exercises...");
            ShowListViewModal?.Invoke(GetAvailableExercises());
        }

        public List<Exercise> GetAvailableExercises()
        {
            List<Exercise> availableExercises = new List<Exercise>();
            foreach (var exercise in Exercises)
            {
                if(!SelectedExercises.Contains(exercise))
                    availableExercises.Add(exercise);
            }
            return availableExercises;
        }

        public void AddExercise(Exercise selectedExercise)
        {

            if (SelectedExercises.Count < MAX_EXERCISES)
            {
                SelectedExercises.Add(selectedExercise);
            }
            else
            {
                Debug.WriteLine("Cannot add more exercises", $"Maximum number of exercises ({MAX_EXERCISES}) reached.");
            }
            
        }
        public void RemoveExercise(Exercise exerciseToBeRemoved)
        {
            SelectedExercises.Remove(exerciseToBeRemoved);
            Debug.WriteLine("Removing exercise...");
        }

        public async void CreateQuiz()
        {
            Debug.WriteLine("Creating quiz...");
            Quiz newQuiz = new Quiz(0, 1, null);
            foreach (var exercise in SelectedExercises)
            {
                newQuiz.AddExercise(exercise);
            }
            try {
                int quizId = await _quizService.CreateQuiz(newQuiz);
                await _quizService.AddExercisesToQuiz(quizId, newQuiz.ExerciseList);
                GoBack();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                RaiseErrorMessage(ex.Message, "");
                
            }
            Debug.WriteLine(newQuiz);
        }
    }
}
