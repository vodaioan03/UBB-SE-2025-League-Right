using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Duo.Commands;
using Duo.Models.Exercises;
using Duo.Services;
using Duo.ViewModels.Base;
using Duo.ViewModels.CreateExerciseViewModels;
using Duo.Views.Components;
using Duo.Views.Components.CreateExerciseComponents;
using Microsoft.UI.Xaml.Controls;

namespace Duo.ViewModels
{
    partial class ExerciseCreationViewModel : AdminBaseViewModel
    {
        private readonly IExerciseService exerciseService;
        private object selectedExerciseContent;

        private string questionText = string.Empty;
        // List of exercise types
        public ObservableCollection<string> ExerciseTypes { get; set; }

        public ObservableCollection<string> Difficulties { get; set; }

        // Selected exercise type
        private string selectedExerciseType;

        private string selectedDifficulty;

        private object currentExerciseViewModel;

        public CreateAssociationExerciseViewModel CreateAssociationExerciseViewModel { get; }
        public CreateFillInTheBlankExerciseViewModel CreateFillInTheBlankExerciseViewModel { get; }
        public CreateMultipleChoiceExerciseViewModel CreateMultipleChoiceExerciseViewModel { get; }
        public CreateFlashcardExerciseViewModel CreateFlashcardExerciseViewModel { get; } = new ();

        public ExerciseCreationViewModel(IExerciseService exerciseService)
        {
            this.exerciseService = exerciseService;
        }

        public ExerciseCreationViewModel()
        {
            try
            {
                exerciseService = (IExerciseService)App.ServiceProvider.GetService(typeof(IExerciseService));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            CreateMultipleChoiceExerciseViewModel = new CreateMultipleChoiceExerciseViewModel(this);
            CreateAssociationExerciseViewModel = new CreateAssociationExerciseViewModel(this);
            CreateFillInTheBlankExerciseViewModel = new CreateFillInTheBlankExerciseViewModel(this);

            SaveButtonCommand = new RelayCommand(CreateExercise);
            /*ExerciseTypes = new ObservableCollection<string>
            {
                "Association",
                "Fill in the blank",
                "Multiple Choice",
                "Flashcard"
            };*/
            ExerciseTypes = new ObservableCollection<string>(Models.Exercises.ExerciseTypes.EXERCISE_TYPES);

            /*Difficulties = new ObservableCollection<string>
            {
                "Easy",
                "Normal",
                "Hard"
            };*/
            Difficulties = new ObservableCollection<string>(Models.DifficultyList.DIFFICULTIES);

            SelectedExerciseContent = new TextBlock { Text = "Select an exercise type." };
            currentExerciseViewModel = CreateAssociationExerciseViewModel;
        }

        public string QuestionText
        {
            get => questionText;
            set
            {
                if (questionText != value)
                {
                    questionText = value;
                    OnPropertyChanged(nameof(QuestionText)); // Notify UI
                }
            }
        }

        public string SelectedExerciseType
        {
            get => selectedExerciseType;
            set
            {
                if (selectedExerciseType != value)
                {
                    selectedExerciseType = value;
                    OnPropertyChanged(nameof(SelectedExerciseType));
                    Debug.WriteLine(value);
                    UpdateExerciseContent(value);
                }
            }
        }

        public string SelectedDifficulty
        {
            get => selectedDifficulty;
            set
            {
                if (selectedDifficulty != value)
                {
                    selectedDifficulty = value;
                    OnPropertyChanged(nameof(SelectedDifficulty));
                }
            }
        }

        public object SelectedExerciseContent
        {
            get => selectedExerciseContent;
            set
            {
                if (selectedExerciseContent != value)
                {
                    selectedExerciseContent = value;
                    OnPropertyChanged(nameof(SelectedExerciseContent));
                }
            }
        }

        public object CurrentExerciseViewModel
        {
            get => currentExerciseViewModel;
            set
            {
                currentExerciseViewModel = value;
                OnPropertyChanged(nameof(CurrentExerciseViewModel));
            }
        }

        private bool isSuccessMessageVisible;
        public bool IsSuccessMessageVisible
        {
            get => isSuccessMessageVisible;
            set => SetProperty(ref isSuccessMessageVisible, value);
        }

        public void ShowSuccessMessage()
        {
            // IsSuccessMessageVisible = true;
            // Task.Delay(3000).ContinueWith(_ => IsSuccessMessageVisible = false);
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
                Duo.Models.Difficulty difficulty = GetDifficulty(SelectedDifficulty);
                Exercise newExercise = CreateMultipleChoiceExerciseViewModel.CreateExercise(QuestionText, difficulty);
                await exerciseService.CreateExercise(newExercise);
                Debug.WriteLine(newExercise);
                ShowSuccessMessage();
                GoBack();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                RaiseErrorMessage(ex.Message, string.Empty);
            }
        }

        public async void CreateAssocitationExercise()
        {
            try
            {
                Duo.Models.Difficulty difficulty = GetDifficulty(SelectedDifficulty);
                Exercise newExercise = CreateAssociationExerciseViewModel.CreateExercise(QuestionText, difficulty);
                await exerciseService.CreateExercise(newExercise);
                Debug.WriteLine(newExercise);
                ShowSuccessMessage();
                GoBack();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                RaiseErrorMessage(ex.Message, string.Empty);
            }
        }

        public async void CreateFlashcardExercise()
        {
            try
            {
                Duo.Models.Difficulty difficulty = GetDifficulty(SelectedDifficulty);
                Exercise newExercise = CreateFlashcardExerciseViewModel.CreateExercise(QuestionText, difficulty);
                await exerciseService.CreateExercise(newExercise);
                Debug.WriteLine(newExercise);
                ShowSuccessMessage();
                GoBack();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                RaiseErrorMessage(ex.Message, string.Empty);
            }
        }

        public async void CreateFillInTheBlankExercise()
        {
            try
            {
                Duo.Models.Difficulty difficulty = GetDifficulty(SelectedDifficulty);
                Exercise newExercise = CreateFillInTheBlankExerciseViewModel.CreateExercise(QuestionText, difficulty);
                await exerciseService.CreateExercise(newExercise);
                Debug.WriteLine(newExercise);
                ShowSuccessMessage();
                GoBack();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                RaiseErrorMessage(ex.Message, string.Empty);
            }
        }

        private Duo.Models.Difficulty GetDifficulty(string difficulty)
        {
            switch (difficulty)
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
