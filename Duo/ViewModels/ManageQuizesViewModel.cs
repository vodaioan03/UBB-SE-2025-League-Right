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

namespace Duo.ViewModels
{
    internal class ManageQuizesViewModel : AdminBaseViewModel
    {
        private readonly ExerciseService exerciseService;
        private readonly QuizService quizService;
        public ObservableCollection<Quiz> Quizes { get; set; } = new ObservableCollection<Quiz>();
        public ObservableCollection<Exercise> QuizExercises { get; private set; } = new ObservableCollection<Exercise>();
        public ObservableCollection<Exercise> AvailableExercises { get; private set; } = new ObservableCollection<Exercise>();

        public event Action<List<Exercise>> ShowListViewModal;

        private Quiz selectedQuiz;

        public ManageQuizesViewModel()
        {
            try
            {
                exerciseService = (ExerciseService)App.ServiceProvider.GetService(typeof(ExerciseService));
                quizService = (QuizService)App.ServiceProvider.GetService(typeof(QuizService));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            DeleteQuizCommand = new RelayCommandWithParameter<Quiz>(DeleteQuiz);
            OpenSelectExercisesCommand = new RelayCommand(OpenSelectExercises);
            RemoveExerciseFromQuizCommand = new RelayCommandWithParameter<Exercise>(RemoveExerciseFromQuiz);
            LoadExercisesAsync();
            InitializeViewModel();
        }

        public Quiz SelectedQuiz
        {
            get => selectedQuiz;
            set
            {
                selectedQuiz = value;
                UpdateQuizExercises(SelectedQuiz);
                OnPropertyChanged();
            }
        }

        public ICommand DeleteQuizCommand { get; }
        public ICommand OpenSelectExercisesCommand { get; }
        public ICommand RemoveExerciseFromQuizCommand { get; }

        public async void DeleteQuiz(Quiz quizToBeDeleted)
        {
            Debug.WriteLine("Deleting quiz...");

            if (quizToBeDeleted == SelectedQuiz)
            {
                SelectedQuiz = null;
                UpdateQuizExercises(SelectedQuiz);
            }

            foreach (var exercise in quizToBeDeleted.ExerciseList)
            {
                AvailableExercises.Add(exercise);
            }

            try
            {
                await quizService.DeleteQuiz(quizToBeDeleted.Id);
                Quizes.Remove(quizToBeDeleted);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                RaiseErrorMessage(ex.Message, string.Empty);
            }
        }

        public async void InitializeViewModel()
        {
            try
            {
                List<Quiz> quizes = await quizService.Get();
                foreach (var quiz in quizes)
                {
                    Quizes.Add(quiz);
                    Debug.WriteLine(quiz);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                RaiseErrorMessage(ex.Message, string.Empty);
            }
        }

        public async void UpdateQuizExercises(Quiz selectedQuiz)
        {
            Debug.WriteLine("Updating quiz exercises...");
            QuizExercises.Clear();
            if (SelectedQuiz == null)
            {
                return;
            }

            List<Exercise> exercisesOfSelectedQuiz = await exerciseService.GetAllExercisesFromQuiz(selectedQuiz.Id);
            foreach (var exercise in exercisesOfSelectedQuiz)
            {
                Debug.WriteLine(exercise);
                QuizExercises.Add(exercise);
            }
        }

        public void OpenSelectExercises()
        {
            Debug.WriteLine("Opening select exercises...");
            ShowListViewModal?.Invoke(AvailableExercises.ToList());
        }
        private async void LoadExercisesAsync()
        {
            AvailableExercises.Clear(); // Clear the ObservableCollection
            var exercises = await exerciseService.GetAllExercises();
            foreach (var exercise in exercises)
            {
                Debug.WriteLine(exercise); // Add each exercise to the ObservableCollection
                AvailableExercises.Add(exercise);
            }
        }
        public async void AddExercise(Exercise selectedExercise)
        {
            Debug.WriteLine("Adding exercise...");
            if (SelectedQuiz == null)
            {
                Debug.WriteLine("No quiz selected.");
                return;
            }
            SelectedQuiz.AddExercise(selectedExercise);
            try
            {
                await quizService.AddExerciseToQuiz(SelectedQuiz.Id, selectedExercise.Id);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                RaiseErrorMessage(ex.Message, string.Empty);
            }
            UpdateQuizExercises(SelectedQuiz);
        }

        public async void RemoveExerciseFromQuiz(Exercise selectedExercise)
        {
            Debug.WriteLine("Removing exercise...");
            try
            {
                await quizService.RemoveExerciseFromQuiz(SelectedQuiz.Id, selectedExercise.Id);
                SelectedQuiz.RemoveExercise(selectedExercise);
                UpdateQuizExercises(SelectedQuiz);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                RaiseErrorMessage(ex.Message, string.Empty);
            }
        }
    }
}
