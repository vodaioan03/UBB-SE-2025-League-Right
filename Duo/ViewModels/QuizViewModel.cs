using Duo.Models.Exercises;
using Duo.Models.Quizzes;
using Duo.Services;
using Duo.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Duo.ViewModels
{
    public class QuizViewModel : ViewModelBase
    {
        private readonly ExerciseService _exerciseService;
        private readonly QuizService _quizService;
        private int _quizId;
        private Quiz _currentQuiz;
        private List<Exercise> _exercises;
        private Exercise _currentExercise;
        private int _currentExerciseIndex;
        private bool _validatedCurrent;

        public int QuizId
        {
            get => _quizId;
            set
            {
                _quizId = value;
                LoadQuiz();
                OnPropertyChanged(nameof(QuizId));
                LoadExercises();
            }
        }

        public List<Exercise> Exercises
        {
            get => _exercises;
            private set
            {
                _exercises = value;
                OnPropertyChanged(nameof(Exercises));
            }
        }

        public Exercise CurrentExercise
        {
            get => _currentExercise;
            set
            {
                _currentExercise = value;
                OnPropertyChanged(nameof(CurrentExercise));
            }
        }

        public Quiz CurrentQuiz
        {
            get => _currentQuiz;
            set
            {
                _currentQuiz = value;
                OnPropertyChanged(nameof(CurrentQuiz));
            }
        }

        public int CurrentExerciseIndex
        {
            get => _currentExerciseIndex;
            set
            {
                _currentExerciseIndex = value;
                OnPropertyChanged(nameof(CurrentExerciseIndex));
            }
        }

        public bool ValidatedCurrent
        {
            get => _validatedCurrent;
            set
            {
                _validatedCurrent = value;
                OnPropertyChanged(nameof(ValidatedCurrent));
            }
        }

        public QuizViewModel(ExerciseService exerciseService)
        {
            _exerciseService = exerciseService;
        }

        public QuizViewModel()
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
        }

        public async Task LoadQuiz()
        {
            try
            {
                //CurrentQuiz = await _quizService.GetExerciseById(_quizId);
                CurrentQuiz = new Quiz(1, 1, 1);

                //WHEN DB IS WORKING UNCOMMENT QUERY AND DELETE CONSTRUCTOR
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading Quiz: {ex.Message}");
            }
        }

        public async Task LoadExercises()
        {
            try
            {
                //Exercises = await _exerciseService.GetAllExercisesFromQuiz(QuizId);
                Exercises = new List<Exercise>()
                {
                    new AssociationExercise(1, "Match these things", Models.Difficulty.Normal ,new List<string>{"ameroicaaaa", "asmakdjskjdh", "hjdiahsd"}, new List<string>{"dnchuksgf", "shndus", "snhdajhvjdh"}),
                    //new MultipleChoiceExercise(2, "What is uhh", Models.Difficulty.Easy, new List<MultipleChoiceAnswerModel>{new MultipleChoiceAnswerModel { Answer="hi", IsCorrect=false }, new MultipleChoiceAnswerModel { Answer="hi", IsCorrect=true } }),
                    //new FillInTheBlankExercise(3, "Hi {} insert there {}", Models.Difficulty.Easy, new List<string>{"hi", "hi"}),
                    //new AssociationExercise(4, "Match these things", Models.Difficulty.Normal ,new List<string>{"ameroicaaaa", "asmakdjskjdh", "hjdiahsd"}, new List<string>{"dnchuksgf", "shndus", "snhdajhvjdh"}),
                    //new AssociationExercise(5, "Match these things", Models.Difficulty.Normal ,new List<string>{"ameroicaaaa", "asmakdjskjdh", "hjdiahsd"}, new List<string>{"dnchuksgf", "shndus", "snhdajhvjdh"}),
                }; 
                
                //WHEN DB IS WORKING UNCOMMENT QUERY AND DELETE CONSTRUCTOR

                CurrentExerciseIndex = 0;
                CurrentExercise = Exercises[CurrentExerciseIndex];
                ValidatedCurrent = false;

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading exercises: {ex.Message}");
                Exercises = new List<Exercise>();
            }
        }

        private void UpdateQuiz(bool isExerciseValid)
        {
            if (isExerciseValid)
            {
                CurrentQuiz.IncrementCorrectAnswers();
                CurrentQuiz.IncrementNumberOfAnswersGiven();
            }
            else
                CurrentQuiz.IncrementNumberOfAnswersGiven();
        }

        public bool ValidateCurrentExercise(object responses)
        {
            if (ValidatedCurrent)
                return true;

            var currentExercise = Exercises[CurrentExerciseIndex];
            bool isValid = false;

            if (currentExercise is AssociationExercise associationExercise)
            {
                isValid = associationExercise.ValidateAnswer((List<(string, string)>)responses);
            }
            else if (currentExercise is FillInTheBlankExercise fillInTheBlanksExercise)
            {
                isValid = fillInTheBlanksExercise.ValidateAnswer((List<string>)responses);
            }
            else if (currentExercise is MultipleChoiceExercise multipleChoiceExercise)
            {
                isValid = multipleChoiceExercise.ValidateAnswer((List<string>)responses);
            }
            ValidatedCurrent = isValid;
            UpdateQuiz(ValidatedCurrent);

            return isValid;
        }

        public bool LoadNext()
        {
            if (ValidatedCurrent == false)
                return false;

            CurrentExerciseIndex += 1;
            if (Exercises.Count <= CurrentExerciseIndex)
                CurrentExercise = null;
            else
                CurrentExercise = Exercises[CurrentExerciseIndex];

            return true;
        }

        public float GetPercentageDone()
        {
            return CurrentQuiz.ExerciseList.Count / CurrentQuiz.GetNumberOfAnswersGiven();
        }

        public float GetPercentageCorrect()
        {
            return CurrentQuiz.ExerciseList.Count / CurrentQuiz.GetNumberOfCorrectAnswers();
        }
    }
}
