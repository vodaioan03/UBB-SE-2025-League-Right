using Duo.Commands;
using Duo.Models.Exercises;
using Duo.Services;
using Duo.ViewModels.Base;
using Duo.ViewModels.CreateExerciseViewModels;
using Duo.Views.Components;
using Duo.Views.Components.CreateExerciseComponents;
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
    class ExerciseCreationViewModel : AdminBaseViewModel
    {
        private readonly ExerciseService _exerciseService;
        private object _selectedExerciseContent;

        private string _questionText = string.Empty;
        // List of exercise types
        public ObservableCollection<string> ExerciseTypes { get; set; }

        public ObservableCollection<string> Difficulties {  get; set; }

        // Selected exercise type
        private string _selectedExerciseType;

        private string _selectedDifficulty;

        private object _currentExerciseViewModel;


        public CreateAssociationExerciseViewModel CreateAssociationExerciseViewModel { get; }
        public CreateFillInTheBlankExerciseViewModel CreateFillInTheBlankExerciseViewModel { get; }
        public CreateMultipleChoiceExerciseViewModel CreateMultipleChoiceExerciseViewModel { get; }
        public CreateFlashcardExerciseViewModel CreateFlashcardExerciseViewModel { get; } = new();

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

            CreateMultipleChoiceExerciseViewModel = new CreateMultipleChoiceExerciseViewModel(this);
            CreateAssociationExerciseViewModel = new CreateAssociationExerciseViewModel(this);
            CreateFillInTheBlankExerciseViewModel = new CreateFillInTheBlankExerciseViewModel(this);

            SaveButtonCommand = new RelayCommand(CreateExercise);
            ExerciseTypes = new ObservableCollection<string>
            {
                "Association",
                "Fill in the blank",
                "Multiple Choice",
                "Flashcard"
            };

            Difficulties = new ObservableCollection<string>
            {
                "Easy",
                "Normal",
                "Hard"
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

        public string SelectedDifficulty
        {
            get => _selectedDifficulty;
            set
            {
                if (_selectedDifficulty != value)
                {
                    _selectedDifficulty = value;
                    OnPropertyChanged(nameof(SelectedDifficulty));
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

        private bool _isSuccessMessageVisible;
        public bool IsSuccessMessageVisible
        {
            get => _isSuccessMessageVisible;
            set => SetProperty(ref _isSuccessMessageVisible, value);
        }

        public void ShowSuccessMessage()
        {
            //IsSuccessMessageVisible = true;
            //Task.Delay(3000).ContinueWith(_ => IsSuccessMessageVisible = false);
        }


        private void UpdateExerciseContent(string exerciseType)
        {
            Debug.WriteLine(exerciseType);

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
                    SelectedExerciseContent = new CreateFlashcardExercise();
                    CurrentExerciseViewModel = CreateFlashcardExerciseViewModel;
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
            Debug.WriteLine(SelectedExerciseType);
            switch (SelectedExerciseType)
            {
                case "Multiple Choice":
                    CreateMultipleChoiceExercise();
                    break;
                case "Association":
                    CreateAssocitationExercise();
                    break;
                case "Flashcard":
                    CreateFlashcardExercise();
                    break;
                case "Fill in the blank":
                    CreateFillInTheBlankExercise();
                    break;
                default:
                    break;

            }
      
        }

        public async void CreateMultipleChoiceExercise()
        {
            try
            {
                Duo.Models.Difficulty difficulty = getDifficulty(SelectedDifficulty);
                Exercise newExercise = CreateMultipleChoiceExerciseViewModel.CreateExercise(QuestionText, difficulty);
                await _exerciseService.CreateExercise(newExercise);
                Debug.WriteLine(newExercise);
                ShowSuccessMessage();
                GoBack();
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
                RaiseErrorMessage(ex.Message,"");
            }
        }

        public async void CreateAssocitationExercise()
        {
            try
            {
                Duo.Models.Difficulty difficulty = getDifficulty(SelectedDifficulty);
                Exercise newExercise = CreateAssociationExerciseViewModel.CreateExercise(QuestionText, difficulty);
                await _exerciseService.CreateExercise(newExercise);
                Debug.WriteLine(newExercise);
                ShowSuccessMessage();
                GoBack();
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
                RaiseErrorMessage(ex.Message,"");
            }
        }

        public async void CreateFlashcardExercise()
        {
            try
            {
                Duo.Models.Difficulty difficulty = getDifficulty(SelectedDifficulty);
                Exercise newExercise = CreateFlashcardExerciseViewModel.CreateExercise(QuestionText, difficulty);
                await _exerciseService.CreateExercise(newExercise);
                Debug.WriteLine(newExercise);
                ShowSuccessMessage();
                GoBack();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                RaiseErrorMessage(ex.Message, "");
            }
        }

        public async void CreateFillInTheBlankExercise()
        {
            try
            {
                Duo.Models.Difficulty difficulty = getDifficulty(SelectedDifficulty);
                Exercise newExercise = CreateFillInTheBlankExerciseViewModel.CreateExercise(QuestionText, difficulty);
                await _exerciseService.CreateExercise(newExercise);
                Debug.WriteLine(newExercise);
                ShowSuccessMessage();
                GoBack();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                RaiseErrorMessage(ex.Message, "");
            }
        }

        private Duo.Models.Difficulty getDifficulty(string difficulty)
        {
            switch(difficulty)
            {
                case "Easy":
                    return Models.Difficulty.Easy;
                case "Normal":
                    return Models.Difficulty.Normal;
                case "Hard":
                    return Models.Difficulty.Hard;
                default:
                    return Models.Difficulty.Normal;
            }
        }
    }
}
