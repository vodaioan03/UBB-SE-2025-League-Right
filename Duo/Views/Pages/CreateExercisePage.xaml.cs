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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Duo.Views.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CreateExercisePage : Page
    {

        public CreateExercisePage()
        {
            this.InitializeComponent();
        }

        public void ExerciseComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox exerciseComboBox = sender as ComboBox;
            Debug.WriteLine("This is a debug message.");

            switch (exerciseComboBox.SelectedItem.ToString())
            {
                case "Association":
                    ContentExerciseDisplayArea.Content = new CreateAssociationExercise();
                    Debug.WriteLine("Association");
                    break;

                case "Fill in the blank":
                    ContentExerciseDisplayArea.Content = new CreateFillInTheBlankExercise();
                    Debug.WriteLine("Fill in the blank");
                    break;

                case "Multiple Choice":
                    ContentExerciseDisplayArea.Content = new CreateMultipleChoiceExercise();
                    Debug.WriteLine("Fill in the blank");
                    break;

                case "Flashcard":
                    break;
                default:
                    break;
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

        /*
        public void SaveButton_Click(object senderm, RoutedEventArgs e)
        {
            ViewModel.CreateExercise();
        }*/

    }
}

