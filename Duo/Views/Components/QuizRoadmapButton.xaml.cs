using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using Duo.Models.Exercises;
using Duo.Models.Quizzes;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using Windows.Foundation;
using Windows.Foundation.Collections;
using static Duo.ViewModels.Roadmap.RoadmapButtonTemplate;

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
                new PropertyMetadata(false, OnIsExamChanged));

        public static readonly DependencyProperty QuizStatusProperty =
            DependencyProperty.Register(
            nameof(QuizStatus),
            typeof(QUIZ_STATUS),
            typeof(QuizRoadmapButton),
            new PropertyMetadata(1));

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

        public QUIZ_STATUS QuizStatus
        {
            get => (QUIZ_STATUS)GetValue(QuizStatusProperty);
            set => SetValue(QuizStatusProperty, value);
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
            this.Loaded += QuizRoadmapButton_Loaded;
        }

        private void QuizRoadmapButton_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateQuizId(QuizId);
            UpdateExamStatus(IsExam);
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

        private static void OnIsExamChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is QuizRoadmapButton button)
            {
                bool isExam = (bool)e.NewValue;
                button.UpdateExamStatus(isExam);
            }
        }

        private void UpdateQuizId(int newQuizId)
        {
            if (ButtonNumber != null)
            {
                ButtonNumber.Text = newQuizId.ToString();
            }

            if (ButtonLabel != null)
            {
                ButtonLabel.Text = IsExam ? $"Exam {newQuizId}" : $"Quiz {newQuizId}";
            }

            // Bind command parameter if set
            if (Command != null && CircularButton != null)
            {
                CircularButton.Tag = CommandParameter;
            }
        }

        private void UpdateExamStatus(bool isExam)
        {
            if (CircularButton != null)
            {
                // Apply special styling for exams
                if (isExam)
                {
                    switch (QuizStatus)
                    {
                        case QUIZ_STATUS.LOCKED:
                            CircularButton.Background = new SolidColorBrush(Microsoft.UI.Colors.DarkGray);
                            break;
                        case QUIZ_STATUS.INCOMPLETE:
                            CircularButton.Background = new SolidColorBrush(Microsoft.UI.Colors.Olive);
                            break;
                        case QUIZ_STATUS.COMPLETED:
                            CircularButton.Background = new SolidColorBrush(Microsoft.UI.Colors.DarkRed);
                            break;
                        default:
                            CircularButton.Background = new SolidColorBrush(Microsoft.UI.Colors.DarkGray);
                            break;
                    }
                    CircularButton.Foreground = new SolidColorBrush(Microsoft.UI.Colors.White);
                }
                else
                {
                    switch (QuizStatus)
                    {
                        case QUIZ_STATUS.LOCKED:
                            CircularButton.Background = new SolidColorBrush(Microsoft.UI.Colors.Gray);
                            break;
                        case QUIZ_STATUS.INCOMPLETE:
                            CircularButton.Background = new SolidColorBrush(Microsoft.UI.Colors.Olive);
                            break;
                        case QUIZ_STATUS.COMPLETED:
                            CircularButton.Background = new SolidColorBrush(Microsoft.UI.Colors.Green);
                            break;
                        default:
                            CircularButton.ClearValue(Button.BackgroundProperty);
                            break;
                    }
                    CircularButton.Foreground = new SolidColorBrush(Microsoft.UI.Colors.White);
                }
            }

            // Update label text
            if (ButtonLabel != null && ButtonNumber != null)
            {
                ButtonLabel.Text = isExam ? $"Exam {QuizId}" : $"Quiz {QuizId}";
            }
        }

        private void OnButtonClick(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine($"Button clicked: {QuizId}");
            Debug.WriteLine($"Is Exam: {IsExam}");
            Debug.WriteLine($"Quiz Status: {QuizStatus}");

            if (QuizStatus == QUIZ_STATUS.LOCKED)
            {
                return;
            }

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
