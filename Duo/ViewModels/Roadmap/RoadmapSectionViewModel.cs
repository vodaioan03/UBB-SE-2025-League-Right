using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Duo.Models.Quizzes;
using Duo.Models.Sections;
using Duo.Services;
using Duo.ViewModels.Base;

namespace Duo.ViewModels.Roadmap
{
    public class RoadmapSectionViewModel : ViewModelBase
    {
        private SectionService _sectionService;

        private int _sectionId;
        private Section _section;
        public Section Section
        {
            get => _section;
        }

        // TEMPORARY DATA
        //private ObservableCollection<Quiz> _quizzes = new ObservableCollection<Quiz>
        //{
        //    new Quiz(1, null, null),
        //    new Quiz(2, null, null),
        //    new Quiz(3, null, null),
        //    new Quiz(4, null, null),
        //    new Quiz(5, null, null),
        //    new Quiz(6, null, null),
        //    new Quiz(7, null, null)
        //};
        //private Exam _exam = new Exam(2, null);


        public ICommand OpenQuizPreviewCommand { get; set; }

        public RoadmapSectionViewModel()
        {
            _sectionService = (SectionService)(App.serviceProvider.GetService(typeof(SectionService)));

            var mainPageViewModel = (RoadmapMainPageViewModel)(App.serviceProvider.GetService(typeof(RoadmapMainPageViewModel)));
            OpenQuizPreviewCommand = mainPageViewModel.OpenQuizPreviewCommand;
            _quizButtonTemplates = new ObservableCollection<RoadmapButtonTemplate>();
        }

        public async Task SetupForSection(int sectionId)
        {
            Debug.WriteLine($"--------------------Setting up section with ID {sectionId}");
            _sectionId = sectionId;
            _section = await _sectionService.GetSectionById(_sectionId);
            QuizService quizService = (QuizService)App.serviceProvider.GetService(typeof(QuizService));
            _section.Quizzes = await quizService.GetAllQuizzesFromSection(_sectionId);
            _section.Exam = await quizService.GetExamFromSection(_sectionId);
            OnPropertyChanged(nameof(Section));
            // maybe doesnt work for the Text where it accesses Section.Title

            PopulateQuizButtonTemplates();
        }

        private void PopulateQuizButtonTemplates()
        {
            Debug.WriteLine($"++++++ Populating section {_section.Id}");
            foreach (Quiz quiz in _section.GetAllQuizzes())
            {
                _quizButtonTemplates.Add(new RoadmapButtonTemplate(quiz, OpenQuizPreviewCommand));
                Debug.WriteLine($"Added quiz with ID {quiz.Id} in section {_section.Id}");
            }
            _examButtonTemplate = new RoadmapButtonTemplate(_section.GetFinalExam(), OpenQuizPreviewCommand);

            // Notify UI that properties have changed
            OnPropertyChanged(nameof(QuizButtonTemplates));
            OnPropertyChanged(nameof(ExamButtonTemplate));
        }

        private ObservableCollection<RoadmapButtonTemplate> _quizButtonTemplates;

        public ObservableCollection<RoadmapButtonTemplate> QuizButtonTemplates
        {
            get => _quizButtonTemplates;
            private set
            {
                _quizButtonTemplates = value;
                OnPropertyChanged(nameof(QuizButtonTemplates));
            }
        }

        private RoadmapButtonTemplate _examButtonTemplate;

        public RoadmapButtonTemplate ExamButtonTemplate
        {
            get => _examButtonTemplate;
            private set
            {
                _examButtonTemplate = value;
                OnPropertyChanged(nameof(ExamButtonTemplate));
            }
        }

    }

    public class RoadmapButtonTemplate
    {
        public BaseQuiz Quiz { get; }
        public ICommand OpenQuizPreviewCommand { get; }

        public bool isExam
        {
            get => Quiz is Exam;
        }

        public RoadmapButtonTemplate(BaseQuiz quiz, ICommand openQuizPreviewCommand)
        {
            Quiz = quiz;
            OpenQuizPreviewCommand = openQuizPreviewCommand;
        }
    }

}
