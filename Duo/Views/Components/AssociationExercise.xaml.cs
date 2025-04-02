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
using Windows.UI;
using Windows.UI.ViewManagement;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Duo.Views.Components
{
    public sealed partial class AssociationExercise : UserControl
    {
        public event EventHandler<AssociationExerciseEventArgs> OnSendClicked;
        public event RoutedEventHandler Click;
        private Button _selectedLeftButton;
        private Button _selectedRightButton;

        public static readonly DependencyProperty QuestionProperty =
           DependencyProperty.Register(nameof(Question), typeof(string), typeof(AssociationExercise), new PropertyMetadata(""));

        public static readonly DependencyProperty FirstAnswersListProperty =
            DependencyProperty.Register(nameof(FirstAnswersList), typeof(ObservableCollection<string>), typeof(AssociationExercise), new PropertyMetadata(new ObservableCollection<string>()));

        public static readonly DependencyProperty SecondAnswersListProperty =
            DependencyProperty.Register(nameof(SecondAnswersList), typeof(ObservableCollection<string>), typeof(AssociationExercise), new PropertyMetadata(new ObservableCollection<string>()));

        private static readonly UISettings uiSettings = new UISettings();
        private readonly SolidColorBrush accentBrush;

        private static readonly SolidColorBrush TransparentBrush = new SolidColorBrush(Microsoft.UI.Colors.Transparent);
        private static readonly SolidColorBrush SelectedBrush = new SolidColorBrush(Color.FromArgb(255, 0, 120, 215));
        private static readonly SolidColorBrush MappedBrush = new SolidColorBrush(Color.FromArgb(255, 0, 120, 215));
        private static readonly SolidColorBrush DefaultBorderBrush = new SolidColorBrush(Color.FromArgb(255, 200, 200, 200));
        private static readonly SolidColorBrush LineBrush = new SolidColorBrush(Color.FromArgb(255, 0, 120, 215));

        private List<Tuple<Button, Button, Line>> pairs = new List<Tuple<Button, Button, Line>>();

        public AssociationExercise()
        {
            this.InitializeComponent();
            accentBrush = new SolidColorBrush(uiSettings.GetColorValue(UIColorType.Accent));
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
                selectedButton.Background = accentBrush;
                selectedButton.BorderBrush = accentBrush;
            }
            else
            {
                selectedButton = clickedButton;
                selectedButton.Background = accentBrush;
            }
        }

        private void DestroyExistingConnections(Button clickedButton)
        {
            foreach (var mapping in pairs.ToList())
            {
                Button leftButtonContent = mapping.Item1;
                Button rightButtonContent = mapping.Item2;
                Line line = mapping.Item3;

                if (leftButtonContent == clickedButton || rightButtonContent == clickedButton)
                {
                    pairs.Remove(mapping);
                    leftButtonContent.Background = TransparentBrush;
                    rightButtonContent.Background = TransparentBrush;
                    clickedButton.Background = accentBrush;
                    LinesCanvas.Children.Remove(line);

                    return;
                }
            }
        }

        private void CheckConnection()
        {
            if (_selectedLeftButton == null || _selectedRightButton == null)
                return;

            var line = new Line
            {
                Stroke = accentBrush,
                StrokeThickness = 2,
                X1 = GetCirclePosition(_selectedLeftButton, true).X,
                Y1 = GetCirclePosition(_selectedLeftButton, true).Y,
                X2 = GetCirclePosition(_selectedRightButton, false).X,
                Y2 = GetCirclePosition(_selectedRightButton, false).Y
            };

            LinesCanvas.Children.Add(line);
            pairs.Add(new Tuple<Button, Button, Line>(_selectedLeftButton, _selectedRightButton, line));

            _selectedLeftButton.Background = accentBrush;
            _selectedRightButton.Background = accentBrush;
            _selectedLeftButton = null;
            _selectedRightButton = null;
        }

        private Point GetCirclePosition(Button button, bool isLeftCircle)
        {
            var transform = button.TransformToVisual(LinesCanvas);
            var buttonPosition = transform.TransformPoint(new Point(0, 0));
            
            // Calculate the center of the button
            var buttonCenterY = buttonPosition.Y + (button.ActualHeight / 2);
            
            // Find the circle element within the button's parent StackPanel
            var stackPanel = button.Parent as StackPanel;
            if (stackPanel != null)
            {
                var circle = stackPanel.Children.OfType<Ellipse>().FirstOrDefault();
                if (circle != null)
                {
                    // Get the circle's position relative to the button
                    var circleTransform = circle.TransformToVisual(LinesCanvas);
                    var circlePosition = circleTransform.TransformPoint(new Point(0, 0));
                    
                    // Return the center of the circle
                    return new Point(
                        circlePosition.X + (circle.ActualWidth / 2),
                        circlePosition.Y + (circle.ActualHeight / 2)
                    );
                }
            }
            
            // Fallback calculation if circle not found
            var circleX = isLeftCircle 
                ? buttonPosition.X + button.ActualWidth + 12  // 12 is half of the circle width (24/2)
                : buttonPosition.X - 12;  // 12 is half of the circle width (24/2)
            
            return new Point(circleX, buttonCenterY);
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
            List<(string, string)> contentPairs = pairs
                .Select(mapping => (
                mapping.Item1.Content.ToString(),
                mapping.Item2.Content.ToString()
                ))
                .ToList();

            OnSendClicked?.Invoke(this, new AssociationExerciseEventArgs(contentPairs));
        }

        public class AssociationExerciseEventArgs : EventArgs
        {
            public List<(string, string)> ContentPairs { get; }

            public AssociationExerciseEventArgs(List<(string, string)> contentPairs)
            {
                ContentPairs = contentPairs;
            }
        }
    }
}
