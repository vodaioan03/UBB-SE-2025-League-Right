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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Duo.Views.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ExamPage : Page
    {
        private static readonly SolidColorBrush CorrectBrush = new SolidColorBrush(Microsoft.UI.Colors.Green);
        private static readonly SolidColorBrush IncorrectBrush = new SolidColorBrush(Microsoft.UI.Colors.Red);

        public ExamPage()
        {
            this.InitializeComponent();
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
                    ExerciseContentControl.Content = null;

                    //ADD END SCREEN;
                }

            }

        }

        private void AssociationControl_OnSendClicked(object sender, AssociationExerciseEventArgs e)
        {
            var contentPairs = e.ContentPairs;

            var valid = (bool)ViewModel.ValidateCurrentExercise(contentPairs);

            var loadedNext = ViewModel.LoadNext();

            if (loadedNext)
                LoadCurrentExercise();

        }
        private void MultipleChoiceControl_OnSendClicked(object sender, MultipleChoiceExerciseEventArgs e)
        {
            var contentPairs = e.ContentPairs;

            var valid = (bool)ViewModel.ValidateCurrentExercise(contentPairs);

            var loadedNext = ViewModel.LoadNext();

            if (loadedNext)
                LoadCurrentExercise();
        }
        private void FillInTheBlanksControl_OnSendClicked(object sender, FillInTheBlanksExerciseEventArgs e)
        {
            var contentPairs = e.ContentPairs;

            var valid = (bool)ViewModel.ValidateCurrentExercise(contentPairs);

            var loadedNext = ViewModel.LoadNext();

            if (loadedNext)
                LoadCurrentExercise();
        }
    }
}

