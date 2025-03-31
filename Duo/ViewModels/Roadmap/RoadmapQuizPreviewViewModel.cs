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
        private BaseQuiz _quiz;
        public BaseQuiz Quiz
        { 
            get => _quiz; 
            set => SetProperty(ref _quiz, value);
        }
        private Section _section;
        private Visibility _isPreviewVisible;
        private readonly QuizService _quizService;
        private readonly SectionService _sectionService;

        public Visibility IsPreviewVisible
        { 
            get => _isPreviewVisible; 
            set => SetProperty(ref _isPreviewVisible, value);
        }

        public string QuizOrderNumber
        {
            get
            {
                if (_quiz == null) return "-1";
                if (_quiz is Exam)
                    return "Exam";
                if (_quiz is Quiz quiz)
                    return quiz.OrderNumber.ToString() ?? "-1";
                return "-1";
            }
            set
            {
                if (_quiz is Quiz quiz)
                {
                    quiz.OrderNumber = int.Parse(value);
                    OnPropertyChanged(nameof(QuizOrderNumber));
                }
            }
        }

        public string SectionTitle
        {
            get => _section?.Title ?? "Unknown Section";
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

        private DispatcherQueue _dispatcherQueue;

        public RoadmapQuizPreviewViewModel()
        {
            _quizService = (QuizService)App.serviceProvider.GetService(typeof(QuizService));
            _sectionService = (SectionService)App.serviceProvider.GetService(typeof(SectionService));
            _isPreviewVisible = Visibility.Visible;

            var mainPageViewModel= (RoadmapMainPageViewModel)App.serviceProvider.GetService(typeof(RoadmapMainPageViewModel));
            StartQuizCommand = mainPageViewModel.StartQuizCommand;
            BackButtonCommand = new RelayCommand(() =>
            {
                _isPreviewVisible = Visibility.Collapsed;
                OnPropertyChanged(nameof(IsPreviewVisible));
            });

            _dispatcherQueue = DispatcherQueue.GetForCurrentThread();
        }

        //public async Task OpenForQuiz(int quizId, bool isExam)
        //{
        //    _isPreviewVisible = Visibility.Visible;
        //    OnPropertyChanged(nameof(IsPreviewVisible));
        //    if (isExam)
        //    {
        //        _quiz = await _quizService.GetExamById(quizId);
        //    }
        //    else
        //    {
        //        _quiz = await _quizService.GetQuizById(quizId);
        //        Debug.WriteLine($"Opening quiz: {_quiz}");
        //    }
        //    _section = await _sectionService.GetSectionById((int)_quiz.SectionId);
        //    // Notify UI that Quiz and Section have changed
        //    OnPropertyChanged(nameof(Quiz));
        //    OnPropertyChanged(nameof(SectionTitle));
        //    OnPropertyChanged(nameof(QuizOrderNumber));


        //    Debug.WriteLine($"VALUE OF QUIZ: {QuizOrderNumber}, {SectionTitle}, {IsPreviewVisible}");
        //}

        public async Task OpenForQuiz(int quizId, bool isExam)
        {
            _dispatcherQueue.TryEnqueue(() =>
            {
                IsPreviewVisible = Visibility.Visible;
                OnPropertyChanged(nameof(IsPreviewVisible));
            });

            if (isExam)
            {
                _quiz = await _quizService.GetExamById(quizId);
            }
            else
            {
                _quiz = await _quizService.GetQuizById(quizId);
                Debug.WriteLine($"Opening quiz: {_quiz}");
            }

            _section = await _sectionService.GetSectionById((int)_quiz.SectionId);

            _dispatcherQueue.TryEnqueue(() =>
            {
                Quiz = _quiz;
                OnPropertyChanged(nameof(Quiz));
                OnPropertyChanged(nameof(SectionTitle));
                OnPropertyChanged(nameof(QuizOrderNumber));
            });

            Debug.WriteLine($"VALUE OF QUIZ: {QuizOrderNumber}, {SectionTitle}, {IsPreviewVisible}");
        }

    }
}
