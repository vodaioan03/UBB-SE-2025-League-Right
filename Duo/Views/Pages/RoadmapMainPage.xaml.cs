using System;
using System.Collections.Generic;
using System.IO;
using System.Collections.ObjectModel;
using System.Linq;
using System.Diagnostics;
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
using Duo.Models.Exercises;
using Duo.Models.Quizzes;
using Microsoft.Extensions.DependencyInjection;
using Duo.ViewModels.Roadmap;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.
namespace Duo.Views.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RoadmapMainPage : Page
    {
        public RoadmapMainPage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            // var ViewModel = (RoadmapMainPageViewModel)(App.serviceProvider.GetService(typeof(RoadmapMainPageViewModel)));
            await ViewModel.SetupViewModel();
            base.OnNavigatedTo(e);
        }

            // public ObservableCollection<MultipleChoiceAnswerModel> Answers = new ObservableCollection<MultipleChoiceAnswerModel>
            // {
            //    new() { Answer = "Option A", IsCorrect = false },
            //    new() { Answer = "Option B", IsCorrect = true }
            // };
            // public ObservableCollection<string> FirstAnswersList = new ObservableCollection<string>
            // {
            //    "Romania", "Bulgaria", "Moldova"
            // };
            // public ObservableCollection<string> SecondAnswersList = new ObservableCollection<string>
            // {
            //    "Bucharest", "Sofia", "Chisinau"
            // };
    }
}
