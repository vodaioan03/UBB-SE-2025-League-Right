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

namespace Duo.ViewModels
{
    internal class CreateExamViewModel : AdminBaseViewModel
    {
        private readonly IQuizService quizService;
        private readonly IExerciseService exerciseService;
        private readonly List<Exercise> availableExercises;
        public ObservableCollection<Exercise> Exercises { get; set; } = new ObservableCollection<Exercise>();
        public ObservableCollection<Exercise> SelectedExercises { get; private set; } = new ObservableCollection<Exercise>();

        public event Action<List<Exercise>> ShowListViewModal;

        private const int NO_ORDER_NUMBER = -1;
        private const int NO_SECTION_ID = -1;
        private const int MAX_EXERCISES = 25;

        public ICommand RemoveExerciseCommand { get; }
        public ICommand SaveButtonCommand { get; }
        public ICommand OpenSelectExercisesCommand { get; }

        public CreateExamViewModel()
        {
            try
            {
                quizService = (IQuizService)App.ServiceProvider.GetService(typeof(IQuizService));
                exerciseService = (IExerciseService)App.ServiceProvider.GetService(typeof(IExerciseService));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            LoadExercisesAsync();
            SaveButtonCommand = new RelayCommand(() => _ = CreateExam());
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

        public async Task CreateExam()
        {
            try
            {
                Debug.WriteLine("Creating exam...");
                Exam newExam = new Exam(0, null);

                foreach (var exercise in SelectedExercises)
                {
                    newExam.AddExercise(exercise);
                }

                int examId = await quizService.CreateExam(newExam);
                // await quizService.AddExercisesToExam(examId, newExam.ExerciseList);
                Debug.WriteLine(newExam);
                GoBack();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error during CreateExam: {ex.Message}");
                Debug.WriteLine(ex.StackTrace);
            }
        }
    }
}
