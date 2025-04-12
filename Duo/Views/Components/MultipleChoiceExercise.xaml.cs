using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Duo.Models.Exercises;
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
using Windows.UI;
using Windows.UI.ViewManagement;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.
namespace Duo.Views.Components
{
    public sealed partial class MultipleChoiceExercise : UserControl
    {
        public event EventHandler<MultipleChoiceExerciseEventArgs> OnSendClicked;

        public event RoutedEventHandler Click;
        private Button selectedLeftButton;
        private Button selectedRightButton;

        public static readonly DependencyProperty QuestionProperty =
           DependencyProperty.Register(nameof(Question), typeof(string), typeof(AssociationExercise), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty AnswersProperty =
            DependencyProperty.Register(nameof(Answers), typeof(ObservableCollection<string>), typeof(AssociationExercise), new PropertyMetadata(new ObservableCollection<string>()));

        private static readonly SolidColorBrush TransparentBrush = new SolidColorBrush(Microsoft.UI.Colors.Transparent);

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
        public Color GetSystemAccentColor()
        {
            var uiSettings = new UISettings();
            return uiSettings.GetColorValue(UIColorType.Accent);
        }
        public Color GetSystemAccentTextColor()
        {
            var uiSettings = new UISettings();
            return uiSettings.GetColorValue(UIColorType.Complement);
        }

        private void SetDefaultButtonStyles(Button clickedButton)
        {
            clickedButton.Background = TransparentBrush;
        }

        private void Option_Click(object sender, RoutedEventArgs e)
        {
            var clickedButton = sender as Button;
  
            if (selectedButtons.Contains(clickedButton))
            {
                SetDefaultButtonStyles(clickedButton);
                selectedButtons.Remove(clickedButton);
            }
            else
            {
                foreach (var selectedButton in selectedButtons)
                {
                    SetDefaultButtonStyles(selectedButton);
                }
                selectedButtons.Clear();
                clickedButton.Background = new SolidColorBrush(GetSystemAccentColor());
                selectedButtons.Add(clickedButton);
            }
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            List<string> userChoices = selectedButtons.Select(b => b.Content.ToString()).ToList();

            OnSendClicked?.Invoke(this, new MultipleChoiceExerciseEventArgs(userChoices));
        }

        public class MultipleChoiceExerciseEventArgs : EventArgs
        {
            public List<string> ContentPairs { get; }

            public MultipleChoiceExerciseEventArgs(List<string> userChoices)
            {
                ContentPairs = userChoices;
            }
        }
    }
}
