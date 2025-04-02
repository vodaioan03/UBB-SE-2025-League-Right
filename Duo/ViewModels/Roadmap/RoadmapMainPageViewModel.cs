using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using Duo;
using Duo.Commands;
using Duo.Models.Quizzes;
using Duo.Models.Sections;
using Duo.Services;
using Duo.ViewModels.Base;
using Duo.ViewModels.Roadmap;
using Duo.Views.Components;
using Microsoft.Extensions.DependencyInjection;

namespace Duo.ViewModels.Roadmap
{
    public class RoadmapMainPageViewModel : ViewModelBase
    {
        private BaseQuiz _selectedQuiz;
        private Section _currentSection;
        private QuizService _quizService;
        private SectionService _sectionService;

        public ICommand OpenQuizPreviewCommand;
        public ICommand StartQuizCommand;


        public RoadmapMainPageViewModel()
        {
            _quizService = (QuizService)App.serviceProvider.GetService(typeof(QuizService));
            _sectionService = (SectionService)App.serviceProvider.GetService(typeof(SectionService));

            StartQuizCommand = new RelayCommand(StartQuiz);
            OpenQuizPreviewCommand = new RelayCommandWithParameter<Tuple<int, bool>>(OpenQuizPreview);
        }

        private async void OpenQuizPreview(Tuple<int, bool> args)
        {
            Debug.WriteLine($"Opening quiz with ID: {args.Item1}");

            var quizPreviewViewModel = (RoadmapQuizPreviewViewModel)App.serviceProvider.GetService(typeof(RoadmapQuizPreviewViewModel));
            await quizPreviewViewModel.OpenForQuiz(args.Item1, args.Item2);
        }

        private void StartQuiz()
        {
            Console.WriteLine($"Starting quiz with ID: {_selectedQuiz?.Id}");
            // Navigation logic goes here
        }
    }
}