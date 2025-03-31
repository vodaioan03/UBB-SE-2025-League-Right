using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI;

namespace Duo.Views.Components
{
    public sealed partial class AnswerFeedbackPopup : ContentDialog
    {
        private readonly SolidColorBrush GreenBrush = new SolidColorBrush(Colors.Green);
        private readonly SolidColorBrush RedBrush = new SolidColorBrush(Colors.Red);

        public AnswerFeedbackPopup()
        {
            this.InitializeComponent();
        }

        public void ShowCorrectAnswer(string correctAnswer)
        {
            FeedbackIcon.Glyph = "\uE73E"; // Checkmark icon
            FeedbackIcon.Foreground = GreenBrush;
            FeedbackMessage.Text = "Correct! Well done!";
            FeedbackMessage.Foreground = GreenBrush;
            CorrectAnswerText.Text = correctAnswer;
            CloseButton.Background = GreenBrush;
            CloseButton.Foreground = new SolidColorBrush(Colors.White);
        }

        public void ShowWrongAnswer(string correctAnswer)
        {
            FeedbackIcon.Glyph = "\uE783"; // X icon
            FeedbackIcon.Foreground = RedBrush;
            FeedbackMessage.Text = "Incorrect. Keep trying!";
            FeedbackMessage.Foreground = RedBrush;
            CorrectAnswerText.Text = correctAnswer;
            CloseButton.Background = RedBrush;
            CloseButton.Foreground = new SolidColorBrush(Colors.White);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }
    }
} 