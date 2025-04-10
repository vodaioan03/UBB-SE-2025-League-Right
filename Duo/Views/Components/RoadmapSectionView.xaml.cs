using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using Duo.Helpers;
using Duo.Models.Exercises;
using Duo.Models.Quizzes;
using Duo.ViewModels.Roadmap;
using Duo.Views.Components;
using Duo.Views.Pages;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Markup;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.
namespace Duo.Views.Components
{
    public sealed partial class RoadmapSectionView : UserControl
    {
        public event RoutedEventHandler Click;

        public RoadmapSectionView()
        {
            this.InitializeComponent();
        }

        private void Quiz_Click(object sender, RoutedEventArgs e)
        {
            if (sender is QuizRoadmapButton button)
            {
                Debug.WriteLine($"Quiz with ID {button.QuizId} clicked!");

                Frame parentFrame = Helpers.Helpers.FindParent<Frame>(this);
                if (parentFrame != null)
                {
                    parentFrame.Navigate(typeof(QuizPreviewPage), (button.QuizId, button.IsExam));
                }
            }
        }
    }
}
