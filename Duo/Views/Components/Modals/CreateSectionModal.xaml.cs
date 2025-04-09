using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Duo.Models.Quizzes;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Duo.Views.Components.Modals
{
    public sealed partial class CreateSectionModal : UserControl
    {
        public event EventHandler<SectionCreatedEventArgs> SectionCreated;
        public event EventHandler ModalClosed;

        private readonly List<Quiz> availableQuizzes;
        private readonly List<Exam> availableExams;
        public ObservableCollection<Quiz> UnassignedQuizzes { get; private set; }
        public ObservableCollection<Exam> SelectedExam { get; private set; }

        public CreateSectionModal()
        {
            this.InitializeComponent();

            // Initialize hardcoded available quizzes
            availableQuizzes = new List<Quiz>
            {
                new Quiz(1, null, null) { /* Add some exercises if needed */ },
                new Quiz(2, null, null) { /* Add some exercises if needed */ },
                new Quiz(3, null, null) { /* Add some exercises if needed */ },
                new Quiz(4, null, null) { /* Add some exercises if needed */ },
                new Quiz(5, null, null) { /* Add some exercises if needed */ }
            };

            // Initialize hardcoded available exams
            availableExams = new List<Exam>
            {
                new Exam(1, null) { /* Add some exercises if needed */ },
                new Exam(2, null) { /* Add some exercises if needed */ },
                new Exam(3, null) { /* Add some exercises if needed */ }
            };

            UnassignedQuizzes = new ObservableCollection<Quiz>();
            SelectedExam = new ObservableCollection<Exam>();

            QuizUnassignedList.ItemsSource = UnassignedQuizzes;
            SelectedExamList.ItemsSource = SelectedExam;
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

            if (!SelectedExam.Any())
            {
                // Show error message
                var dialog = new ContentDialog
                {
                    Title = "Error",
                    Content = "Please select a final exam for the section.",
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
                AssignedQuizzes = new List<Quiz>(UnassignedQuizzes),
                FinalExam = SelectedExam.First()
            });

            // Clear the form
            SubjectTextBox.Text = string.Empty;
            UnassignedQuizzes.Clear();
            SelectedExam.Clear();

            // Close the modal
            ModalClosed?.Invoke(this, EventArgs.Empty);
        }

        private async void AddExamButton_Click(object sender, RoutedEventArgs e)
        {
            var availableExamsToAdd = availableExams
                .Where(e => !SelectedExam.Any(se => se.Id == e.Id))
                .ToList();

            if (!availableExamsToAdd.Any())
            {
                var noExamsDialog = new ContentDialog
                {
                    Title = "No Exams Available",
                    Content = "All available exams have been assigned to sections.",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                };
                await noExamsDialog.ShowAsync();
                return;
            }

            // Create the exam selection dialog
            var dialog = new ContentDialog
            {
                Title = "Select Final Exam",
                CloseButtonText = "Cancel",
                PrimaryButtonText = "Add",
                XamlRoot = this.XamlRoot
            };

            var examListView = new ListView
            {
                SelectionMode = ListViewSelectionMode.Single,
                Height = 300,
                Margin = new Thickness(0, 10, 0, 10),
                ItemTemplate = (DataTemplate)Resources["QuizSelectionItemTemplate"],
                ItemsSource = availableExamsToAdd
            };

            dialog.Content = examListView;

            dialog.PrimaryButtonClick += (s, args) =>
            {
                if (examListView.SelectedItem is Exam selectedExam)
                {
                    SelectedExam.Clear(); // Only allow one exam
                    SelectedExam.Add(selectedExam);
                }
            };

            await dialog.ShowAsync();
        }

        private async void AddQuizButton_Click(object sender, RoutedEventArgs e)
        {
            var availableQuizzesToAdd = availableQuizzes
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
                if (quizListView.SelectedItem is Quiz selectedQuiz)
                {
                    UnassignedQuizzes.Add(selectedQuiz);
                }
            };

            await dialog.ShowAsync();
        }

        private void RemoveQuiz_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Quiz quizToRemove)
            {
                UnassignedQuizzes.Remove(quizToRemove);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Clear the form
            SubjectTextBox.Text = string.Empty;
            UnassignedQuizzes.Clear();
            SelectedExam.Clear();

            ModalClosed?.Invoke(this, EventArgs.Empty);
        }
    }

    public class SectionCreatedEventArgs : EventArgs
    {
        public string Subject { get; set; }
        public List<Quiz> AssignedQuizzes { get; set; }
        public Exam FinalExam { get; set; }
    }
}