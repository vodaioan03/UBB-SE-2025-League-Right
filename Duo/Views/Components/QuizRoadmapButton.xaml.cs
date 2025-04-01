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
    public sealed partial class QuizRoadmapButton : UserControl
    {
        public event RoutedEventHandler ButtonClick;

        public static readonly DependencyProperty QuizIdProperty =
           DependencyProperty.Register(
               nameof(QuizId),
               typeof(int),
               typeof(QuizRoadmapButton),
               new PropertyMetadata(0, OnQuizIdChanged));

        public static readonly DependencyProperty IsExamProperty =
            DependencyProperty.Register(
                nameof(IsExam),
                typeof(bool),
                typeof(QuizRoadmapButton),
                new PropertyMetadata(false));

        public int QuizId
        {
            get => (int)GetValue(QuizIdProperty);
            set => SetValue(QuizIdProperty, value);
        }

        public bool IsExam
        {
            get => (bool)GetValue(IsExamProperty);
            set => SetValue(IsExamProperty, value);
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(
                nameof(Command),
                typeof(ICommand),
                typeof(QuizRoadmapButton),
                new PropertyMetadata(null));

        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register(
                nameof(CommandParameter),
                typeof(object),
                typeof(QuizRoadmapButton),
                new PropertyMetadata(null));

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }



        public QuizRoadmapButton()
        {
            this.InitializeComponent();
        }

        private static void OnQuizIdChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is QuizRoadmapButton button)
            {
                int newQuizId = (int)e.NewValue;
                Debug.WriteLine($"Quiz ID changed: {newQuizId}");
                button.UpdateQuizId(newQuizId);
            }
        }

        private void UpdateQuizId(int newQuizId)
        {
            TextBlock dynamicTextBlock;

            Button circularButton = new Button
            {
                Width = 50,
                Height = 50,
                BorderThickness = new Thickness(2),
                BorderBrush = new SolidColorBrush(Microsoft.UI.Colors.Black),
                CornerRadius = new Microsoft.UI.Xaml.CornerRadius(50),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            circularButton.Click += OnButtonClick;  // Link button click to OnButtonClick handler

            // Set the background and dynamic text depending on whether it's an exam or quiz
            if (IsExam)
            {
                circularButton.Background = new SolidColorBrush(Microsoft.UI.Colors.Red);
                dynamicTextBlock = new TextBlock
                {
                    Text = $"Exam {newQuizId}",
                    FontSize = 12,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(10)
                };
            }
            else
            {
                dynamicTextBlock = new TextBlock
                {
                    Text = $"Quiz {newQuizId}",
                    FontSize = 12,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(10)
                };
            }

            // Bind the command and command parameter to the button if they are set
            if (Command != null)
            {
                circularButton.Tag = CommandParameter; 
            }

            // Add elements to the StackPanel content
            if (this.Content is StackPanel stackPanel)
            {
                stackPanel.Children.Add(circularButton);
                stackPanel.Children.Add(dynamicTextBlock);
            }
        }

        private void OnButtonClick(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine($"Button clicked: {QuizId}");
            Debug.WriteLine("");
            Debug.WriteLine($"Click Command: {Command}");
            Debug.WriteLine($"Button clicked: {CommandParameter}");
            Debug.WriteLine("");
            Debug.WriteLine("");
            if (Command?.CanExecute(CommandParameter) == true)
            {
                Command.Execute(CommandParameter);
            }
            else
            {
                ButtonClick?.Invoke(this, e);  // Fallback to the event if the command is not set or cannot execute
            }
        }

    }
}
