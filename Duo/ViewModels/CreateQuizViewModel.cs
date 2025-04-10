using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Duo.Commands;
using Duo.Models;
using Duo.Models.Exercises;
using Duo.Models.Quizzes;
using Duo.Services;
using Duo.ViewModels.Base;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Duo.ViewModels
{
    internal class CreateQuizViewModel : AdminBaseViewModel
    {
        private readonly IQuizService quizService;
        private readonly IExerciseService exerciseService;
        private readonly List<Exercise> availableExercises;
        public ObservableCollection<Exercise> Exercises { get; set; } = new ObservableCollection<Exercise>();
        public ObservableCollection<Exercise> SelectedExercises { get; private set; } = new ObservableCollection<Exercise>();

        public event Action<List<Exercise>>? ShowListViewModal;

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
                if (App.ServiceProvider != null)
                {
                    quizService = (IQuizService?)App.ServiceProvider.GetService(typeof(IQuizService));
                    exerciseService = (IExerciseService?)App.ServiceProvider.GetService(typeof(IExerciseService));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            LoadExercisesAsync();
            SaveButtonCommand = new RelayCommand(() => _ = CreateQuiz());

            OpenSelectExercisesCommand = new RelayCommand(OpenSelectExercises);
            RemoveExerciseCommand = new RelayCommandWithParameter<Exercise>(RemoveExercise);
        }

        private async Task LoadExercisesAsync()
        {
            try
            {
                Exercises.Clear();
                var exercises = await exerciseService.GetAllExercises();
                foreach (var exercise in exercises)
                {
                    // Add each exercise to the ObservableCollection
                    Exercises.Add(exercise);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading exercises: {ex.Message}");
            }
        }

        public void OpenSelectExercises()
        {
            Debug.WriteLine("Opening select exercises...");
            ShowListViewModal?.Invoke(GetAvailableExercises());
        }

        public List<Exercise> GetAvailableExercises()
        {
            List<Exercise> availableExercises = new List<Exercise>();
            foreach (var exercise in Exercises)
            {
                if (!SelectedExercises.Contains(exercise))
                {
                    availableExercises.Add(exercise);
                }
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

        public async Task CreateQuiz()
        {
            try
            {
                Debug.WriteLine("Creating quiz...");
                Quiz newQuiz = new Quiz(0, 1, null);

                foreach (var exercise in SelectedExercises)
                {
                    newQuiz.AddExercise(exercise);
                }

                int quizId = await quizService.CreateQuiz(newQuiz);
                await quizService.AddExercisesToQuiz(quizId, newQuiz.ExerciseList);

                GoBack();
                Debug.WriteLine(newQuiz);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error during CreateQuiz: {ex.Message}");
                Debug.WriteLine(ex.StackTrace);
                RaiseErrorMessage(ex.Message, string.Empty);
            }
        }
    }
}
