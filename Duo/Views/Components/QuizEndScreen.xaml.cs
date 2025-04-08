using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Duo.Models.Exercises;
using Duo.Views.Pages;
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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.
namespace Duo.Views.Components
{
    public sealed partial class QuizEndScreen : UserControl
    {
        public QuizEndScreen()
        {
            this.InitializeComponent();
        }

        public string CorrectAnswersText { get; set; }
        public string PassingPercentText { get; set; }
        public string IsPassedText { get; set; }

        private void GoToRoadmap_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                Frame parentFrame = Helpers.Helpers.FindParent<Frame>(this);
                if (parentFrame != null)
                {
                    parentFrame.Navigate(typeof(RoadmapMainPage));
                }
            }
        }
    }
}
