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
using System.Reflection.Metadata;
using System.Diagnostics;
using Duo.Views.Components;
using Duo.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using Duo.Models.Exercises;
using static Duo.Views.Components.AssociationExercise;
using static Duo.Views.Components.MultipleChoiceExercise;
using static Duo.Views.Components.FillInTheBlanksExercise;
using Duo.Models;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Duo.Views.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class QuizPage : Page
    {
        private static readonly SolidColorBrush CorrectBrush = new SolidColorBrush(Microsoft.UI.Colors.Green);
        private static readonly SolidColorBrush IncorrectBrush = new SolidColorBrush(Microsoft.UI.Colors.Red);

        public QuizPage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is int quizId)
            {
                Debug.WriteLine($"QuizPage received QuizId: {quizId}");
                ViewModel.QuizId = quizId;

                await ViewModel.LoadExercises();
                LoadCurrentExercise();
            }
        }


        public void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
            }
        }

        public void CancelButton_Click(object senderm, RoutedEventArgs e)
        {
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
            }
        }


        private void LoadCurrentExercise()
        {
            if (ViewModel != null && ViewModel.Exercises != null)
            {
                var currentExercise = ViewModel.CurrentExercise;

                if (currentExercise != null)
                {
                    // Remove any existing difficulty indicators
                    var mainStackPanel = ExerciseContentControl.Parent as StackPanel;
                    if (mainStackPanel != null)
                    {
                        var existingIndicators = mainStackPanel.Children.OfType<Grid>()
                            .Where(g => g.Children.OfType<FontIcon>().Any() && g.Children.OfType<TextBlock>().Any())
                            .ToList();
                        foreach (var indicator in existingIndicators)
                        {
                            mainStackPanel.Children.Remove(indicator);
                        }
                    }

                    // Add difficulty indicator
                    var difficultyGrid = new Grid
                    {
                        Background = new SolidColorBrush(Microsoft.UI.Colors.Transparent),
                        HorizontalAlignment = HorizontalAlignment.Right,
                        Margin = new Thickness(0, 0, 0, 16),
                        Padding = new Thickness(12, 6, 12, 6)
                    };

                    difficultyGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                    difficultyGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

                    var icon = new FontIcon
                    {
                        FontSize = 16,
                        Margin = new Thickness(0, 0, 8, 0)
                    };

                    var text = new TextBlock
                    {
                        FontSize = 14,
                        Style = (Style)Application.Current.Resources["BodyStrongTextBlockStyle"]
                    };

                    switch (currentExercise.Difficulty)
                    {
                        case Difficulty.Easy:
                            icon.Glyph = "\uE73E"; // Checkmark
                            text.Text = "Easy";
                            text.Foreground = new SolidColorBrush(Microsoft.UI.Colors.Green);
                            break;
                        case Difficulty.Normal:
                            icon.Glyph = "\uE7C3"; // Warning
                            text.Text = "Normal";
                            text.Foreground = new SolidColorBrush(Microsoft.UI.Colors.Orange);
                            break;
                        case Difficulty.Hard:
                            icon.Glyph = "\uE783"; // Star
                            text.Text = "Hard";
                            text.Foreground = new SolidColorBrush(Microsoft.UI.Colors.Red);
                            break;
                    }

                    Grid.SetColumn(icon, 0);
                    Grid.SetColumn(text, 1);

                    difficultyGrid.Children.Add(icon);
                    difficultyGrid.Children.Add(text);

                    // Add the difficulty indicator to the main StackPanel
                    if (mainStackPanel != null)
                    {
                        mainStackPanel.Children.Insert(1, difficultyGrid); // Insert after the back button
                    }

                    if (currentExercise is Models.Exercises.AssociationExercise associationExercise)
                    {
                        var associationControl = new Components.AssociationExercise()
                        {
                            Question = associationExercise.Question,
                            FirstAnswersList = new ObservableCollection<string>(associationExercise.FirstAnswersList),
                            SecondAnswersList = new ObservableCollection<string>(associationExercise.SecondAnswersList)
                        };
                        associationControl.OnSendClicked += AssociationControl_OnSendClicked;

                        ExerciseContentControl.Content = associationControl;
                    }
                    else if (currentExercise is Models.Exercises.FillInTheBlankExercise fillInTheBlanksExercise)
                    {
                        var fillInTheBlanksControl = new FillInTheBlanksExercise()
                        {
                            Question = fillInTheBlanksExercise.Question
                        };
                        fillInTheBlanksControl.OnSendClicked += FillInTheBlanksControl_OnSendClicked;

                        ExerciseContentControl.Content = fillInTheBlanksControl;
                    }
                    else if (currentExercise is Models.Exercises.MultipleChoiceExercise multipleChoiceExercise)
                    {
                        var multipleChoiceControl = new Components.MultipleChoiceExercise()
                        {
                            Question = multipleChoiceExercise.Question,
                            Answers = new ObservableCollection<MultipleChoiceAnswerModel>(multipleChoiceExercise.Choices)
                        };
                        multipleChoiceControl.OnSendClicked += MultipleChoiceControl_OnSendClicked;

                        ExerciseContentControl.Content = multipleChoiceControl;
                    }
                }
                else
                {
                    NextExerciseButton.Visibility = Visibility.Collapsed;

                    var endScreen = new Components.QuizEndScreen()
                    {
                        CorrectAnswersText = ViewModel.CorrectAnswersText,
                        PassingPercentText = ViewModel.PassingPercentText,
                        IsPassedText = ViewModel.IsPassedText
                    };

                    ExerciseContentControl.Content = endScreen;
                }
            }
        }


        private async void ShowMessage(FrameworkElement parentElement, bool valid)
        {

            var feedbackPopup = new AnswerFeedbackPopup();
            feedbackPopup.XamlRoot = parentElement.XamlRoot;


            if (valid)
            {
                feedbackPopup.ShowCorrectAnswer(ViewModel.GetCurrentExerciseCorrectAnswer());
            }
            else
            {
                feedbackPopup.ShowWrongAnswer(ViewModel.GetCurrentExerciseCorrectAnswer());
            }

            await feedbackPopup.ShowAsync();
        }


        private void AssociationControl_OnSendClicked(object sender, AssociationExerciseEventArgs e)
        {
            if (ViewModel.ValidatedCurrent == null)
            {
                var contentPairs = e.ContentPairs;

                var valid = (bool)ViewModel.ValidateCurrentExercise(contentPairs);

                ShowMessage((FrameworkElement)sender, valid);
            }
        }
        private void MultipleChoiceControl_OnSendClicked(object sender, MultipleChoiceExerciseEventArgs e)
        {
            if (ViewModel.ValidatedCurrent == null)
            {
                var contentPairs = e.ContentPairs;

                var valid = (bool)ViewModel.ValidateCurrentExercise(contentPairs);
                ShowMessage((FrameworkElement)sender, valid);
            }
        }
        private void FillInTheBlanksControl_OnSendClicked(object sender, FillInTheBlanksExerciseEventArgs e)
        {
            if (ViewModel.ValidatedCurrent == null)
            {
                var contentPairs = e.ContentPairs;

                var valid = (bool)ViewModel.ValidateCurrentExercise(contentPairs);
                ShowMessage((FrameworkElement)sender, valid);
            }
        }
        private void NextQuiz_Click(object sender, RoutedEventArgs e)
        {
            var loadedNext = ViewModel.LoadNext();

            if (loadedNext)
                LoadCurrentExercise();
        }
    }
}

