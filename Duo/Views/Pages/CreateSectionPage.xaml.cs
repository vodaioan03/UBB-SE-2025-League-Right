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
        }
        public event EventHandler<SectionCreatedEventArgs> SectionCreated;
        public event EventHandler ModalClosed;

        private readonly List<Quiz> _availableQuizzes;
        private readonly List<Exam> _availableExams;
        public ObservableCollection<Quiz> UnassignedQuizzes { get; private set; }
        public ObservableCollection<Exam> SelectedExam { get; private set; }

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
