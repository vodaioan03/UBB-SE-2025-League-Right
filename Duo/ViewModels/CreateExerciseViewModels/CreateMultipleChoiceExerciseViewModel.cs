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
using Duo.ViewModels.Base;
using Duo.ViewModels.ExerciseViewModels;

namespace Duo.ViewModels.CreateExerciseViewModels
{
    partial class CreateMultipleChoiceExerciseViewModel : CreateExerciseViewModelBase
    {
        private ExerciseCreationViewModel parentViewModel;
        public const int MINIMUM_ANSWERS = 2;
        public const int MAXIMUM_ANSWERS = 5;

        private string selectedAnswer = string.Empty;

        public ObservableCollection<Answer> Answers { get; set; } = new ObservableCollection<Answer>();

        public CreateMultipleChoiceExerciseViewModel(ExerciseCreationViewModel parentViewModel)
        {
            this.parentViewModel = parentViewModel;
            AddNewAnswerCommand = new RelayCommand(AddNewAnswer);
            UpdateSelectedAnswerComand = new RelayCommandWithParameter<string>(UpdateSelectedAnswer);
        }

        public override Exercise CreateExercise(string questionText, Difficulty difficulty)
        {
            List<MultipleChoiceAnswerModel> multipleChoiceAnswerModelList = GenerateAnswerModelList();
            Exercise newExercise = new Models.Exercises.MultipleChoiceExercise(0, questionText, difficulty, multipleChoiceAnswerModelList);
            return newExercise;
        }

        public List<MultipleChoiceAnswerModel> GenerateAnswerModelList()
        {
            List<Answer> finalAnswers = Answers.ToList();
            List<MultipleChoiceAnswerModel> multipleChoiceAnswerModels = new List<MultipleChoiceAnswerModel>();
            foreach (Answer answer in finalAnswers)
            {
                multipleChoiceAnswerModels.Add(new ()
                {
                    Answer = answer.Value,
                    IsCorrect = answer.IsCorrect
                });
            }
            return multipleChoiceAnswerModels;
        }

        public string SelectedAnswer
        {
            get => selectedAnswer;
            set
            {
                selectedAnswer = value;
                OnPropertyChanged(nameof(SelectedAnswer));
            }
        }

        public ICommand AddNewAnswerCommand { get; }
        public ICommand UpdateSelectedAnswerComand { get; }

        private void AddNewAnswer()
        {
            if (Answers.Count == MAXIMUM_ANSWERS)
            {
                parentViewModel.RaiseErrorMessage("Cannot add more answers", $"Maximum number of answers ({MAXIMUM_ANSWERS}) reached.");
                return;
            }
            Answers.Add(new Answer(string.Empty, false));
        }

        private void UpdateSelectedAnswer(string selectedValue)
        {
            foreach (var answer in Answers)
            {
                answer.IsCorrect = answer.Value == selectedValue;
            }

            SelectedAnswer = selectedValue; // Update the selected answer reference
            OnPropertyChanged(nameof(Answers)); // Notify UI
        }

        public class Answer : ViewModelBase
        {
            private string value;
            private bool isCorrect;

            public string Value
            {
                get => value;
                set
                {
                    this.value = value;
                    OnPropertyChanged(nameof(Value));
                }
            }

            public bool IsCorrect
            {
                get => isCorrect;
                set
                {
                    if (isCorrect != value) // Prevent unnecessary updates
                    {
                        isCorrect = value;
                        OnPropertyChanged(nameof(isCorrect));
                    }
                }
            }

            public Answer(string value, bool isCorrect)
            {
                this.value = value;
                this.isCorrect = isCorrect;
            }
        }
    }
}
