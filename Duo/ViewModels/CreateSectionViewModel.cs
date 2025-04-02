using Duo.Commands;
using Duo.Models.Exercises;
using Duo.Models.Quizzes;
using Duo.Models.Sections;
using Duo.Services;
using Duo.ViewModels.Base;
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
    internal class CreateSectionViewModel : AdminBaseViewModel
    {
        private readonly SectionService _sectionService;
        private readonly QuizService _quizService;
        private readonly ExerciseService _exerciseService;
        private string _subjectText;
        public ObservableCollection<Quiz> Quizes { get; set; } = new ObservableCollection<Quiz>();
        public ObservableCollection<Quiz> SelectedQuizes { get; private set; } = new ObservableCollection<Quiz>();
        public ObservableCollection<Exam> Exams { get; set; } = new ObservableCollection<Exam>();
        public ObservableCollection<Exam> SelectedExams { get; private set; } = new ObservableCollection<Exam>();

        public event Action<List<Quiz>> ShowListViewModalQuizes;
        public event Action<List<Exam>> ShowListViewModalExams;
        public ICommand RemoveQuizCommand { get; }
        public ICommand SaveButtonCommand { get; }
        public ICommand OpenSelectQuizesCommand { get; }
        public ICommand OpenSelectExamsCommand { get; }


        public CreateSectionViewModel()
        {
            try
            {
                _sectionService = (SectionService)App.serviceProvider.GetService(typeof(SectionService));
                _quizService = (QuizService)App.serviceProvider.GetService(typeof(QuizService));
                _exerciseService = (ExerciseService)App.serviceProvider.GetService(typeof(ExerciseService));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            OpenSelectQuizesCommand = new RelayCommand(OpenSelectQuizes);
            OpenSelectExamsCommand = new RelayCommand(OpenSelectExams);
            SaveButtonCommand = new RelayCommand(CreateSection);
            RemoveQuizCommand = new RelayCommandWithParameter<Quiz>(RemoveSelectedQuiz);
            GetQuizesAsync();
            GetExamsAsync();
        }
        public string SubjectText
        {
            get => _subjectText;
            set
            {
                if (_subjectText != value)
                {
                    _subjectText = value;
                    OnPropertyChanged(nameof(SubjectText)); // Notify UI
                }
            }
        }
        public void RemoveSelectedQuiz(Quiz quizToBeRemoved)
        {
            Debug.WriteLine("Removing quiz...");
            SelectedQuizes.Remove(quizToBeRemoved);
        }
        public void OpenSelectQuizes()
        {
            Debug.WriteLine("Opening select quizes...");
            ShowListViewModalQuizes?.Invoke(GetAvailableQuizes());
        }
        public void OpenSelectExams()
        {
            Debug.WriteLine("Opening select exams...");
            ShowListViewModalExams?.Invoke(GetAvailableExams());
        }

        public async void GetQuizesAsync()
        {
            Quizes.Clear(); // Clear the ObservableCollection
            List<Quiz> quizes = await _quizService.GetAllQuizzesFromSection(1);
            foreach (var quiz in quizes)
            {
                Debug.WriteLine(quiz); // Add each quiz to the ObservableCollection
                Quizes.Add(quiz);
            }
        }
        public async void GetExamsAsync()
        {
            Quizes.Clear(); // Clear the ObservableCollection
            Exam exam = await _quizService.GetExamFromSection(1);
            Exams.Add(exam);
        }

        public List<Exam> GetAvailableExams()
        {
            List<Exam> availableExams = new List<Exam>();
            foreach (var exam in Exams)
            {
                if (!SelectedExams.Contains(exam))
                    availableExams.Add(exam);
            }
            return availableExams;
        }
        public List<Quiz> GetAvailableQuizes()
        {
            List<Quiz> availableQuizes = new List<Quiz>();
            foreach (var quiz in Quizes)
            {
                if (!SelectedQuizes.Contains(quiz))
                    availableQuizes.Add(quiz);
            }
            return availableQuizes;
        }

        public void AddQuiz(Quiz newQuiz)
        {
            Debug.WriteLine("add quiz..."+ newQuiz.Id);
            SelectedQuizes.Add(newQuiz);
        }
        public void AddExam(Exam newExam)
        {
            Debug.WriteLine("add exam..." + newExam.Id);
            SelectedExams.Add(newExam);
        }
        public async void CreateSection()
        {
            try
            {
                Section newSection = new Section(0, 1, SubjectText, "placeholder description", 1, null);
                newSection.Quizzes = SelectedQuizes.ToList();
                foreach (var quiz in newSection.Quizzes)
                {
                    quiz.ExerciseList = await _exerciseService.GetAllExercisesFromQuiz(quiz.Id);
                }
                foreach (var quiz in newSection.Quizzes)
                {
                    Debug.WriteLine(quiz);
                }
                if (SelectedExams.Count != 1)
                {
                    RaiseErrorMessage("You must have exactly one exam selected!", "");
                    return;
                }
                    
                newSection.Exam = SelectedExams.ToList()[0];
                newSection.Exam.ExerciseList = await _exerciseService.GetAllExercisesFromExam(newSection.Exam.Id);
                int sectionId = await _sectionService.AddSection(newSection);
                foreach(var quiz in SelectedQuizes.ToList())
                {
                    quiz.SectionId = sectionId;
                    await _quizService.UpdateQuiz(quiz);
                }

                Debug.WriteLine("Section created: " + newSection);
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
                RaiseErrorMessage(ex.Message, "");
            }
            GoBack();
        }
    }
}
