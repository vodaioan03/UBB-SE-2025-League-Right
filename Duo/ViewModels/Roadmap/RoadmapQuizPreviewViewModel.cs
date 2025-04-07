using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Duo.Commands;
using Duo.Models.Quizzes;
using Duo.Models.Sections;
using Duo.Services;
using Duo.ViewModels.Base;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
namespace Duo.ViewModels.Roadmap
{
    public class RoadmapQuizPreviewViewModel : ViewModelBase
    {
        private BaseQuiz quiz;
        public BaseQuiz Quiz
        {
            get => quiz;
            set => SetProperty(ref quiz, value);
        }
        private Section section;
        private Visibility isPreviewVisible;
        private readonly QuizService quizService;
        private readonly SectionService sectionService;

        public Visibility IsPreviewVisible
        {
            get => isPreviewVisible;
            set => SetProperty(ref isPreviewVisible, value);
        }

        public string QuizOrderNumber
        {
            get
            {
                if (this.quiz == null)
                {
                    return "-1";
                }
                if (this.quiz is Exam)
                {
                    return "Final Exam";
                }
                if (this.quiz is Quiz quizInstance)
                {
                    return $"Quiz nr. {quizInstance.OrderNumber.ToString()}" ?? "-1";
                }
                return "-1";
            }
            set
            {
                if (this.quiz is Quiz quizInstance)
                {
                    quizInstance.OrderNumber = int.Parse(value);
                    OnPropertyChanged(nameof(QuizOrderNumber));
                }
            }
        }

        public string SectionTitle
        {
            get => section?.Title ?? "Unknown Section";
        }
        public ICommand StartQuizCommand
        {
            get;
            set;
        }
        public ICommand BackButtonCommand
        {
            get;
            set;
        }

        private DispatcherQueue dispatcherQueue;

        public RoadmapQuizPreviewViewModel()
        {
            quizService = (QuizService)App.ServiceProvider.GetService(typeof(QuizService));
            sectionService = (SectionService)App.ServiceProvider.GetService(typeof(SectionService));
            isPreviewVisible = Visibility.Visible;

            var mainPageViewModel = (RoadmapMainPageViewModel)App.ServiceProvider.GetService(typeof(RoadmapMainPageViewModel));
            StartQuizCommand = mainPageViewModel.StartQuizCommand;
            BackButtonCommand = new RelayCommand(() =>
            {
                isPreviewVisible = Visibility.Collapsed;
                OnPropertyChanged(nameof(IsPreviewVisible));
            });

            dispatcherQueue = DispatcherQueue.GetForCurrentThread();
        }

        public async Task OpenForQuiz(int quizId, bool isExam)
        {
            dispatcherQueue.TryEnqueue(() =>
            {
                IsPreviewVisible = Visibility.Visible;
                OnPropertyChanged(nameof(IsPreviewVisible));
            });

            if (isExam)
            {
                quiz = await quizService.GetExamById(quizId);
            }
            else
            {
                quiz = await quizService.GetQuizById(quizId);
                Debug.WriteLine($"Opening quiz: {quiz}");
            }

            section = await sectionService.GetSectionById((int)quiz.SectionId);

            dispatcherQueue.TryEnqueue(() =>
            {
                Quiz = quiz;
                OnPropertyChanged(nameof(Quiz));
                OnPropertyChanged(nameof(SectionTitle));
                OnPropertyChanged(nameof(QuizOrderNumber));
            });

            Debug.WriteLine($"VALUE OF QUIZ: {QuizOrderNumber}, {SectionTitle}, {IsPreviewVisible}");
        }
    }
}
