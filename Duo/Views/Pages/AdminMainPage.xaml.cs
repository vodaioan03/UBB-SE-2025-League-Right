using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Duo.Models.Quizzes;
using Duo.Models.Exercises;
using Duo.Views.Components.Modals;

namespace Duo.Views.Pages
{
    public sealed partial class AdminMainPage : Page
    {
        public AdminMainPage()
        {
            this.InitializeComponent();

            // Subscribe to modal events
            CreateSectionModal.SectionCreated += CreateSectionModal_SectionCreated;
            CreateSectionModal.ModalClosed += CreateSectionModal_ModalClosed;
            CreateExamModal.ExamCreated += CreateExamModal_ExamCreated;
            CreateExamModal.ModalClosed += CreateExamModal_ModalClosed;
            CreateQuizModal.QuizCreated += CreateQuizModal_QuizCreated;
            CreateQuizModal.ModalClosed += CreateQuizModal_ModalClosed;


            ManageSectionsCard.AddButtonClicked += CreateSection_Click;
            ManageExercisesCard.AddButtonClicked += CreateExercise_Click;
            ManageQuizesCard.AddButtonClicked += CreateQuiz_Click;
            ManageExamsCard.AddButtonClicked += CreateExam_Click;

            ManageSectionsCard.ManageButtonClicked += OpenManageSectionsPage_Click;
            ManageExercisesCard.ManageButtonClicked += OpenManageExercisesPage_Click;
            ManageQuizesCard.ManageButtonClicked += OpenManageQuizesPage_Click;
            ManageExamsCard.ManageButtonClicked += OpenManageExamsPage_Click;

        }
        private void CreateExam_Click(object sender, EventArgs e)
        {
            ExamModalOverlay.Visibility = Visibility.Visible;
        }

        private void CreateExercise_Click(object sender, EventArgs e)
        {
            Frame.Navigate(typeof(CreateExercisePage));
        }

        private void CreateSection_Click(object sender, EventArgs e)
        {
            //SectionModalOverlay.Visibility = Visibility.Visible;

            Frame.Navigate(typeof(CreateSectionPage));
        }

        private void CreateSectionModal_SectionCreated(object sender, SectionCreatedEventArgs e)
        {
            var dialog = new ContentDialog
            {
                Title = "Success",
                Content = $"Section '{e.Subject}' created successfully!",
                CloseButtonText = "OK",
                XamlRoot = this.XamlRoot
            };
            dialog.ShowAsync();
        }

        private void CreateExam_Click(object sender, RoutedEventArgs e)
        {
            ExamModalOverlay.Visibility = Visibility.Visible;
        }

        private void CreateExamModal_ExamCreated(object sender, ExamCreatedEventArgs e)
        {
            var exam = e.Exam;
            ExamModalOverlay.Visibility = Visibility.Collapsed;
        }

        private void CreateExamModal_ModalClosed(object sender, EventArgs e)
        {
            ExamModalOverlay.Visibility = Visibility.Collapsed;
        }

        public void OpenCreateExamPage_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(CreateExercisePage));
        }

        private void CreateSectionModal_ModalClosed(object sender, EventArgs e)
        {
            SectionModalOverlay.Visibility = Visibility.Collapsed;
        }


        private void CreateQuiz_Click(object sender, EventArgs e)
        {
            //QuizModalOverlay.Visibility = Visibility.Visible;
            Frame.Navigate(typeof(CreateQuizPage));
        }

        private async void CreateQuizModal_QuizCreated(object sender, QuizCreatedEventArgs e)
        {
            var dialog = new ContentDialog
            {
                Title = "Success",
                Content = $"Quiz '{e.Title}' created successfully with {e.Quiz.ExerciseList.Count} exercises!",
                CloseButtonText = "OK",
                XamlRoot = this.XamlRoot
            };
            await dialog.ShowAsync();
        }

        private void CreateQuizModal_ModalClosed(object sender, EventArgs e)
        {
            QuizModalOverlay.Visibility = Visibility.Collapsed;
        }

        public void OpenManageExercisesPage_Click(object sender, EventArgs e)
        {
            Frame.Navigate(typeof(ManageExercisesPage));
        }

        public void OpenManageQuizesPage_Click(object sender, EventArgs e)
        {
            Frame.Navigate(typeof(ManageQuizesPage));
        }
        public void OpenManageSectionsPage_Click(object sender, EventArgs e)
        {
            Frame.Navigate(typeof(ManageSectionsPage));
        }
        public void OpenManageExamsPage_Click(object sender, EventArgs e)
        {

        }

    }
}