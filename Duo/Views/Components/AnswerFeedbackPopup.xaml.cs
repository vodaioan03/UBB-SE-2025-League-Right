using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI;

namespace Duo.Views.Components
{
    public sealed partial class AnswerFeedbackPopup : ContentDialog
    {
        private readonly SolidColorBrush greenBrush = new SolidColorBrush(Colors.Green);
        private readonly SolidColorBrush redBrush = new SolidColorBrush(Colors.Red);

        public AnswerFeedbackPopup()
        {
            this.InitializeComponent();
        }

        public void ShowCorrectAnswer(string correctAnswer)
        {
            FeedbackIcon.Glyph = "\uE73E"; // Checkmark icon
            FeedbackIcon.Foreground = greenBrush;
            FeedbackMessage.Text = "Correct! Well done!";
            FeedbackMessage.Foreground = greenBrush;
            CorrectAnswerText.Text = correctAnswer;
            CloseButton.Background = greenBrush;
            CloseButton.Foreground = new SolidColorBrush(Colors.White);
        }

        public void ShowWrongAnswer(string correctAnswer)
        {
            FeedbackIcon.Glyph = "\uE783"; // X icon
            FeedbackIcon.Foreground = redBrush;
            FeedbackMessage.Text = "Incorrect. Keep trying!";
            FeedbackMessage.Foreground = redBrush;
            CorrectAnswerText.Text = correctAnswer;
            CloseButton.Background = redBrush;
            CloseButton.Foreground = new SolidColorBrush(Colors.White);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }
    }
}