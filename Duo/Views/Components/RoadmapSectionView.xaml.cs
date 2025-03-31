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
using Microsoft.UI.Xaml.Documents;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Microsoft.UI.Xaml.Shapes;
using Duo.Models.Exercises;
using Duo.Models.Quizzes;
using System.Windows.Input;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Duo.Views.Components
{
    public sealed partial class RoadmapSectionView : UserControl
    {
        public event RoutedEventHandler Click;

        //public static readonly DependencyProperty QuizzesProperty =
        //    DependencyProperty.Register(nameof(Quizzes), typeof(ObservableCollection<Quiz>), typeof(RoadmapSectionView), new PropertyMetadata(new ObservableCollection<Quiz>()));

        //public static readonly DependencyProperty ExamProperty =
        //    DependencyProperty.Register(nameof(Exam), typeof(Exam), typeof(RoadmapSectionView), new PropertyMetadata(null));

        //public static readonly DependencyProperty SectionProperty =
        //    DependencyProperty.Register(nameof(Section), typeof(Exam), typeof(RoadmapSectionView), new PropertyMetadata(null));

        //public static readonly DependencyProperty OpenQuizPreviewCommandProperty =
        //DependencyProperty.Register(
        //    nameof(OpenQuizPreviewCommand),
        //    typeof(ICommand),
        //    typeof(RoadmapSectionView),
        //    new PropertyMetadata(null));

        //public ICommand OpenQuizPreviewCommand
        //{
        //    get => (ICommand)GetValue(OpenQuizPreviewCommandProperty);
        //    set => SetValue(OpenQuizPreviewCommandProperty, value);
        //}

        public RoadmapSectionView()
        {
            this.InitializeComponent();
        }

        //public ObservableCollection<Quiz> Quizzes
        //{
        //    get => (ObservableCollection<Quiz>)GetValue(QuizzesProperty);
        //    set => SetValue(QuizzesProperty, value);
        //}

        //public Exam Exam
        //{
        //    get => (Exam)GetValue(ExamProperty);
        //    set
        //    {
        //        SetValue(ExamProperty, value);
        //    }
        //}
        //public string Section
        //{
        //    get => (string)GetValue(SectionProperty);
        //    set => SetValue(SectionProperty, value);
        //}

        private void Quiz_Click(object sender, RoutedEventArgs e)
        {
            var clickedButton = sender as Button;
            // HANDLE QUIZ CLICK
        }

        private void Exam_Click(object sender, RoutedEventArgs e)
        {
            // HANDLE EXAM CLICK/
        }

        public void OnQuizRoadmapButtonClick(object sender, RoutedEventArgs e)
        {
            if (sender is QuizRoadmapButton button)
            {
                Debug.WriteLine($"Quiz with ID {button.QuizId} clicked!");

                // You can perform any additional logic here based on the button that was clicked
            }
        }

        public void OnExamRoadmapButtonClick(object sender, RoutedEventArgs e)
        {
            if (sender is QuizRoadmapButton button)
            {
                Debug.WriteLine($"Exam with ID {button.QuizId} clicked!");

                // You can perform any additional logic here based on the button that was clicked
            }
        }

    }
}
