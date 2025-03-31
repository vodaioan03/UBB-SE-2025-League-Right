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
                if (part.Contains("{}"))
                    QuestionElements.Add(new TextBox
                    {
                        Width = 100,
                        Margin = new Thickness(5)
                    });
                else
                    QuestionElements.Add(new TextBlock
                    {
                        Text = part,
                        Margin = new Thickness(5)
                    });
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
