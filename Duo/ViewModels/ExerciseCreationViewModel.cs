using Duo.Commands;
using Duo.Models.Exercises;
using Duo.Services;
using Duo.ViewModels.Base;
using Duo.ViewModels.CreateExerciseViewModels;
using Duo.Views.Components;
using Microsoft.UI.Xaml.Controls;
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
    class ExerciseCreationViewModel : ViewModelBase
    {
        private readonly ExerciseService _exerciseService;
        private object _selectedExerciseContent;

        private string _questionText = string.Empty;
        // List of exercise types
        public ObservableCollection<string> ExerciseTypes { get; set; }

        // Selected exercise type
        private string _selectedExerciseType;

        private object _currentExerciseViewModel;


        public CreateAssociationExerciseViewModel CreateAssociationExerciseViewModel { get; } = new();
        public CreateFillInTheBlankExerciseViewModel CreateFillInTheBlankExerciseViewModel { get; } = new();
        public CreateMultipleChoiceExerciseViewModel CreateMultipleChoiceExerciseViewModel { get; } = new();
        
        public event EventHandler RequestGoBack;
        public ExerciseCreationViewModel(ExerciseService exerciseService) 
        {
            _exerciseService = exerciseService;
        }

        public ExerciseCreationViewModel()
        {
            try
            {
                _exerciseService = (ExerciseService)App.serviceProvider.GetService(typeof(ExerciseService));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            SaveButtonCommand = new RelayCommand(CreateExercise);
            ExerciseTypes = new ObservableCollection<string>
            {
                "Association",
                "Fill in the blank",
                "Multiple Choice",
                "Flashcard"
            };

            SelectedExerciseContent = new TextBlock { Text = "Select an exercise type." };
            _currentExerciseViewModel = CreateAssociationExerciseViewModel;
        }


        public string QuestionText
        {
            get => _questionText;
            set
            {
                if (_questionText != value)
                {
                    _questionText = value;
                    OnPropertyChanged(nameof(QuestionText)); // Notify UI
                }
            }
        }
        public string SelectedExerciseType
        {
            get => _selectedExerciseType;
            set
            {
                if (_selectedExerciseType != value)
                {
                    _selectedExerciseType = value;
                    OnPropertyChanged(nameof(SelectedExerciseType));
                    Debug.WriteLine(value);
                    UpdateExerciseContent(value);
                }
            }
        }
        public object SelectedExerciseContent
        {
            get => _selectedExerciseContent;
            set
            {
                if (_selectedExerciseContent != value)
                {
                    _selectedExerciseContent = value;
                    OnPropertyChanged(nameof(SelectedExerciseContent));
                }
            }
        }

        public object CurrentExerciseViewModel
        {
            get => _currentExerciseViewModel;
            set
            {
                _currentExerciseViewModel = value;
                OnPropertyChanged(nameof(CurrentExerciseViewModel));
            }
        }

        private void UpdateExerciseContent(string exerciseType)
        {
            switch (exerciseType)
            {
                case "Association":
                    SelectedExerciseContent = new CreateAssociationExercise();
                    CurrentExerciseViewModel = CreateAssociationExerciseViewModel;
                    break;
                case "Fill in the blank":
                    SelectedExerciseContent = new CreateFillInTheBlankExercise();
                    CurrentExerciseViewModel = CreateFillInTheBlankExerciseViewModel;
                    break;
                case "Multiple Choice":
                    SelectedExerciseContent = new CreateMultipleChoiceExercise();
                    CurrentExerciseViewModel = CreateMultipleChoiceExerciseViewModel;
                    break;
                case "Flashcard":
                    // You can set Flashcard content here, or leave it as null
                    break;
                default:
                    SelectedExerciseContent = new TextBlock { Text = "Select an exercise type." };
                    break;
            }
        }

        public ICommand SaveButtonCommand { get; }

        public void CreateExercise()
        {
            switch (SelectedExerciseType)
            {
                case "Multiple Choice":
                    CreateMultipleChoiceExercise();
                    break;
                case "Association":
                    CreateAssocitationExercise();
                    break;
                default:
                    break;

            }
      
        }

        public void CreateMultipleChoiceExercise()
        {
            Exercise newExercise = CreateMultipleChoiceExerciseViewModel.CreateExercise(QuestionText, Models.Difficulty.Easy);
            _exerciseService.CreateExercise(newExercise);
            Debug.WriteLine(newExercise);
            RequestGoBack?.Invoke(this, EventArgs.Empty);
        }

        public void CreateAssocitationExercise()
        {
            Exercise newExercise = CreateAssociationExerciseViewModel.CreateExercise(QuestionText,Models.Difficulty.Easy);
            _exerciseService.CreateExercise(newExercise);
            Debug.WriteLine(newExercise);
            RequestGoBack?.Invoke(this, EventArgs.Empty);
        }


    }
}
