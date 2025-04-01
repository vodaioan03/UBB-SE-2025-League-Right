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
using Duo.Models.Quizzes;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Duo.Views.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class QuizPreviewPage : Page
    {

        public QuizPreviewPage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is ValueTuple<int, bool> parameters)
            {
                int quizId = parameters.Item1;
                bool isExam = parameters.Item2;

                QuizPreview.Load(quizId, isExam);

                Debug.WriteLine($"QuizPreviewPage received QuizId: {quizId}");
            }
            base.OnNavigatedTo(e);
        }

        public void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
            }
        }
        
    }
}

