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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Duo.Views.Components
{
    public sealed partial class AssociationExercise : UserControl
    {
        public event RoutedEventHandler Click;
        private Button _selectedLeftButton;
        private Button _selectedRightButton;

        public static readonly DependencyProperty QuestionProperty =
           DependencyProperty.Register(nameof(Question), typeof(string), typeof(AssociationExercise), new PropertyMetadata(""));

        public static readonly DependencyProperty FirstAnswersListProperty =
            DependencyProperty.Register(nameof(FirstAnswersList), typeof(ObservableCollection<string>), typeof(AssociationExercise), new PropertyMetadata(new ObservableCollection<string>()));

        public static readonly DependencyProperty SecondAnswersListProperty =
            DependencyProperty.Register(nameof(SecondAnswersList), typeof(ObservableCollection<string>), typeof(AssociationExercise), new PropertyMetadata(new ObservableCollection<string>()));

        private static readonly SolidColorBrush TransparentBrush = new SolidColorBrush(Microsoft.UI.Colors.Transparent);
        private static readonly SolidColorBrush SelectedBrush = new SolidColorBrush(Microsoft.UI.Colors.Coral);
        private static readonly SolidColorBrush MappedBrush = new SolidColorBrush(Microsoft.UI.Colors.Gray);

        private List<Tuple<Button, Button>> pairs = new List<Tuple<Button, Button>>();


        public AssociationExercise()
        {
            this.InitializeComponent();
        }

        public string Question
        {
            get => (string)GetValue(QuestionProperty);
            set => SetValue(QuestionProperty, value);
        }

        public ObservableCollection<string> FirstAnswersList
        {
            get
            {
                var list = (ObservableCollection<string>)GetValue(FirstAnswersListProperty);
                return new ObservableCollection<string>(list.OrderBy(_ => Guid.NewGuid()));
            }
            set => SetValue(FirstAnswersListProperty, value);
        }

        public ObservableCollection<string> SecondAnswersList
        {
            get => (ObservableCollection<string>)GetValue(SecondAnswersListProperty);
            set => SetValue(SecondAnswersListProperty, value);
        }

        private void HandleOptionClick(ref Button selectedButton, Button clickedButton)
        {
            if (selectedButton == clickedButton)
            {
                selectedButton.Background = TransparentBrush;
                selectedButton = null;
            }
            else if (selectedButton != clickedButton && selectedButton != null)
            {
                selectedButton.Background = TransparentBrush;

                selectedButton = clickedButton;
                selectedButton.Background = SelectedBrush;
            }
            else
            {
                selectedButton = clickedButton;
                selectedButton.Background = SelectedBrush;
            }
        }

        private void DestroyExistingConnections(Button clickedButton)
        {

            foreach (var mapping in pairs)
            {
                Button leftButtonContent = mapping.Item1;
                Button rightButtonContent = mapping.Item2;

                if (leftButtonContent == clickedButton || rightButtonContent == clickedButton)
                {
                    pairs.Remove(mapping);
                    leftButtonContent.Background = TransparentBrush;
                    rightButtonContent.Background = TransparentBrush;
                    clickedButton.Background = SelectedBrush;

                    return;
                }
            }
        }

        private void CheckConnection()
        {
            if (_selectedLeftButton == null || _selectedRightButton == null)
                return;

            pairs.Add(new Tuple<Button, Button>(_selectedLeftButton, _selectedRightButton));

            _selectedLeftButton.Background = MappedBrush;
            _selectedRightButton.Background = MappedBrush;
            _selectedLeftButton = null;
            _selectedRightButton = null;

        }

        private void LeftOption_Click(object sender, RoutedEventArgs e)
        {
            var clickedButton = sender as Button;

            HandleOptionClick(ref _selectedLeftButton, clickedButton);
            DestroyExistingConnections(clickedButton);
            CheckConnection();
        }

        private void RightOption_Click(object sender, RoutedEventArgs e)
        {
            var clickedButton = sender as Button;

            HandleOptionClick(ref _selectedRightButton, clickedButton);
            DestroyExistingConnections(clickedButton);
            CheckConnection();
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            List<Tuple<string, string>> contentPairs = pairs
                .Select(mapping => new Tuple<string, string>(
                    mapping.Item1.Content.ToString(),
                    mapping.Item2.Content.ToString()))
                .ToList(); 
            
            // IMPLEMENT SEND RESULTS
        }
    }
}
