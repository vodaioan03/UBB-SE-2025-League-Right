using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.ObjectModel;
using Duo.Models;
using Duo.Models.Exercises;
using Duo.Models.Quizzes;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Duo.Views.Components.Modals
{
    public sealed partial class CreateQuizModal : UserControl
    {
        public event EventHandler<QuizCreatedEventArgs> QuizCreated;
        public event EventHandler ModalClosed;

        private readonly List<Exercise> _availableExercises;
        public ObservableCollection<Exercise> SelectedExercises { get; private set; }

        private const int MAX_EXERCISES = 10;

        public CreateQuizModal()
        {
            this.InitializeComponent();

            // Initialize available exercises using helper method
            _availableExercises = CreateSampleExercises();

            SelectedExercises = new ObservableCollection<Exercise>();
            ExerciseList.ItemsSource = SelectedExercises;

            // Subscribe to collection changed event
            SelectedExercises.CollectionChanged += (s, e) => UpdateExerciseCount();

            // Initial count update
            UpdateExerciseCount();
        }

        private List<Exercise> CreateSampleExercises()
        {
            var exercises = new List<Exercise>();

            try
            {
                // Fill in the blank exercises
                exercises.Add(new FillInTheBlankExercise(1, "The sky is ___.", Difficulty.Normal, new List<string> { "blue" }));
                exercises.Add(new FillInTheBlankExercise(2, "Water boils at ___ degrees Celsius.", Difficulty.Normal, new List<string> { "100" }));

                // Association exercise - using fully qualified namespace
                var firstList = new List<string> { "Dog", "Cat" };
                var secondList = new List<string> { "Bark", "Meow" };
                exercises.Add(new Duo.Models.Exercises.AssociationExercise(3, "Match animals with their sounds", Difficulty.Hard, firstList, secondList));
            }
            catch (Exception ex)
            {
                // Add error handling for debugging
                System.Diagnostics.Debug.WriteLine($"Error creating exercises: {ex.Message}");
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }

            return exercises;
        }

        private void UpdateExerciseCount()
        {
            // Update the count display if you have one
            // ExerciseCountText.Text = $"Selected Exercises: {SelectedExercises.Count}/{MAX_EXERCISES}";
        }

        private async void AddExercise_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ContentDialog
            {
                Title = "Select Exercise",
                CloseButtonText = "Cancel",
                DefaultButton = ContentDialogButton.Primary,
                XamlRoot = this.XamlRoot
            };

            var listView = new ListView
            {
                ItemsSource = _availableExercises.Where(ex => !SelectedExercises.Contains(ex)).ToList(),
                SelectionMode = ListViewSelectionMode.Single,
                MaxHeight = 300,
                ItemTemplate = (DataTemplate)Resources["ExerciseSelectionItemTemplate"]
            };

            dialog.Content = listView;
            dialog.PrimaryButtonText = "Add";
            dialog.IsPrimaryButtonEnabled = false;

            listView.SelectionChanged += (s, args) =>
            {
                dialog.IsPrimaryButtonEnabled = listView.SelectedItem != null;
            };

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary && listView.SelectedItem is Exercise selectedExercise)
            {
                if (SelectedExercises.Count < MAX_EXERCISES)
                {
                    SelectedExercises.Add(selectedExercise);
                }
                else
                {
                    await ShowErrorMessage("Cannot add more exercises", $"Maximum number of exercises ({MAX_EXERCISES}) reached.");
                }
            }
        }

        private void RemoveExercise_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Exercise exercise)
            {
                SelectedExercises.Remove(exercise);
            }
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TitleTextBox.Text))
            {
                await ShowErrorMessage("Invalid Title", "Please enter a title for the quiz.");
                return;
            }

            if (SelectedExercises.Count == 0)
            {
                await ShowErrorMessage("No Exercises", "Please add at least one exercise to the quiz.");
                return;
            }

            var quiz = new Quiz(new Random().Next(1000), null, null); // Using random ID for demo
            foreach (var exercise in SelectedExercises)
            {
                quiz.AddExercise(exercise);
            }

            QuizCreated?.Invoke(this, new QuizCreatedEventArgs(quiz, TitleTextBox.Text));
            ModalClosed?.Invoke(this, EventArgs.Empty);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ModalClosed?.Invoke(this, EventArgs.Empty);
        }

        private async Task ShowErrorMessage(string title, string message)
        {
            var dialog = new ContentDialog
            {
                Title = title,
                Content = message,
                CloseButtonText = "OK",
                XamlRoot = this.XamlRoot
            };

            await dialog.ShowAsync();
        }
    }

    public class QuizCreatedEventArgs : EventArgs
    {
        public Quiz Quiz { get; }
        public string Title { get; }

        public QuizCreatedEventArgs(Quiz quiz, string title)
        {
            Quiz = quiz;
            Title = title;
        }
    }
}