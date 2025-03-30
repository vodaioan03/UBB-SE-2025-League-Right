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
    public sealed partial class CreateExamModal : UserControl
    {
        public event EventHandler<ExamCreatedEventArgs> ExamCreated;
        public event EventHandler ModalClosed;

        private readonly List<Exercise> _availableExercises;
        public ObservableCollection<Exercise> SelectedExercises { get; private set; }

        private const int MAX_EXERCISES = 25; // From Exam class
        private const double DEFAULT_PASSING_THRESHOLD = 90; // From Exam class

        public CreateExamModal()
        {
            this.InitializeComponent();

            // Initialize available exercises using helper method
            _availableExercises = CreateSampleExercises();

            SelectedExercises = new ObservableCollection<Exercise>();
            ExerciseList.ItemsSource = SelectedExercises;

            // Subscribe to collection changed event
            SelectedExercises.CollectionChanged += (s, e) => UpdateExerciseCount();

            // Set default passing threshold
            PassingThresholdBox.Value = DEFAULT_PASSING_THRESHOLD;

            // Initial count update
            UpdateExerciseCount();
        }

        private List<Duo.Models.Exercises.Exercise> CreateSampleExercises()
        {
            var exercises = new List<Duo.Models.Exercises.Exercise>();

            try
            {
                // Fill in the blank exercises
                exercises.Add(new Duo.Models.Exercises.FillInTheBlankExercise(1, "The capital of France is ___.", Duo.Models.Difficulty.Normal, new List<string> { "Paris" }));
                exercises.Add(new Duo.Models.Exercises.FillInTheBlankExercise(2, "The largest planet in our solar system is ___.", Duo.Models.Difficulty.Normal, new List<string> { "Jupiter" }));

                // Association exercise
                var firstList = new List<string> { "H2O", "CO2" };
                var secondList = new List<string> { "Water", "Carbon Dioxide" };
                exercises.Add(new Duo.Models.Exercises.AssociationExercise(3, "Match chemical formulas with their names", Duo.Models.Difficulty.Hard, firstList, secondList));

                // Multiple choice exercise
                var choices = new List<Duo.Models.Exercises.MultipleChoiceAnswerModel>
                {
                    new Duo.Models.Exercises.MultipleChoiceAnswerModel { Answer = "Mercury", IsCorrect = true },
                    new Duo.Models.Exercises.MultipleChoiceAnswerModel { Answer = "Venus", IsCorrect = false },
                    new Duo.Models.Exercises.MultipleChoiceAnswerModel { Answer = "Mars", IsCorrect = false }
                };
                exercises.Add(new Duo.Models.Exercises.MultipleChoiceExercise(4, "Which is the closest planet to the Sun?", Duo.Models.Difficulty.Normal, choices, "Mercury"));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error creating exercises: {ex.Message}");
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }

            return exercises;
        }

        private void UpdateExerciseCount()
        {
            ExerciseCountText.Text = $"Selected Exercises: {SelectedExercises.Count}/{MAX_EXERCISES}";
        }

        private async void AddExerciseButton_Click(object sender, RoutedEventArgs e)
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
            if (SelectedExercises.Count == 0)
            {
                await ShowErrorMessage("No Exercises", "Please add at least one exercise to the exam.");
                return;
            }

            if (SelectedExercises.Count < MAX_EXERCISES)
            {
                await ShowErrorMessage("Insufficient Exercises", $"An exam must have exactly {MAX_EXERCISES} exercises.");
                return;
            }

            var exam = new Exam(new Random().Next(1000)); // Using random ID for demo

            foreach (var exercise in SelectedExercises)
            {
                exam.AddExercise(exercise);
            }

            ExamCreated?.Invoke(this, new ExamCreatedEventArgs(exam));
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

    public class ExamCreatedEventArgs : EventArgs
    {
        public Exam Exam { get; }

        public ExamCreatedEventArgs(Exam exam)
        {
            Exam = exam;
        }
    }
}