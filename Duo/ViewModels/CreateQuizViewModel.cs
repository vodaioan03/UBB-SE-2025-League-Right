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
    internal class CreateQuizViewModel: ViewModelBase
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

        private void CreateSampleExercises()
        {

            try
            {
                // Fill in the blank exercises
                Exercises.Add(new FillInTheBlankExercise(1, "The sky is ___.", Difficulty.Normal, new List<string> { "blue" }));
                Exercises.Add(new FillInTheBlankExercise(2, "Water boils at ___ degrees Celsius.", Difficulty.Normal, new List<string> { "100" }));

                // Association exercise - using fully qualified namespace
                var firstList = new List<string> { "Dog", "Cat" };
                var secondList = new List<string> { "Bark", "Meow" };
                Exercises.Add(new Duo.Models.Exercises.AssociationExercise(3, "Match animals with their sounds", Difficulty.Hard, firstList, secondList));
            }
            catch (Exception ex)
            {
                // Add error handling for debugging
                System.Diagnostics.Debug.WriteLine($"Error creating exercises: {ex.Message}");
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
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
            Quiz newQuiz = new Quiz(0, 1, 3);
            foreach (var exercise in SelectedExercises)
            {
                newQuiz.AddExercise(exercise);
            }
            try {
                await _quizService.CreateQuiz(newQuiz);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            Debug.WriteLine(newQuiz);
        }
    }
}
