using Duo.Commands;
using Duo.Models;
using Duo.Models.Exercises;
using Duo.ViewModels.Base;
using Duo.ViewModels.ExerciseViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Duo.ViewModels.CreateExerciseViewModels
{
    class CreateMultipleChoiceExerciseViewModel : CreateExerciseViewModelBase
    {
        private string _selectedAnswer;

        public ObservableCollection<Answer> Answers { get; set; } = new ObservableCollection<Answer>();

        public CreateMultipleChoiceExerciseViewModel()
        {
            AddNewAnswerCommand = new RelayCommand(AddNewAnswer);
            UpdateSelectedAnswerComand = new RelayCommandWithParameter<string>(UpdateSelectedAnswer);
        }

        public override Exercise CreateExercise(string questionText, Difficulty difficulty)
        {
            try
            {
                List<MultipleChoiceAnswerModel> multipleChoiceAnswerModelList = generateAnswerModelList();
                Exercise newExercise = new Models.Exercises.MultipleChoiceExercise(0, questionText, difficulty, multipleChoiceAnswerModelList);
                return newExercise;
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
                return null;
            }
        }

        public List<MultipleChoiceAnswerModel> generateAnswerModelList()
        {
            List<Answer> finalAnswers = Answers.ToList();
            List<MultipleChoiceAnswerModel> multipleChoiceAnswerModels = new List<MultipleChoiceAnswerModel>();
            foreach(Answer answer in finalAnswers)
            {
                Debug.WriteLine(answer.Value);
                Debug.WriteLine(answer.isCorrect);
                multipleChoiceAnswerModels.Add(new()
                {
                    Answer = answer.Value,
                    IsCorrect = answer.isCorrect
                });
            }
            return multipleChoiceAnswerModels;
        }

        public string SelectedAnswer
        {
            get => _selectedAnswer;
            set
            {
                _selectedAnswer = value;
                OnPropertyChanged(nameof(SelectedAnswer));
            }
        }


        public ICommand AddNewAnswerCommand { get; }
        public ICommand UpdateSelectedAnswerComand { get; }

        private void AddNewAnswer()
        {
            Debug.WriteLine($"New answer");
            Answers.Add(new Answer("", false));
        }
        private void UpdateSelectedAnswer(string selectedValue)
        {
            foreach (var answer in Answers)
            {
                answer.isCorrect = answer.Value == selectedValue;
            }

            SelectedAnswer = selectedValue; // Update the selected answer reference
            OnPropertyChanged(nameof(Answers)); // Notify UI
        }

        public class Answer : ViewModelBase
        {
            private string _value;
            private bool _isCorrect;
            public string Value
            {
                get => _value;
                set
                {
                    _value = value;
                    OnPropertyChanged(nameof(Value));
                }
            }

            public bool isCorrect
            {
                get => _isCorrect;
                set
                {
                    if (_isCorrect != value) // Prevent unnecessary updates
                    {
                        _isCorrect = value;
                        OnPropertyChanged(nameof(isCorrect));
                    }
                }
            }

            public Answer(string value, bool isCorrect)
            {
                _value = value;
                _isCorrect = isCorrect;
            }
        }

    }
}
