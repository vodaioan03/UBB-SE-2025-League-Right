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
using Duo.Models.Quizzes;
using Duo.Models.Exercises;
using Duo.Views.Components.Modals;

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
            CreateExamModal.ExamCreated += CreateExamModal_ExamCreated;
            CreateExamModal.ModalClosed += CreateExamModal_ModalClosed;
        }

        private void CreateExam_Click(object sender, RoutedEventArgs e)
        {
            ModalOverlay.Visibility = Visibility.Visible;
        }

        private void CreateExamModal_ExamCreated(object sender, ExamCreatedEventArgs e)
        {
            // Handle the created exam
            var exam = e.Exam;

            // TODO: Save the exam to your data source

            // Hide the modal
            ModalOverlay.Visibility = Visibility.Collapsed;
        }

        private void CreateExamModal_ModalClosed(object sender, EventArgs e)
        {
            ModalOverlay.Visibility = Visibility.Collapsed;
        }

        public void OpenCreateExamPage_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(CreateExercisePage));
        }
    }
}
