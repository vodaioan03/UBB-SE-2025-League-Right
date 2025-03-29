using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Duo.Views.Components.Modals;
using Duo.Models.Exercises;
using Duo.Models.Quizzes;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Duo.Views.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AdminMainPage : Page
    {
        public AdminMainPage()
        {
            this.InitializeComponent();

            // Subscribe to modal events
            CreateQuizModal.QuizCreated += CreateQuizModal_QuizCreated;
            CreateQuizModal.ModalClosed += CreateQuizModal_ModalClosed;
        }

        private void CreateQuiz_Click(object sender, RoutedEventArgs e)
        {
            QuizModalOverlay.Visibility = Visibility.Visible;
        }

        private async void CreateQuizModal_QuizCreated(object sender, QuizCreatedEventArgs e)
        {
            // TODO: Handle the new quiz creation
            // You can add your logic here to save the quiz to your data store
            var dialog = new ContentDialog
            {
                Title = "Success",
                Content = $"Quiz '{e.Title}' created successfully with {e.Quiz.GetExerciseCount()} exercises!",
                CloseButtonText = "OK",
                XamlRoot = this.XamlRoot
            };
            await dialog.ShowAsync();
        }

        private void CreateQuizModal_ModalClosed(object sender, EventArgs e)
        {
            QuizModalOverlay.Visibility = Visibility.Collapsed;
        }

        public void OpenCreateExamPage_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(CreateExercisePage));
        }
    }
}
