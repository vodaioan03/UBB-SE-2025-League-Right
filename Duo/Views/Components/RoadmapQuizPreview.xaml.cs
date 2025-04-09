using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Duo.Models.Quizzes;
using Duo.ViewModels.Roadmap;
using Duo.Views.Pages;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;

namespace Duo.Views.Components
{
    public sealed partial class RoadmapQuizPreview : UserControl
    {
        public RoadmapQuizPreview()
        {
            this.InitializeComponent();

            // BuildUI();
        }

        public void OpenQuizButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                Frame parentFrame = Helpers.Helpers.FindParent<Frame>(this);
                if (parentFrame != null)
                {
                    if (ViewModel.Quiz is Exam)
                    {
                        Debug.WriteLine("HEI");
                        parentFrame.Navigate(typeof(QuizPage), (ViewModel.Quiz.Id, true));
                    }
                    else
                    {
                        parentFrame.Navigate(typeof(QuizPage), (ViewModel.Quiz.Id, false));
                    }
                }
            }
        }

        public async Task Load(int quizId, bool isExam)
        {
            await ViewModel.OpenForQuiz(quizId, isExam);
        }
    }
}
