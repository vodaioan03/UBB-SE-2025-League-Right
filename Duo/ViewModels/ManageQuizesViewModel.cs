using Duo.Commands;
using Duo.Models;
using Duo.Models.Exercises;
using Duo.Models.Quizzes;
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
    internal class ManageQuizesViewModel: ViewModelBase
    {

        private readonly QuizService _quizService;
        public ObservableCollection<Quiz> Quizes { get; set; } = new ObservableCollection<Quiz>();
        public ObservableCollection<Exercise> QuizExercises { get; private set; } = new ObservableCollection<Exercise>();
        public ObservableCollection<Exercise> AvailableExercises { get; private set; } = new ObservableCollection<Exercise>();


        public event Action<List<Exercise>> ShowListViewModal;

        private Quiz _selectedQuiz;

        public ManageQuizesViewModel()
        {
            try
            {
                _quizService = (QuizService)App.serviceProvider.GetService(typeof(QuizService));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            DeleteQuizCommand = new RelayCommandWithParameter<Quiz>(DeleteQuiz);
            OpenSelectExercisesCommand = new RelayCommand(openSelectExercises);
            RemoveExerciseFromQuizCommand = new RelayCommandWithParameter<Exercise>(RemoveExerciseFromQuiz);
            GetAvailableExercises();
            initializeViewModel();
        }

        public Quiz SelectedQuiz
        {
            get => _selectedQuiz;
            set
            {
                _selectedQuiz = value;
                UpdateQuizExercises(SelectedQuiz);
                OnPropertyChanged();
            }
        }

        public ICommand DeleteQuizCommand { get; }
        public ICommand OpenSelectExercisesCommand { get; }
        public ICommand RemoveExerciseFromQuizCommand { get; }

        public void DeleteQuiz(Quiz quizToBeDeleted)
        {
            Debug.WriteLine("Deleting quiz...");

            if(quizToBeDeleted==SelectedQuiz)
            {
                SelectedQuiz = null;
                UpdateQuizExercises(SelectedQuiz);
            }

            foreach (var exercise in quizToBeDeleted.ExerciseList)
            {
                AvailableExercises.Add(exercise);
            }
            Quizes.Remove(quizToBeDeleted);
        }

        public void initializeViewModel()
        {
            Quiz quiz1 = new Quiz(0, -1, -1);
            quiz1.AddExercise(new FillInTheBlankExercise(5, "The sky is ___.", Difficulty.Normal, new List<string> { "blue" }));
            Quizes.Add(quiz1);
            Quizes.Add(new Quiz(1, -1, -1));
            Quizes.Add(new Quiz(2, -1, -1));
            Quizes.Add(new Quiz(3, -1, -1));
            Quizes.Add(new Quiz(4, -1, -1));
        }

        public void UpdateQuizExercises(Quiz selectedQuiz)
        {
            Debug.WriteLine("Updating quiz exercises...");
            QuizExercises.Clear();
            if (SelectedQuiz==null)
                return;
            foreach (var exercise in selectedQuiz.ExerciseList)
            {
                QuizExercises.Add(exercise);
            }
        }

        public void openSelectExercises()
        {
            Debug.WriteLine("Opening select exercises...");
            ShowListViewModal?.Invoke(AvailableExercises.ToList());
        }

        public void GetAvailableExercises()
        {
            AvailableExercises.Add(new FillInTheBlankExercise(1, "The sky is ___.", Difficulty.Normal, new List<string> { "blue" }));
            AvailableExercises.Add(new FillInTheBlankExercise(2, "Water boils at ___ degrees Celsius.", Difficulty.Normal, new List<string> { "100" }));
            AvailableExercises.Add(new AssociationExercise(3, "Match animals with their sounds", Difficulty.Hard, new List<string> { "Dog", "Cat" }, new List<string> { "Bark", "Meow" }));
            AvailableExercises.Add(new AssociationExercise(4, "Match the capitals to their countries.", Difficulty.Easy, new List<string> { "Paris", "London", "Berlin" }, new List<string> { "France", "England", "Germany" }));
        }
        public void AddExercise(Exercise selectedExercise)
        {
            Debug.WriteLine("Adding exercise...");
            SelectedQuiz.AddExercise(selectedExercise);
            UpdateQuizExercises(SelectedQuiz);
            AvailableExercises.Remove(selectedExercise);
        }

        public void RemoveExerciseFromQuiz(Exercise selectedExercise)
        {
            Debug.WriteLine("Removing exercise...");
            SelectedQuiz.RemoveExercise(selectedExercise);
            UpdateQuizExercises(SelectedQuiz);
            AvailableExercises.Add(selectedExercise);
        }

    }
}
