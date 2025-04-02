using Duo.Commands;
using Duo.Models.Exercises;
using Duo.Models.Quizzes;
using Duo.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using static Azure.Core.HttpHeader;

namespace Duo.ViewModels
{
    internal class ManageExamsViewModel:AdminBaseViewModel
    {

        private readonly ExerciseService _exerciseService;
        private readonly QuizService _quizService;
        public ObservableCollection<Exam> Exams { get; set; } = new ObservableCollection<Exam>();
        public ObservableCollection<Exercise> ExamExercises { get; private set; } = new ObservableCollection<Exercise>();
        public ObservableCollection<Exercise> AvailableExercises { get; private set; } = new ObservableCollection<Exercise>();


        public event Action<List<Exercise>> ShowListViewModal;

        private Exam _selectedExam;

        public ManageExamsViewModel()
        {
            try
            {
                _exerciseService = (ExerciseService)App.serviceProvider.GetService(typeof(ExerciseService));
                _quizService = (QuizService)App.serviceProvider.GetService(typeof(QuizService));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            DeleteExamCommand = new RelayCommandWithParameter<Exam>(DeleteExam);
            OpenSelectExercisesCommand = new RelayCommand(openSelectExercises);
            RemoveExerciseFromQuizCommand = new RelayCommandWithParameter<Exercise>(RemoveExerciseFromExam);
            LoadExercisesAsync();
            initializeViewModel();
        }

        public Exam SelectedExam
        {
            get => _selectedExam;
            set
            {
                _selectedExam = value;
                UpdateExamExercises(SelectedExam);
                OnPropertyChanged();
            }
        }

        public ICommand DeleteExamCommand { get; }
        public ICommand OpenSelectExercisesCommand { get; }
        public ICommand RemoveExerciseFromQuizCommand { get; }

        public async void DeleteExam(Exam examToBeDeleted)
        {
            Debug.WriteLine("Deleting quiz...");

            if (examToBeDeleted == SelectedExam)
            {
                SelectedExam = null;
                UpdateExamExercises(SelectedExam);
            }

            foreach (var exercise in examToBeDeleted.ExerciseList)
            {
                AvailableExercises.Add(exercise);
            }

            try
            {
                await _quizService.DeleteExam(examToBeDeleted.Id);
                Exams.Remove(examToBeDeleted);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                RaiseErrorMessage(ex.Message, "");
            }
        }

        public async void initializeViewModel()
        {

            try
            {
                List<Exam> exams = await _quizService.GetAllAvailableExams();
                foreach (var exam in exams)
                {
                    Exams.Add(exam);
                    Debug.WriteLine(exam);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                RaiseErrorMessage(ex.Message, "");
            }
        }

        public async void UpdateExamExercises(Exam selectedExam)
        {
            Debug.WriteLine("Updating exam exercises...");
            ExamExercises.Clear();
            if (SelectedExam == null)
                return;

            List<Exercise> exercisesOfSelectedQuiz = await _exerciseService.GetAllExercisesFromExam(selectedExam.Id);
            foreach (var exercise in exercisesOfSelectedQuiz)
            {
                Debug.WriteLine(exercise);
                ExamExercises.Add(exercise);
            }
        }

        public void openSelectExercises()
        {
            Debug.WriteLine("Opening select exercises...");
            ShowListViewModal?.Invoke(AvailableExercises.ToList());
        }
        private async void LoadExercisesAsync()
        {
            AvailableExercises.Clear(); // Clear the ObservableCollection
            var exercises = await _exerciseService.GetAllExercises();
            foreach (var exercise in exercises)
            {
                Debug.WriteLine(exercise); // Add each exercise to the ObservableCollection
                AvailableExercises.Add(exercise);
            }
        }
        public async void AddExercise(Exercise selectedExercise)
        {
            Debug.WriteLine("Adding exercise...");
            if (SelectedExam == null)
            {
                Debug.WriteLine("No quiz selected.");
                return;
            }
            SelectedExam.AddExercise(selectedExercise);
            try
            {
                await _quizService.AddExerciseToQuiz(SelectedExam.Id, selectedExercise.Id);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                RaiseErrorMessage(ex.Message, "");
            }
            UpdateExamExercises(SelectedExam);
        }

        public async void RemoveExerciseFromExam(Exercise selectedExercise)
        {
            Debug.WriteLine("Removing exercise...");
            try
            {
                await _quizService.RemoveExerciseFromQuiz(SelectedExam.Id, selectedExercise.Id);
                SelectedExam.RemoveExercise(selectedExercise);
                UpdateExamExercises(SelectedExam);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                RaiseErrorMessage(ex.Message, "");
            }
        }

    }
}
