using Duo.Commands;
using Duo.Models.Exercises;
using Duo.Models.Quizzes;
using Duo.Models;
using Duo.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Duo.ViewModels
{
    internal class CreateExamViewModel : AdminBaseViewModel
    {
        private readonly QuizService _quizService;
        private readonly ExerciseService _exerciseService;
        private readonly List<Exercise> _availableExercises;
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
                _quizService = (QuizService)App.serviceProvider.GetService(typeof(QuizService));
                _exerciseService = (ExerciseService)App.serviceProvider.GetService(typeof(ExerciseService));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            LoadExercisesAsync();
            SaveButtonCommand = new RelayCommand(CreateExam);
            OpenSelectExercisesCommand = new RelayCommand(openSelectExercises);
            RemoveExerciseCommand = new RelayCommandWithParameter<Exercise>(RemoveExercise);
        }

        private async void LoadExercisesAsync()
        {
            Exercises.Clear(); // Clear the ObservableCollection
            var exercises = await _exerciseService.GetAllExercises();
            foreach (var exercise in exercises)
            {
                Exercises.Add(exercise);
            }
        }

        public void openSelectExercises()
        {
            ShowListViewModal?.Invoke(GetAvailableExercises());
        }

        public List<Exercise> GetAvailableExercises()
        {
            List<Exercise> availableExercises = new List<Exercise>();
            foreach (var exercise in Exercises)
            {
                if (!SelectedExercises.Contains(exercise))
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

        public async void CreateExam()
        {
            Debug.WriteLine("Creating exam...");
            Exam newExam = new Exam(0, null);
            foreach (var exercise in SelectedExercises)
            {
                newExam.AddExercise(exercise);
            }
            try
            {
                int examId = await _quizService.CreateExam(newExam);
                //await _quizService.AddExercisesToExam(examId, newExam.ExerciseList);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            Debug.WriteLine(newExam);
            GoBack();
        }
   
    }
}
