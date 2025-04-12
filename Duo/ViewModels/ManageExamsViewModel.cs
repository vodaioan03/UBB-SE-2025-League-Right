using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;
using System.Windows.Input;
using Duo.Commands;
using Duo.Models.Exercises;
using Duo.Models.Quizzes;
using Duo.Services;
using static Azure.Core.HttpHeader;

namespace Duo.ViewModels
{
    internal class ManageExamsViewModel : AdminBaseViewModel
    {
        private readonly IExerciseService exerciseService;
        private readonly IQuizService quizService;
        public ObservableCollection<Exam> Exams { get; set; } = new ObservableCollection<Exam>();
        public ObservableCollection<Exercise> ExamExercises { get; private set; } = new ObservableCollection<Exercise>();
        public ObservableCollection<Exercise> AvailableExercises { get; private set; } = new ObservableCollection<Exercise>();

        public event Action<List<Exercise>> ShowListViewModal;

        private Exam selectedExam;

        public ManageExamsViewModel()
        {
            try
            {
                exerciseService = (IExerciseService)App.ServiceProvider.GetService(typeof(IExerciseService));
                quizService = (IQuizService)App.ServiceProvider.GetService(typeof(IQuizService));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            DeleteExamCommand = new RelayCommandWithParameter<Exam>(exam => _ = DeleteExam(exam));
            OpenSelectExercisesCommand = new RelayCommand(OpenSelectExercises);
            RemoveExerciseFromQuizCommand = new RelayCommandWithParameter<Exercise>(exercise => _ = RemoveExerciseFromExam(exercise));
            LoadExercisesAsync();
            InitializeViewModel();
        }

        public Exam SelectedExam
        {
            get => selectedExam;
            set
            {
                selectedExam = value;
                UpdateExamExercises(SelectedExam);
                OnPropertyChanged();
            }
        }

        public ICommand DeleteExamCommand { get; }
        public ICommand OpenSelectExercisesCommand { get; }
        public ICommand RemoveExerciseFromQuizCommand { get; }

        public async Task DeleteExam(Exam examToBeDeleted)
        {
            Debug.WriteLine("Deleting quiz...");

            if (examToBeDeleted == SelectedExam)
            {
                SelectedExam = null;
                await UpdateExamExercises(SelectedExam);
            }

            foreach (var exercise in examToBeDeleted.ExerciseList)
            {
                AvailableExercises.Add(exercise);
            }

            try
            {
                await quizService.DeleteExam(examToBeDeleted.Id);
                Exams.Remove(examToBeDeleted);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                RaiseErrorMessage(ex.Message, string.Empty);
            }
        }

        public async Task InitializeViewModel()
        {
            try
            {
                List<Exam> exams = await quizService.GetAllAvailableExams();
                foreach (var exam in exams)
                {
                    Exams.Add(exam);
                    Debug.WriteLine(exam);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                RaiseErrorMessage(ex.Message, string.Empty);
            }
        }

        public async Task UpdateExamExercises(Exam selectedExam)
        {
            try
            {
                Debug.WriteLine("Updating exam exercises...");
                ExamExercises.Clear();

                if (selectedExam == null)
                {
                    Debug.WriteLine("No exam selected. Skipping update.");
                    return;
                }

                List<Exercise> exercisesOfSelectedQuiz = await exerciseService.GetAllExercisesFromExam(selectedExam.Id);

                foreach (var exercise in exercisesOfSelectedQuiz)
                {
                    Debug.WriteLine(exercise);
                    ExamExercises.Add(exercise);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error during UpdateExamExercises: {ex.Message}");
                Debug.WriteLine(ex.StackTrace);
            }
        }

        public void OpenSelectExercises()
        {
            Debug.WriteLine("Opening select exercises...");
            ShowListViewModal?.Invoke(AvailableExercises.ToList());
        }
        private async Task LoadExercisesAsync()
        {
            try
            {
                AvailableExercises.Clear(); // Clear the ObservableCollection

                var exercises = await exerciseService.GetAllExercises();

                foreach (var exercise in exercises)
                {
                    Debug.WriteLine(exercise); // Add each exercise to the ObservableCollection
                    AvailableExercises.Add(exercise);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error during LoadExercisesAsync: {ex.Message}");
                Debug.WriteLine(ex.StackTrace);
            }
        }

        public async Task AddExercise(Exercise selectedExercise)
        {
            try
            {
                Debug.WriteLine("Adding exercise...");

                if (SelectedExam == null)
                {
                    Debug.WriteLine("No quiz selected.");
                    return;
                }

                SelectedExam.AddExercise(selectedExercise);

                await quizService.AddExerciseToQuiz(SelectedExam.Id, selectedExercise.Id);
                await UpdateExamExercises(SelectedExam);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error while adding exercise: {ex.Message}");
                Debug.WriteLine(ex.StackTrace);
                RaiseErrorMessage(ex.Message, string.Empty);
            }
        }

        public async Task RemoveExerciseFromExam(Exercise selectedExercise)
        {
            try
            {
                Debug.WriteLine("Removing exercise...");

                await quizService.RemoveExerciseFromQuiz(SelectedExam.Id, selectedExercise.Id);
                SelectedExam.RemoveExercise(selectedExercise);
                await UpdateExamExercises(SelectedExam);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error while removing exercise: {ex.Message}");
                Debug.WriteLine(ex.StackTrace);
                RaiseErrorMessage(ex.Message, string.Empty);
            }
        }
    }
}
