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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Duo.Views.Components
{
    public sealed partial class MultipleChoiceExercise : UserControl
    {
        public event RoutedEventHandler Click;
        private Button _selectedLeftButton;
        private Button _selectedRightButton;

        public static readonly DependencyProperty QuestionProperty =
           DependencyProperty.Register(nameof(Question), typeof(string), typeof(AssociationExercise), new PropertyMetadata(""));

        public static readonly DependencyProperty AnswersProperty =
            DependencyProperty.Register(nameof(Answers), typeof(ObservableCollection<string>), typeof(AssociationExercise), new PropertyMetadata(new ObservableCollection<string>()));


        private static readonly SolidColorBrush TransparentBrush = new SolidColorBrush(Microsoft.UI.Colors.Transparent);
        private static readonly SolidColorBrush SelectedBrush = new SolidColorBrush(Microsoft.UI.Colors.Coral);

        private List<Button> selectedButtons = new List<Button>();


        public MultipleChoiceExercise()
        {
            this.InitializeComponent();
        }

        public string Question
        {
            get => (string)GetValue(QuestionProperty);
            set => SetValue(QuestionProperty, value);
        }


        public ObservableCollection<MultipleChoiceAnswerModel> Answers
        {
            get => (ObservableCollection<MultipleChoiceAnswerModel>)GetValue(AnswersProperty);
            set => SetValue(AnswersProperty, value);
        }

        private void Option_Click(object sender, RoutedEventArgs e)
        {
            var clickedButton = sender as Button;

            if (selectedButtons.Contains(clickedButton))
            {
                clickedButton.Background = TransparentBrush;
                selectedButtons.Remove(clickedButton);
            }
            else
            {
                clickedButton.Background = SelectedBrush;
                selectedButtons.Add(clickedButton);
            }
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            List<string> userChoices = selectedButtons.Select(b => b.Content.ToString()).ToList();

            // IMPLEMENT SEND RESULTS
        }
    }
}
