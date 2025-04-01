using System;
using System.Collections.Generic;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Duo.Models;
using Duo.Models.Exercises;

namespace Duo.Views.Pages
{
    /// <summary>
    /// A page for testing and interacting with flashcards
    /// </summary>
    public sealed partial class FlashcardsPage : Page
    {
        private List<Models.Exercises.FlashcardExercise> _flashcardExercises;
        private int _currentIndex = 0;
        
        public FlashcardsPage()
        {
            this.InitializeComponent();
            
            // Initialize a list of sample flashcard exercises
            LoadFlashcardExercises();
        }
        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            
            // Initialize the flashcard with the first exercise
            LoadCurrentExercise();
            
            // Update button states
            UpdateButtonStates();
        }

        private void LoadFlashcardExercises()
        {
            // Create sample flashcard exercises with proper constructor
            _flashcardExercises = new List<Models.Exercises.FlashcardExercise>
            {
                // Example of a fill-in-the-blank question (with ___ placeholder)
                new Models.Exercises.FlashcardExercise(
                    1, 
                    "The tallest mountain in the world is ___ with a height of 8,848 meters.",
                    "Mount Everest",
                    Models.Difficulty.Easy),
                
                // Original examples
                new Models.Exercises.FlashcardExercise(
                    2, 
                    "What is Earth's highest mountain above sea level?",
                    "Mount Everest",
                    Models.Difficulty.Normal),
                
                new Models.Exercises.FlashcardExercise(
                    3, 
                    "What is the capital city of France?",
                    "Paris",
                    Models.Difficulty.Easy),
                
                new Models.Exercises.FlashcardExercise(
                    4, 
                    "What is the longest river in the world?", 
                    "Nile River",
                    Models.Difficulty.Hard),
                
                new Models.Exercises.FlashcardExercise(
                    5, 
                    "What is the largest ocean on Earth?",
                    "Pacific Ocean",
                    Models.Difficulty.Normal),
                
                new Models.Exercises.FlashcardExercise(
                    6,
                    "Which planet is known as the Red Planet?", 
                    "Mars",
                    Models.Difficulty.Easy)
            };

            // Load first exercise
            if (_flashcardExercises.Count > 0)
            {
                _currentIndex = 0;
                LoadCurrentExercise();
            }
        }

        private void LoadCurrentExercise()
        {
            if (_currentIndex >= 0 && _currentIndex < _flashcardExercises.Count)
            {
                if (this.FlashcardExercise != null)
                {
                    this.FlashcardExercise.Initialize(_flashcardExercises[_currentIndex]);
                }
            }
        }
        
        private void UpdateButtonStates()
        {
            // Enable/disable buttons based on current index
            if (this.PreviousButton != null)
                this.PreviousButton.IsEnabled = _currentIndex > 0;
                
            if (this.NextButton != null)
                this.NextButton.IsEnabled = _currentIndex < _flashcardExercises.Count - 1;
        }
        
        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentIndex < _flashcardExercises.Count - 1)
            {
                _currentIndex++;
                LoadCurrentExercise();
                UpdateButtonStates();
            }
        }
        
        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentIndex > 0)
            {
                _currentIndex--;
                LoadCurrentExercise();
                UpdateButtonStates();
            }
        }
        
        // Event handler for when a flashcard is completed
        private void FlashcardExercise_ExerciseCompleted(object sender, bool isCorrect)
        {
            // Show feedback based on whether the answer was correct
            string message = isCorrect ? 
                "Correct! Well done." : 
                $"Incorrect. The correct answer is: {_flashcardExercises[_currentIndex].Answer}";
            
            ShowCompletionMessage(message);
        }

        // Event handler for when a flashcard is closed
        private void FlashcardExercise_ExerciseClosed(object sender, EventArgs e)
        {
            ShowCompletionMessage("Flashcard closed.");
        }

        private async void ShowCompletionMessage(string message)
        {
            if (this.XamlRoot != null)
            {
                ContentDialog dialog = new ContentDialog()
                {
                    Title = "Flashcard Result",
                    Content = message,
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                };

                await dialog.ShowAsync();
            }
        }
    }
} 