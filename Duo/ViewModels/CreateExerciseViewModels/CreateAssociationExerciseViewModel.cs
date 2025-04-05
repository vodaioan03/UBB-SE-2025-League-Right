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
using static Duo.ViewModels.CreateExerciseViewModels.CreateAssociationExerciseViewModel;
using static Duo.ViewModels.CreateExerciseViewModels.CreateMultipleChoiceExerciseViewModel;

namespace Duo.ViewModels.CreateExerciseViewModels
{
    internal partial class CreateAssociationExerciseViewModel : CreateExerciseViewModelBase
    {
        private readonly ExerciseCreationViewModel parentViewModel;
        public ObservableCollection<Answer> LeftSideAnswers { get; set; } = new ObservableCollection<Answer>();

        public ObservableCollection<Answer> RightSideAnswers { get; set; } = new ObservableCollection<Answer>();
        public const int MINIMUM_ANSWERS = 2;
        public const int MAXIMUM_ANSWERS = 5;

        public ICommand AddNewAnswerCommand { get; }

        public CreateAssociationExerciseViewModel(ExerciseCreationViewModel parentViewModel)
        {
            this.parentViewModel = parentViewModel;
            LeftSideAnswers.Add(new Answer(string.Empty));
            RightSideAnswers.Add(new Answer(string.Empty));
            AddNewAnswerCommand = new RelayCommand(AddNewAnswer);
        }

        private void AddNewAnswer()
        {
            if (LeftSideAnswers.Count >= MAXIMUM_ANSWERS || RightSideAnswers.Count >= MAXIMUM_ANSWERS)
            {
                parentViewModel.RaiseErrorMessage("You can only have up to 5 answers", string.Empty);
                return;
            }
            Debug.WriteLine($"New answer");
            LeftSideAnswers.Add(new Answer(string.Empty));
            RightSideAnswers.Add(new Answer(string.Empty));
        }

        public override Exercise CreateExercise(string question, Difficulty difficulty)
        {
            Exercise newExercise = new Models.Exercises.AssociationExercise(0, question, difficulty, GenerateAnswerList(LeftSideAnswers), GenerateAnswerList(RightSideAnswers));
            return newExercise;
        }

        public List<string> GenerateAnswerList(ObservableCollection<Answer> answers)
        {
            List<Answer> finalAnswers = answers.ToList();
            List<string> answersList = new List<string>();
            foreach (Answer answer in finalAnswers)
            {
                answersList.Add(answer.Value);
            }
            return answersList;
        }

        public class Answer : ViewModelBase
        {
            private string value;
            public string Value
            {
                get => value;
                set
                {
                    this.value = value;
                    OnPropertyChanged(nameof(Value));
                }
            }

            public Answer(string value)
            {
                this.value = value;
            }
        }
    }
}
