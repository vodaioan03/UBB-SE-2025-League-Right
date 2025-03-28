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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Duo.Views.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CreateExercisePage : Page
    {
        private string _questionText = string.Empty;
        public CreateExercisePage()
        {
            this.InitializeComponent();
        }

        public void ExerciseComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        public void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
            }
        }

        public string QuestionText
        {
            get => _questionText;
            set
            {
                _questionText = value;
            }
        }

        public void CancelButton_Click(object senderm, RoutedEventArgs e)
        {

        }


        public void SaveButton_Click(object senderm, RoutedEventArgs e)
        {

        }

    }
}

