using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Duo.Models.Quizzes;

namespace Duo.Views.Pages
{
    public sealed partial class EndQuizPage : Page
    {
        private readonly Quiz quiz;
        private readonly TimeSpan timeTaken;

        public EndQuizPage(Quiz quiz, TimeSpan timeTaken)
        {
            this.InitializeComponent();
            quiz = quiz;
            timeTaken = timeTaken;

            DisplayResults();
        }

        /* Demo implementation with dummy data
        private readonly Quiz? _quiz;
        private readonly TimeSpan? _timeTaken;

        public EndQuizPage()
        {
            this.InitializeComponent();
            ShowDemoQuizResults();
        }

        private void ShowDemoQuizResults()
        {
            // Create a dummy quiz with good results
            int correctAnswers = 8;
            int totalQuestions = 10;
            double scorePercentage = ((double)correctAnswers / totalQuestions) * 100;
            // Display score
            ScoreTextBlock.Text = $"{correctAnswers}/{totalQuestions} ({scorePercentage:F1}%)";
            // Display a sample time (3 minutes and 45 seconds)
            TimeTextBlock.Text = "3m 45s";

            // Show a success message
            FeedbackTextBlock.Text = "Great job! You've passed the quiz!";
            FeedbackTextBlock.Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Green);
        }
        */

        private void DisplayResults()
        {
            // Calculate score percentage
            double scorePercentage = ((double)quiz.GetNumberOfCorrectAnswers() / quiz.GetNumberOfAnswersGiven()) * 100;

            // Display score
            ScoreTextBlock.Text = $"{quiz.GetNumberOfCorrectAnswers()}/{quiz.GetNumberOfAnswersGiven()} ({scorePercentage:F1}%)";

            // Display time taken
            TimeTextBlock.Text = $"{timeTaken.Minutes}m {timeTaken.Seconds}s";

            // Set feedback message based on score
            if (scorePercentage >= quiz.GetPassingThreshold())
            {
                FeedbackTextBlock.Text = "Great job! You've passed the quiz!";
                FeedbackTextBlock.Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Green);
            }
            else
            {
                FeedbackTextBlock.Text = "Keep practicing! You can do better next time.";
                FeedbackTextBlock.Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Red);
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Frame.CanGoBack)
                {
                    Frame.GoBack();
                }
                else
                {
                    // If we can't go back, navigate to the quiz page
                    Frame.Navigate(typeof(RoadmapMainPage));
                }
            }
            catch (Exception)
            {
                // If navigation fails, try to navigate to the quiz page
                Frame.Navigate(typeof(RoadmapMainPage));
            }
        }
    }
}