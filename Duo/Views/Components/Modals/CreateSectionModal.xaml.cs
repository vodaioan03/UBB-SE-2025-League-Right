using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.ObjectModel;
using Duo.Models.Quizzes;
using System.Collections.Generic;
using System.Linq;

namespace Duo.Views.Components.Modals
{
    public sealed partial class CreateSectionModal : UserControl
    {
        public event EventHandler<SectionCreatedEventArgs> SectionCreated;
        public event EventHandler ModalClosed;

        private readonly List<Quiz<object>> _availableQuizzes;
        public ObservableCollection<Quiz<object>> UnassignedQuizzes { get; private set; }

        public CreateSectionModal()
        {
            this.InitializeComponent();

            // Initialize hardcoded available quizzes
            _availableQuizzes = new List<Quiz<object>>
            {
                new Quiz<object>(1) { /* Add some exercises if needed */ },
                new Quiz<object>(2) { /* Add some exercises if needed */ },
                new Quiz<object>(3) { /* Add some exercises if needed */ },
                new Quiz<object>(4) { /* Add some exercises if needed */ },
                new Quiz<object>(5) { /* Add some exercises if needed */ }
            };

            UnassignedQuizzes = new ObservableCollection<Quiz<object>>();
            QuizUnassignedList.ItemsSource = UnassignedQuizzes;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var subject = SubjectTextBox.Text.Trim();

            if (string.IsNullOrEmpty(subject))
            {
                // Show error message
                var dialog = new ContentDialog
                {
                    Title = "Error",
                    Content = "Please enter a subject.",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                };
                dialog.ShowAsync();
                return;
            }

            // Raise the SectionCreated event with the new section data
            SectionCreated?.Invoke(this, new SectionCreatedEventArgs
            {
                Subject = subject,
                AssignedQuizzes = new List<Quiz<object>>(UnassignedQuizzes)
            });

            // Clear the form
            SubjectTextBox.Text = string.Empty;
            UnassignedQuizzes.Clear();

            // Close the modal
            ModalClosed?.Invoke(this, EventArgs.Empty);
        }

        private async void AddQuizButton_Click(object sender, RoutedEventArgs e)
        {
            var availableQuizzesToAdd = _availableQuizzes
                .Where(q => !UnassignedQuizzes.Any(uq => uq.Id == q.Id))
                .ToList();

            if (!availableQuizzesToAdd.Any())
            {
                var noQuizzesDialog = new ContentDialog
                {
                    Title = "No Quizzes Available",
                    Content = "All available quizzes have been added to the section.",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                };
                await noQuizzesDialog.ShowAsync();
                return;
            }

            // Create the quiz selection dialog
            var dialog = new ContentDialog
            {
                Title = "Select Quiz",
                CloseButtonText = "Cancel",
                PrimaryButtonText = "Add",
                XamlRoot = this.XamlRoot
            };

            var quizListView = new ListView
            {
                SelectionMode = ListViewSelectionMode.Single,
                Height = 300,
                Margin = new Thickness(0, 10, 0, 10),
                ItemTemplate = (DataTemplate)Resources["QuizSelectionItemTemplate"],
                ItemsSource = availableQuizzesToAdd
            };

            dialog.Content = quizListView;

            dialog.PrimaryButtonClick += (s, args) =>
            {
                if (quizListView.SelectedItem is Quiz<object> selectedQuiz)
                {
                    UnassignedQuizzes.Add(selectedQuiz);
                }
            };

            await dialog.ShowAsync();
        }

        private void RemoveQuiz_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Quiz<object> quizToRemove)
            {
                UnassignedQuizzes.Remove(quizToRemove);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Clear the form
            SubjectTextBox.Text = string.Empty;
            UnassignedQuizzes.Clear();

            ModalClosed?.Invoke(this, EventArgs.Empty);
        }
    }

    public class SectionCreatedEventArgs : EventArgs
    {
        public string Subject { get; set; }
        public List<Quiz<object>> AssignedQuizzes { get; set; }
    }
}