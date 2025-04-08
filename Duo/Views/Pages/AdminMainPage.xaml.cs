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
            ManageSectionsCard.AddButtonClicked += CreateSection_Click;
            ManageExercisesCard.AddButtonClicked += CreateExercise_Click;
            ManageQuizesCard.AddButtonClicked += CreateQuiz_Click;
            ManageExamsCard.AddButtonClicked += CreateExam_Click;

            ManageSectionsCard.ManageButtonClicked += OpenManageSectionsPage_Click;
            ManageExercisesCard.ManageButtonClicked += OpenManageExercisesPage_Click;
            ManageQuizesCard.ManageButtonClicked += OpenManageQuizesPage_Click;
            ManageExamsCard.ManageButtonClicked += OpenManageExamsPage_Click;
        }

        private void CreateSection_Click(object sender, EventArgs e)
        {
            Frame.Navigate(typeof(CreateSectionPage));
        }
        private void CreateQuiz_Click(object sender, EventArgs e)
        {
            Frame.Navigate(typeof(CreateQuizPage));
        }

        private void CreateExam_Click(object sender, EventArgs e)
        {
            Frame.Navigate(typeof(CreateExamPage));
        }

        private void CreateExercise_Click(object sender, EventArgs e)
        {
            Frame.Navigate(typeof(CreateExercisePage));
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
            Frame.Navigate(typeof(ManageExamsPage));
        }
    }
}