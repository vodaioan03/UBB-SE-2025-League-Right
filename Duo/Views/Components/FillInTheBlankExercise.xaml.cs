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
using System.Text.RegularExpressions;
using static Duo.Views.Components.AssociationExercise;
using Windows.UI;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Duo.Views.Components
{
    public sealed partial class FillInTheBlanksExercise : UserControl
    {
        public event EventHandler<FillInTheBlanksExerciseEventArgs> OnSendClicked;

        public event RoutedEventHandler Click;
        private Button _selectedLeftButton;
        private Button _selectedRightButton;

        public static readonly DependencyProperty QuestionProperty =
           DependencyProperty.Register(nameof(Question), typeof(string), typeof(AssociationExercise), new PropertyMetadata(""));

        private static readonly SolidColorBrush TransparentBrush = new SolidColorBrush(Microsoft.UI.Colors.Transparent);
        private static readonly SolidColorBrush SelectedBrush = new SolidColorBrush(Microsoft.UI.Colors.Coral);

        public ObservableCollection<UIElement> QuestionElements { get; set; } = new ObservableCollection<UIElement>();


        public FillInTheBlanksExercise()
        {
            this.InitializeComponent();
        }

        public string Question
        {
            get => (string)GetValue(QuestionProperty);
            set {
                SetValue(QuestionProperty, value);
                ParseQuestion(value);
            }
        }

        private void ParseQuestion(string question)
        {
            QuestionElements.Clear();
            var parts = Regex.Split(question, @"({})");

            foreach (var part in parts)
            {
                if (part.Contains("{}"))
                {
                    var textBox = new TextBox
                    {
                        Width = 150,
                        Height = 40,
                        FontSize = 16,
                        PlaceholderText = "Type here...",
                        BorderThickness = new Thickness(1),
                        BorderBrush = new SolidColorBrush(Color.FromArgb(255, 200, 200, 200)),
                        Background = new SolidColorBrush(Color.FromArgb(255, 245, 245, 245)),
                        Padding = new Thickness(8, 4, 8, 4),
                        Margin = new Thickness(4),
                        CornerRadius = new CornerRadius(4),
                        SelectionHighlightColor = new SolidColorBrush(Color.FromArgb(255, 0, 120, 215)),
                        SelectionHighlightColorWhenNotFocused = new SolidColorBrush(Color.FromArgb(255, 0, 120, 215)),
                        VerticalContentAlignment = VerticalAlignment.Center
                    };

                    // Add focus visual style
                    textBox.GotFocus += (s, e) =>
                    {
                        textBox.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 0, 120, 215));
                        textBox.Background = new SolidColorBrush(Microsoft.UI.Colors.White);
                    };

                    textBox.LostFocus += (s, e) =>
                    {
                        textBox.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 200, 200, 200));
                        textBox.Background = new SolidColorBrush(Color.FromArgb(255, 245, 245, 245));
                    };

                    QuestionElements.Add(textBox);
                }
                else
                {
                    var textBlock = new TextBlock
                    {
                        Text = part,
                        FontSize = 16,
                        Foreground = new SolidColorBrush(Color.FromArgb(255, 51, 51, 51)),
                        VerticalAlignment = VerticalAlignment.Center,
                        Margin = new Thickness(4)
                    };
                    QuestionElements.Add(textBlock);
                }
            }
        }


        private void Send_Click(object sender, RoutedEventArgs e)
        {
            List<string> inputValues = QuestionElements
                .OfType<TextBox>()      
                .Select(textBox => textBox.Text) 
                .ToList();

            OnSendClicked?.Invoke(this, new FillInTheBlanksExerciseEventArgs(inputValues));
        }

        public class FillInTheBlanksExerciseEventArgs : EventArgs
        {
            public List<string> ContentPairs { get; }

            public FillInTheBlanksExerciseEventArgs(List<string> inputValues)
            {
                ContentPairs = inputValues;
            }
        }
    }
}
