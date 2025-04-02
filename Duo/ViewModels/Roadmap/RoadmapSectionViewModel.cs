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

        private bool _isCompleted;
        private int _nrOfCompletedQuizzes;

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

        public RoadmapSectionViewModel()
        {
            _sectionService = (SectionService)(App.serviceProvider.GetService(typeof(SectionService)));

            var mainPageViewModel = (RoadmapMainPageViewModel)(App.serviceProvider.GetService(typeof(RoadmapMainPageViewModel)));
            OpenQuizPreviewCommand = mainPageViewModel.OpenQuizPreviewCommand;
            _quizButtonTemplates = new ObservableCollection<RoadmapButtonTemplate>();
        }

        public async Task SetupForSection(int sectionId, bool isCompleted, int nrOfCompletedQuizzes)
        {
            Debug.WriteLine($"--------------------Setting up section with ID {sectionId}, Compl {isCompleted}, nr_quiz {nrOfCompletedQuizzes}");
            _sectionId = sectionId;
            _section = await _sectionService.GetSectionById(_sectionId);

            QuizService quizService = (QuizService)App.serviceProvider.GetService(typeof(QuizService));
            _section.Quizzes = await quizService.GetAllQuizzesFromSection(_sectionId);
            _section.Exam = await quizService.GetExamFromSection(_sectionId);
            
            _isCompleted = isCompleted;
            _nrOfCompletedQuizzes = nrOfCompletedQuizzes;

            OnPropertyChanged(nameof(Section));

            PopulateQuizButtonTemplates();
        }

        private void PopulateQuizButtonTemplates()
        {
            Debug.WriteLine($"++++++ Populating section {_section.Id}");
            foreach (Quiz quiz in _section.GetAllQuizzes())
            {
                RoadmapButtonTemplate.QUIZ_STATUS quizStatus = RoadmapButtonTemplate.QUIZ_STATUS.LOCKED;
                if (_isCompleted)
                {
                    quizStatus = RoadmapButtonTemplate.QUIZ_STATUS.COMPLETED;
                }
                else if (quiz.OrderNumber <= _nrOfCompletedQuizzes)
                {
                    quizStatus = RoadmapButtonTemplate.QUIZ_STATUS.COMPLETED;
                }
                else if (quiz.OrderNumber == _nrOfCompletedQuizzes + 1)
                {
                    quizStatus = RoadmapButtonTemplate.QUIZ_STATUS.INCOMPLETE;
                }

                Debug.WriteLine($"++++++++++ Populating quiz {quiz.Id} -> {quizStatus}");
                _quizButtonTemplates.Add(new RoadmapButtonTemplate(quiz, OpenQuizPreviewCommand, quizStatus));
                //Debug.WriteLine($"Added quiz with ID {quiz.Id} in section {_section.Id}");
            }
            RoadmapButtonTemplate.QUIZ_STATUS examStatus = RoadmapButtonTemplate.QUIZ_STATUS.LOCKED;
            if (_isCompleted)
            {
                examStatus = RoadmapButtonTemplate.QUIZ_STATUS.COMPLETED;
            }
            else if (_section.GetAllQuizzes().Count<Quiz>() == _nrOfCompletedQuizzes)
            {
                examStatus = RoadmapButtonTemplate.QUIZ_STATUS.INCOMPLETE;
            }
            Debug.WriteLine($"++++++++++ Populating exam -> {examStatus}");
            _examButtonTemplate = new RoadmapButtonTemplate(_section.GetFinalExam(), OpenQuizPreviewCommand, examStatus);

            OnPropertyChanged(nameof(QuizButtonTemplates));
            OnPropertyChanged(nameof(ExamButtonTemplate));
        }
    }

    public class RoadmapButtonTemplate
    {
        public enum QUIZ_STATUS
        {
            LOCKED = 1,
            COMPLETED = 2,
            INCOMPLETE = 3
        }

        public BaseQuiz Quiz { get; }
        public ICommand OpenQuizPreviewCommand { get; }

        public QUIZ_STATUS QuizStatus
        {
            get; set;
        }

        public RoadmapButtonTemplate(BaseQuiz quiz, ICommand openQuizPreviewCommand, QUIZ_STATUS status)
        {
            Quiz = quiz;
            OpenQuizPreviewCommand = openQuizPreviewCommand;
            QuizStatus = status;
        }
    }

}
