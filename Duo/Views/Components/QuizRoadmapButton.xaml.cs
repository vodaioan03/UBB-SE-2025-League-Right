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
    public sealed partial class QuizRoadmapButton : UserControl
    {
        public event RoutedEventHandler Click;


        public static readonly DependencyProperty QuizProperty =
            DependencyProperty.Register(nameof(Quiz), typeof(Quiz), typeof(QuizRoadmapButton), new PropertyMetadata(null));


        public string QuizId { get; set;  }

        public QuizRoadmapButton()
        {
            this.InitializeComponent();
        }

        public Quiz Quiz
        {
            get => (Quiz)GetValue(QuizProperty);
            set { 
                SetValue(QuizProperty, value);
                QuizId = "Quiz " + value.Id.ToString();

                Debug.WriteLine($"Quiz set: {QuizId}, ID: {value.Id}");

            }
        }
    }
}
