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
    partial class CreateFillInTheBlankExerciseViewModel : CreateExerciseViewModelBase
    {
        private ExerciseCreationViewModel parentViewModel;
        public ObservableCollection<Answer> Answers { get; set; } = new ObservableCollection<Answer>();
        public const int MAX_ANSWERS = 3;

        public CreateFillInTheBlankExerciseViewModel(ExerciseCreationViewModel parentViewModel)
        {
            this.parentViewModel = parentViewModel;
            Answers.Add(new Answer(string.Empty));
            AddNewAnswerCommand = new RelayCommand(AddNewAnswer);
        }

        public ICommand AddNewAnswerCommand { get; }

        public override Exercise CreateExercise(string question, Difficulty difficulty)
        {
            Exercise newExercise = new Models.Exercises.FillInTheBlankExercise(0, question, difficulty, GenerateAnswerList(Answers));
            return newExercise;
        }

        public List<string> GenerateAnswerList(ObservableCollection<Answer> answers)
        {
            List<string> answerList = new List<string>();
            foreach (Answer answer in answers)
            {
                answerList.Add(answer.Value);
            }
            return answerList;
        }

        public void AddNewAnswer()
        {
            if (Answers.Count >= MAX_ANSWERS)
            {
                parentViewModel.RaiseErrorMessage("You can only have 3 answers for a fill in the blank exercise.", string.Empty);
                return;
            }
            Answers.Add(new Answer(string.Empty));
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
