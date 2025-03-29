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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Duo.Views.Components
{
    public sealed partial class SectionRoadmapView : UserControl
    {
        public event RoutedEventHandler Click;

        public static readonly DependencyProperty QuizzesProperty =
            DependencyProperty.Register(nameof(Quizzes), typeof(ObservableCollection<Quiz>), typeof(SectionRoadmapView), new PropertyMetadata(new ObservableCollection<Quiz>()));

        public static readonly DependencyProperty ExamProperty =
            DependencyProperty.Register(nameof(Exam), typeof(Exam), typeof(SectionRoadmapView), new PropertyMetadata(null));

        private static readonly SolidColorBrush TransparentBrush = new SolidColorBrush(Microsoft.UI.Colors.Transparent);
        private static readonly SolidColorBrush SelectedBrush = new SolidColorBrush(Microsoft.UI.Colors.Coral);

        public SectionRoadmapView()
        {
            this.InitializeComponent();
        }

        public ObservableCollection<Quiz> Quizzes
        {
            get => (ObservableCollection<Quiz>)GetValue(QuizzesProperty);
            set => SetValue(QuizzesProperty, value);
        }

        public Exam Exam
        {
            get => (Exam)GetValue(ExamProperty);
            set => SetValue(ExamProperty, value);
        }

        private void Quiz_Click(object sender, RoutedEventArgs e)
        {
            var clickedButton = sender as Button;
            // HANDLE QUIZ CLICK
        }

        private void Exam_Click(object sender, RoutedEventArgs e)
        {
            // HANDLE EXAM CLICK
        }
    }
}
