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
using Microsoft.UI.Xaml.Shapes;
using Microsoft.UI.Xaml.Media.Animation;
using Duo.Models.Exercises;

namespace Duo.Views.Components
{
    /// <summary>
    /// Flashcard exercise component that displays a question on the front and answer on the back
    /// </summary>
    public sealed partial class FlashcardExercise : UserControl
    {
        private DispatcherTimer _timer;
        private int _timerDuration; // Maximum time in seconds
        private TimeSpan _remainingTime; // Time remaining
        private TimeSpan _elapsedTime; // Time elapsed for statistics
        private bool _isRunning;
        private Models.Exercises.FlashcardExercise _exerciseData;
        
        // Total time for timer (in seconds) based on difficulty
        private int GetTimerDurationByDifficulty(Duo.Models.Difficulty difficulty)
        {
            switch (difficulty)
            {
                case Duo.Models.Difficulty.Easy:
                    return 15; // 15 seconds for easy
                case Duo.Models.Difficulty.Hard:
                    return 45; // 45 seconds for hard
                case Duo.Models.Difficulty.Normal:
                default:
                    return 30; // 30 seconds for normal
            }
        }
        
        /// <summary>
        /// Raised when the exercise is completed by clicking OK
        /// </summary>
        public event EventHandler<bool> ExerciseCompleted;
        
        /// <summary>
        /// Raised when the exercise is closed by clicking Close
        /// </summary>
        public event EventHandler ExerciseClosed;

        // User's answer from the fill-in-the-gap input
        public string UserAnswer
        {
            get => FillInGapInput?.Text ?? string.Empty;
        }

        // Topic property for category display
        public static readonly DependencyProperty TopicProperty =
            DependencyProperty.Register(nameof(Topic), typeof(string), typeof(FlashcardExercise), 
                new PropertyMetadata(string.Empty, OnTopicChanged));

        public string Topic
        {
            get => (string)GetValue(TopicProperty);
            set => SetValue(TopicProperty, value);
        }

        // Question property
        public static readonly DependencyProperty QuestionProperty =
            DependencyProperty.Register(nameof(Question), typeof(string), typeof(FlashcardExercise), 
                new PropertyMetadata(string.Empty, OnQuestionChanged));

        public string Question
        {
            get => (string)GetValue(QuestionProperty);
            set => SetValue(QuestionProperty, value);
        }

        // Answer property
        public static readonly DependencyProperty AnswerProperty =
            DependencyProperty.Register(nameof(Answer), typeof(string), typeof(FlashcardExercise), 
                new PropertyMetadata(string.Empty));

        public string Answer
        {
            get => (string)GetValue(AnswerProperty);
            set => SetValue(AnswerProperty, value);
        }

        // Handle changes to the Topic property
        private static void OnTopicChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FlashcardExercise flashcard && e.NewValue is string topic)
            {
                flashcard.UpdateTopicDisplay(topic);
            }
        }

        // Update topic display on both sides of the card
        private void UpdateTopicDisplay(string topic)
        {
            if (TopicTitle != null)
                TopicTitle.Text = topic;
                
            if (BackTopicTitle != null)
                BackTopicTitle.Text = topic;
        }

        // Handle changes to the Question property
        private static void OnQuestionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FlashcardExercise flashcard && e.NewValue is string question)
            {
                // The question title is already bound with x:Bind
                // We just need to update the question parts for the fill-in-the-gap
                flashcard.UpdateQuestionParts(question);
            }
        }

        // Constructor
        public FlashcardExercise()
        {
            this.InitializeComponent();
            this.Loaded += FlashcardExercise_Loaded;
        }

        private void FlashcardExercise_Loaded(object sender, RoutedEventArgs e)
        {
            // Ensure timer is setup
            if (_timer == null)
            {
                SetupTimer();
            }
            
            StartTimer();
            
            // Set initial question parts if not already set
            if (string.IsNullOrEmpty(QuestionPart1?.Text))
            {
                UpdateQuestionParts(Question);
            }
            
            // Initially hide feedback icons until answer is evaluated
            if (RightAnswerIcon != null)
                RightAnswerIcon.Opacity = 0.5;
                
            if (WrongAnswerIcon != null)
                WrongAnswerIcon.Opacity = 0.5;
        }

        private void UpdateQuestionParts(string question)
        {
            if (QuestionPart1 == null || QuestionPart2 == null || string.IsNullOrEmpty(question))
                return;

            // Set actual question text to display
            if (QuestionDisplay != null)
            {
                QuestionDisplay.Text = question;
            }
            
            // Try to create a fill-in-the-blank by analyzing the question string
            
            // Check if the question already has a blank placeholder
            if (question.Contains("___"))
            {
                string[] parts = question.Split("___");
                if (parts.Length >= 2)
                {
                    QuestionPart1.Text = parts[0].Trim();
                    QuestionPart2.Text = parts[1].Trim();
                    return;
                }
            }
            
            // No placeholder found, create a fill-in-the-blank by breaking the question
            // into two parts around the expected answer
            if (!string.IsNullOrEmpty(Answer) && question.Contains(Answer, StringComparison.OrdinalIgnoreCase))
            {
                int answerIndex = question.IndexOf(Answer, StringComparison.OrdinalIgnoreCase);
                QuestionPart1.Text = question.Substring(0, answerIndex).Trim();
                
                int afterAnswerIndex = answerIndex + Answer.Length;
                if (afterAnswerIndex < question.Length)
                {
                    QuestionPart2.Text = question.Substring(afterAnswerIndex).Trim();
                }
                else
                {
                    QuestionPart2.Text = "";
                }
                
                return;
            }

            // Default fallback if we can't determine how to split
            QuestionPart1.Text = "Fill in the answer";
            QuestionPart2.Text = "";
        }

        private void SetupTimer()
        {
            // Get duration in seconds for this difficulty
            _timerDuration = GetTimerDurationByDifficulty(_exerciseData?.ExerciseDifficulty ?? Duo.Models.Difficulty.Normal);
            
            // Initialize remaining time to full duration
            _remainingTime = TimeSpan.FromSeconds(_timerDuration);
            _elapsedTime = TimeSpan.Zero;

            // Set up timer text to show max time
            if (TimerText != null)
            {
                TimerText.Text = string.Format("00:{0:00}", _timerDuration);
            }

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, object e)
        {
            if (_remainingTime.TotalSeconds > 0)
            {
                // Decrease remaining time by 1 second
                _remainingTime = _remainingTime.Subtract(TimeSpan.FromSeconds(1));
                
                // Increase elapsed time for statistics
                _elapsedTime = _elapsedTime.Add(TimeSpan.FromSeconds(1));
                
                // Update timer display with remaining time
                if (TimerText != null)
                {
                    int seconds = (int)_remainingTime.TotalSeconds;
                    TimerText.Text = string.Format("00:{0:00}", seconds);
                }
                
                // Calculate progress as ratio of elapsed time to total duration
                double progress = 1.0 - (_remainingTime.TotalSeconds / _timerDuration);
                UpdateTimerVisual(progress);
                
                // If time is up, automatically flip the card
                if (_remainingTime.TotalSeconds <= 0)
                {
                    PerformCardFlip();
                }
            }
            else
            {
                // Time is up, automatically flip the card if not already flipped
                if (FrontSide.Visibility == Visibility.Visible)
                {
                    PerformCardFlip();
                }
            }
        }

        private void UpdateTimerVisual(double progress)
        {
            try 
            {
                // Define colors
                SolidColorBrush redBrush = new SolidColorBrush(Microsoft.UI.Colors.Red);
                SolidColorBrush orangeBrush = new SolidColorBrush(Microsoft.UI.Colors.Orange);
                SolidColorBrush blackBrush = new SolidColorBrush(Microsoft.UI.Colors.Black);
                
                // Change color based on remaining time
                if (progress > 0.75)
                {
                    // Red for last 25% of time
                    if (TimerArc != null)
                        TimerArc.Fill = redBrush;
                        
                    if (TimerText != null)
                        TimerText.Foreground = redBrush;
                }
                else if (progress > 0.5)
                {
                    // Orange for 50-75% of time
                    if (TimerArc != null)
                        TimerArc.Fill = orangeBrush;
                        
                    if (TimerText != null)
                        TimerText.Foreground = orangeBrush;
                }
                else
                {
                    // Default black for 0-50% of time
                    if (TimerArc != null)
                        TimerArc.Fill = blackBrush;
                        
                    if (TimerText != null)
                        TimerText.Foreground = blackBrush;
                }
                
                // Calculate the angle for the arc
                double angle = progress * 360;
                double radians = (angle - 90) * Math.PI / 180.0;
                double radius = 15.0;
                double endX = 20 + radius * Math.Cos(radians);
                double endY = 20 + radius * Math.Sin(radians);

                // Create SVG path for arc
                string arcFlag = angle > 180 ? "1" : "0";
                string pathData = $"M 20,20 L 20,5 A {radius},{radius} 0 {arcFlag} 1 {endX},{endY} z";
                
                // Update the timer arc path
                if (TimerArc != null)
                {
                    var pathGeometry = new PathGeometry();
                    var figure = new PathFigure();
                    figure.StartPoint = new Point(20, 20);
                    
                    // Create segment for the straight line
                    var lineSegment = new LineSegment();
                    lineSegment.Point = new Point(20, 5);
                    figure.Segments.Add(lineSegment);
                    
                    // Create arc segment
                    var arcSegment = new ArcSegment();
                    arcSegment.Point = new Point(endX, endY);
                    arcSegment.Size = new Size(radius, radius);
                    arcSegment.SweepDirection = SweepDirection.Clockwise;
                    arcSegment.IsLargeArc = angle > 180;
                    figure.Segments.Add(arcSegment);
                    
                    // Complete the path
                    figure.IsClosed = true;
                    pathGeometry.Figures.Add(figure);
                    
                    TimerArc.Data = pathGeometry;
                }
            }
            catch (Exception ex)
            {
                // Fallback if there's an issue
                System.Diagnostics.Debug.WriteLine($"Error updating timer: {ex.Message}");
            }
        }

        private void StartTimer()
        {
            if (_timer != null && !_timer.IsEnabled)
            {
                _timer.Start();
            }
        }

        private void StopTimer()
        {
            if (_timer != null && _timer.IsEnabled)
            {
                _timer.Stop();
            }
        }

        private void ResetTimer()
        {
            try
            {
                StopTimer();
                
                // Reset times
                _elapsedTime = TimeSpan.Zero;
                _remainingTime = TimeSpan.FromSeconds(_timerDuration);
                
                if (TimerText != null)
                {
                    TimerText.Text = string.Format("00:{0:00}", _timerDuration);
                    TimerText.Foreground = new SolidColorBrush(Microsoft.UI.Colors.Black);
                }
                
                if (TimerArc != null)
                {
                    TimerArc.Fill = new SolidColorBrush(Microsoft.UI.Colors.Black);
                }
                    
                UpdateTimerVisual(0);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error resetting timer: {ex.Message}");
            }
        }

        public void Initialize(Models.Exercises.FlashcardExercise exercise)
        {
            _exerciseData = exercise;
            
            // Set topic, question and answer from the model
            Topic = string.IsNullOrEmpty(exercise.Topic) ? 
                DetermineTopicFromExercise(exercise) : // Fallback for backward compatibility
                exercise.Topic;
            Question = exercise.Question;
            Answer = exercise.Answer;
            
            // Reset the UI state
            FrontSide.Visibility = Visibility.Visible;
            BackSide.Visibility = Visibility.Collapsed;
            
            // Clear the user input
            if (FillInGapInput != null)
                FillInGapInput.Text = string.Empty;
            
            // Reset feedback icons
            if (RightAnswerIcon != null)
                RightAnswerIcon.Opacity = 0.5;
                
            if (WrongAnswerIcon != null)
                WrongAnswerIcon.Opacity = 0.5;
            
            // Reset the timer with the appropriate duration for this exercise's difficulty
            ResetTimer();
            
            // Setup timer fresh to ensure proper difficulty-based duration
            SetupTimer();
            
            // Start the timer
            StartTimer();
            
            // Update difficulty info on UI if needed
            UpdateDifficultyIndicator(exercise.ExerciseDifficulty);
        }

        // This is a temporary function that would be replaced with actual data from a database
        private string DetermineTopicFromExercise(Models.Exercises.FlashcardExercise exercise)
        {
            // This is just a placeholder function that extracts topic from question content
            // In a real implementation, the exercise would have a Topic property from the database
            string question = exercise.Question?.ToLower() ?? string.Empty;
            
            if (question.Contains("mountain"))
                return "Mountains";
            else if (question.Contains("capital"))
                return "Capitals";
            else if (question.Contains("river"))
                return "Rivers";
            else if (question.Contains("country"))
                return "Countries";
            else if (question.Contains("ocean") || question.Contains("sea"))
                return "Bodies of Water";
            else
                return "General Knowledge";
        }

        // Common method for flipping the card to avoid code duplication
        private void PerformCardFlip()
        {
            // Store elapsed time in the exercise data
            if (_exerciseData != null)
            {
                _exerciseData.ElapsedTime = _elapsedTime;
            }
            
            // Check if the user's answer matches the correct answer
            bool isCorrect = IsAnswerCorrect();
            
            // Update the feedback icons
            if (RightAnswerIcon != null)
                RightAnswerIcon.Opacity = isCorrect ? 1.0 : 0.3;
                
            if (WrongAnswerIcon != null)
            {
                WrongAnswerIcon.Opacity = isCorrect ? 0.3 : 1.0;
                
                // Make the X red when answer is incorrect
                if (!isCorrect)
                {
                    // Find the path that makes the X and update its color
                    var wrongPath = WrongAnswerIcon.Children.OfType<Microsoft.UI.Xaml.Shapes.Path>().FirstOrDefault();
                    if (wrongPath != null)
                    {
                        wrongPath.Stroke = new SolidColorBrush(Microsoft.UI.Colors.Red);
                    }
                    
                    // Find the ellipse and update its stroke color
                    var wrongEllipse = WrongAnswerIcon.Children.OfType<Ellipse>().FirstOrDefault();
                    if (wrongEllipse != null)
                    {
                        wrongEllipse.Stroke = new SolidColorBrush(Microsoft.UI.Colors.Red);
                    }
                }
            }
            
            // Update OK button color based on correctness
            var okButton = this.FindName("OkButton") as Button;
            if (okButton != null)
            {
                okButton.Background = new SolidColorBrush(
                    isCorrect ? 
                    Microsoft.UI.ColorHelper.FromArgb(255, 102, 204, 102) : // #66CC66 (green)
                    Microsoft.UI.Colors.Red);
            }

            // Flip the card
            FrontSide.Visibility = Visibility.Collapsed;
            BackSide.Visibility = Visibility.Visible;
        }

        private void FlipButton_Click(object sender, RoutedEventArgs e)
        {
            // Stop the timer
            StopTimer();
            
            // Perform the flip
            PerformCardFlip();
        }

        private bool IsAnswerCorrect()
        {
            if (string.IsNullOrEmpty(UserAnswer) || string.IsNullOrEmpty(Answer))
                return false;
                
            // Simple case-insensitive comparison
            return UserAnswer.Trim().Equals(Answer.Trim(), StringComparison.OrdinalIgnoreCase);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            StopTimer();
            
            if (_exerciseData != null)
            {
                _exerciseData.ElapsedTime = _elapsedTime;
            }

            // Notify listeners that the card was closed
            ExerciseClosed?.Invoke(this, EventArgs.Empty);
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            StopTimer();
            
            if (_exerciseData != null)
            {
                _exerciseData.ElapsedTime = _elapsedTime;
            }

            // Notify listeners that the card was completed
            ExerciseCompleted?.Invoke(this, IsAnswerCorrect());
        }

        public void Reset()
        {
            // Reset UI state
            FrontSide.Visibility = Visibility.Visible;
            BackSide.Visibility = Visibility.Collapsed;
            
            // Clear user input
            if (FillInGapInput != null)
                FillInGapInput.Text = string.Empty;
            
            // Reset feedback icons
            if (RightAnswerIcon != null)
                RightAnswerIcon.Opacity = 0.5;
                
            if (WrongAnswerIcon != null)
                WrongAnswerIcon.Opacity = 0.5;
            
            // Reset timer
            ResetTimer();
            StartTimer();
        }

        public TimeSpan GetElapsedTime()
        {
            return _elapsedTime;
        }

        // Update UI to show difficulty level
        private void UpdateDifficultyIndicator(Duo.Models.Difficulty difficulty)
        {
            // Get duration in seconds for this difficulty
            int seconds = GetTimerDurationByDifficulty(difficulty);
            _timerDuration = seconds;
            _remainingTime = TimeSpan.FromSeconds(seconds);
            
            // Update timer text to show maximum time
            if (TimerText != null)
            {
                TimerText.Text = string.Format("00:{0:00}", seconds);
                // Ensure timer text has default color
                TimerText.Foreground = new SolidColorBrush(Microsoft.UI.Colors.Black);
            }
            
            // Ensure timer arc has default color
            if (TimerArc != null)
            {
                TimerArc.Fill = new SolidColorBrush(Microsoft.UI.Colors.Black);
            }
            
            try
            {
                // Update difficulty text
                var difficultyText = this.FindName("DifficultyText") as TextBlock;
                if (difficultyText != null)
                {
                    difficultyText.Text = difficulty.ToString();
                }
                
                // Update difficulty bars visibility
                var difficultyBars = this.FindName("DifficultyBars") as StackPanel;
                if (difficultyBars != null && difficultyBars.Children.Count >= 3)
                {
                    // Find the individual bars
                    var easyBar = difficultyBars.Children[0] as Rectangle;
                    var normalBar = difficultyBars.Children[1] as Rectangle;
                    var hardBar = difficultyBars.Children[2] as Rectangle;
                    
                    if (easyBar != null && normalBar != null && hardBar != null)
                    {
                        // Reset all bars
                        easyBar.Opacity = 0.3;
                        normalBar.Opacity = 0.3;
                        hardBar.Opacity = 0.3;
                        
                        // Highlight appropriate bars based on difficulty
                        switch(difficulty)
                        {
                            case Duo.Models.Difficulty.Easy:
                                easyBar.Opacity = 1.0;
                                break;
                            case Duo.Models.Difficulty.Normal:
                                easyBar.Opacity = 1.0;
                                normalBar.Opacity = 1.0;
                                break;
                            case Duo.Models.Difficulty.Hard:
                                easyBar.Opacity = 1.0;
                                normalBar.Opacity = 1.0;
                                hardBar.Opacity = 1.0;
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error updating difficulty indicator: {ex.Message}");
            }
        }
    }
} 