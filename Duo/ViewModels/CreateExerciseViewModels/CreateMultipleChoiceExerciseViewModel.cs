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

        private ObservableCollection<string> _answers;

        public CreateMultipleChoiceExerciseViewModel() 
        {
            _answers = new ObservableCollection<string>();
            _answers.Add("");
            AddNewAnswerCommand = new RelayCommand(AddNewAnswer);
        }

        public override Exercise CreateExercise(string questionText, Difficulty difficulty)
        {
            List<MultipleChoiceAnswerModel> multipleChoiceAnswerModelList = generateAnswerModelList();
            Exercise newExercise = new Models.Exercises.MultipleChoiceExercise(0, questionText, difficulty, multipleChoiceAnswerModelList);
            return newExercise;
        }

        public List<MultipleChoiceAnswerModel> generateAnswerModelList()
        {
            List<string> finalAnswers = Answers.ToList();
            List<MultipleChoiceAnswerModel> multipleChoiceAnswerModels = new List<MultipleChoiceAnswerModel>();
            foreach(string answer in finalAnswers)
            {
                Debug.WriteLine("test");
                Debug.WriteLine(answer);
                multipleChoiceAnswerModels.Add(new()
                {
                    Answer = answer,
                    IsCorrect = true
                });
            }
            return multipleChoiceAnswerModels;
        }

        public ObservableCollection<string> Answers
        {
            get => _answers;
            set
            {
                if (_answers != value)
                {
                    _answers = value;
                    OnPropertyChanged(nameof(Answers));
                }
            }
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

        private void AddNewAnswer()
        {
            Debug.WriteLine($"New answer");
            _answers.Add("test");
        }


    }
}
