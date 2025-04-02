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
using static Duo.ViewModels.CreateExerciseViewModels.CreateAssociationExerciseViewModel;
using static Duo.ViewModels.CreateExerciseViewModels.CreateMultipleChoiceExerciseViewModel;

namespace Duo.ViewModels.CreateExerciseViewModels
{
    class CreateAssociationExerciseViewModel : CreateExerciseViewModelBase
    {
        private ExerciseCreationViewModel _parentViewModel;
        public ObservableCollection<Answer> LeftSideAnswers { get; set; } = new ObservableCollection<Answer>();

        public ObservableCollection<Answer> RightSideAnswers { get; set; } = new ObservableCollection<Answer>();
        public const int MINIMUM_ANSWERS = 2;
        public const int MAXIMUM_ANSWERS = 5;
        public CreateAssociationExerciseViewModel(ExerciseCreationViewModel parentViewModel)
        {
            _parentViewModel = parentViewModel;
            LeftSideAnswers.Add(new Answer(""));
            RightSideAnswers.Add(new Answer(""));
            AddNewAnswerCommand = new RelayCommand(AddNewAnswer);
        }

        public ICommand AddNewAnswerCommand { get; }

        private void AddNewAnswer()
        {
            if(LeftSideAnswers.Count >= MAXIMUM_ANSWERS || RightSideAnswers.Count >= MAXIMUM_ANSWERS)
            {
                _parentViewModel.RaiseErrorMessage("You can only have up to 5 answers","");
                return;
            }
            Debug.WriteLine($"New answer");
            LeftSideAnswers.Add(new Answer(""));
            RightSideAnswers.Add(new Answer(""));
        }

        public override Exercise CreateExercise(string question, Difficulty difficulty)
        {
            Exercise newExercise = new Models.Exercises.AssociationExercise(0, question, difficulty, generateAnswerList(LeftSideAnswers), generateAnswerList(RightSideAnswers));
            return newExercise;
        }

        public List<string> generateAnswerList(ObservableCollection<Answer> Answers)
        {
            List<Answer> finalAnswers = Answers.ToList();
            List<string> AnswersList = new List<string>();
            foreach (Answer answer in finalAnswers)
            {
                Debug.WriteLine(answer.Value);
                AnswersList.Add(answer.Value);
            }
            return AnswersList;
        }

        public class Answer : ViewModelBase
        {
            private string _value;
            public string Value
            {
                get => _value;
                set
                {
                    _value = value;
                    OnPropertyChanged(nameof(Value));
                }
            }

            public Answer(string value)
            {
                _value = value;
            }
        }

    }
}
