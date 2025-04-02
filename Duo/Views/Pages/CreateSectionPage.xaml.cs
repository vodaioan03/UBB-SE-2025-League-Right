using Duo.Models.Exercises;
using Duo.Models.Quizzes;
using Duo.Views.Components.Modals;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Duo.Views.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CreateSectionPage : Page
    {
        public CreateSectionPage()
        {
            this.InitializeComponent();
            ViewModel.ShowListViewModalQuizes += ViewModel_openSelectQuizes;
            ViewModel.ShowListViewModalExams += ViewModel_openSelectExams;

            ViewModel.ShowErrorMessageRequested += ViewModel_ShowErrorMessageRequested;
            ViewModel.RequestGoBack += ViewModel_RequestGoBack;
        }
        private async void ViewModel_ShowErrorMessageRequested(object sender, (string Title, string Message) e)
        {
            await ShowErrorMessage(e.Title, e.Message);
        }
        public void ViewModel_RequestGoBack(object sender, EventArgs e)
        {
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
            }
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

        private async void ViewModel_openSelectExams(List<Exam> exams)
        {
            var dialog = new ContentDialog
            {
                Title = "Select Exam",
                CloseButtonText = "Cancel",
                DefaultButton = ContentDialogButton.Primary,
                XamlRoot = this.XamlRoot
            };

            var listView = new ListView
            {
                ItemsSource = exams,
                SelectionMode = ListViewSelectionMode.Single,
                MaxHeight = 300,
                ItemTemplate = (DataTemplate)Resources["QuizSelectionItemTemplate"]
            };

            dialog.Content = listView;
            dialog.PrimaryButtonText = "Add";
            dialog.IsPrimaryButtonEnabled = false;

            listView.SelectionChanged += (s, args) =>
            {
                dialog.IsPrimaryButtonEnabled = listView.SelectedItem != null;
            };

            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary && listView.SelectedItem is Exam selectedExam)
                ViewModel.AddExam(selectedExam);
        }

        private async void ViewModel_openSelectQuizes(List<Quiz> quizzes)
        {
            var dialog = new ContentDialog
            {
                Title = "Select Quiz",
                CloseButtonText = "Cancel",
                DefaultButton = ContentDialogButton.Primary,
                XamlRoot = this.XamlRoot
            };

            var listView = new ListView
            {
                ItemsSource = quizzes,
                SelectionMode = ListViewSelectionMode.Single,
                MaxHeight = 300,
                ItemTemplate = (DataTemplate)Resources["QuizSelectionItemTemplate"]
            };

            dialog.Content = listView;
            dialog.PrimaryButtonText = "Add";
            dialog.IsPrimaryButtonEnabled = false;

            listView.SelectionChanged += (s, args) =>
            {
                dialog.IsPrimaryButtonEnabled = listView.SelectedItem != null;
            };

            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary && listView.SelectedItem is Quiz selectedQuiz)
                ViewModel.AddQuiz(selectedQuiz);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void AddExamButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void AddQuizButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void RemoveQuiz_Click(object sender, RoutedEventArgs e)
        {
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
            }
        }
    }
}
