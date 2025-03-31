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
        private int _quizId;
        private List<Exercise> _exercises;
        private int _currentExerciseIndex;

        public int QuizId
        {
            get => _quizId;
            set
            {
                _quizId = value;
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

        public int CurrentExerciseIndex
        {
            get => _currentExerciseIndex;
            set
            {
                _currentExerciseIndex = value;
                OnPropertyChanged(nameof(CurrentExerciseIndex));
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
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
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
                    new MultipleChoiceExercise(2, "What is uhh", Models.Difficulty.Easy, new List<MultipleChoiceAnswerModel>{new MultipleChoiceAnswerModel { Answer="hi", IsCorrect=false }, new MultipleChoiceAnswerModel { Answer="hi", IsCorrect=true } }),
                    new FillInTheBlankExercise(3, "Hi {} insert there {}", Models.Difficulty.Easy, new List<string>{"hi", "hi"}),
                    new AssociationExercise(4, "Match these things", Models.Difficulty.Normal ,new List<string>{"ameroicaaaa", "asmakdjskjdh", "hjdiahsd"}, new List<string>{"dnchuksgf", "shndus", "snhdajhvjdh"}),
                    new AssociationExercise(5, "Match these things", Models.Difficulty.Normal ,new List<string>{"ameroicaaaa", "asmakdjskjdh", "hjdiahsd"}, new List<string>{"dnchuksgf", "shndus", "snhdajhvjdh"}),
                };

                CurrentExerciseIndex = 0;

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading exercises: {ex.Message}");
                Exercises = new List<Exercise>();
            }
        }

        public bool ValidateCurrentExercise(object responses)
        {
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

            return isValid;
        }
    }
}
