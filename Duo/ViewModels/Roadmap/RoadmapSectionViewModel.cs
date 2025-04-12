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
        private ISectionService sectionService;

        private int sectionId;
        private Section section;
        public Section Section
        {
            get => section;
        }

        private bool isCompleted;
        private int nrOfCompletedQuizzes;

        // TEMPORARY DATA
        // private ObservableCollection<Quiz> _quizzes = new ObservableCollection<Quiz>
        // {
        //    new Quiz(1, null, null),
        //    new Quiz(2, null, null),
        //    new Quiz(3, null, null),
        //    new Quiz(4, null, null),
        //    new Quiz(5, null, null),
        //    new Quiz(6, null, null),
        //    new Quiz(7, null, null)
        // };
        // private Exam _exam = new Exam(2, null);
        public ICommand OpenQuizPreviewCommand { get; set; }

        private ObservableCollection<RoadmapButtonTemplate> quizButtonTemplates;

        public ObservableCollection<RoadmapButtonTemplate> QuizButtonTemplates
        {
            get => quizButtonTemplates;
            private set
            {
                quizButtonTemplates = value;
                OnPropertyChanged(nameof(QuizButtonTemplates));
            }
        }

        private RoadmapButtonTemplate examButtonTemplate;

        public RoadmapButtonTemplate ExamButtonTemplate
        {
            get => examButtonTemplate;
            private set
            {
                examButtonTemplate = value;
                OnPropertyChanged(nameof(ExamButtonTemplate));
            }
        }

        public RoadmapSectionViewModel()
        {
            sectionService = (ISectionService)(App.ServiceProvider.GetService(typeof(ISectionService)));

            var mainPageViewModel = (RoadmapMainPageViewModel)(App.ServiceProvider.GetService(typeof(RoadmapMainPageViewModel)));
            OpenQuizPreviewCommand = mainPageViewModel.OpenQuizPreviewCommand;
            quizButtonTemplates = new ObservableCollection<RoadmapButtonTemplate>();
        }

        public async Task SetupForSection(int sectionId, bool isCompleted, int nrOfCompletedQuizzes)
        {
            Debug.WriteLine($"--------------------Setting up section with ID {sectionId}, Compl {isCompleted}, nr_quiz {nrOfCompletedQuizzes}");
            this.sectionId = sectionId;
            section = await sectionService.GetSectionById(this.sectionId);

            IQuizService quizService = (IQuizService)App.ServiceProvider.GetService(typeof(IQuizService));
            section.Quizzes = await quizService.GetAllQuizzesFromSection(this.sectionId);
            section.Exam = await quizService.GetExamFromSection(this.sectionId);

            this.isCompleted = isCompleted;
            this.nrOfCompletedQuizzes = nrOfCompletedQuizzes;

            OnPropertyChanged(nameof(Section));

            PopulateQuizButtonTemplates();
        }

        private void PopulateQuizButtonTemplates()
        {
            Debug.WriteLine($"++++++ Populating section {section.Id}");
            foreach (Quiz quiz in section.GetAllQuizzes())
            {
                RoadmapButtonTemplate.QUIZ_STATUS quizStatus = RoadmapButtonTemplate.QUIZ_STATUS.LOCKED;
                if (isCompleted)
                {
                    quizStatus = RoadmapButtonTemplate.QUIZ_STATUS.COMPLETED;
                }
                else if (quiz.OrderNumber <= nrOfCompletedQuizzes)
                {
                    quizStatus = RoadmapButtonTemplate.QUIZ_STATUS.COMPLETED;
                }
                else if (quiz.OrderNumber == nrOfCompletedQuizzes + 1)
                {
                    quizStatus = RoadmapButtonTemplate.QUIZ_STATUS.INCOMPLETE;
                }

                Debug.WriteLine($"++++++++++ Populating quiz {quiz.Id} -> {quizStatus}");
                quizButtonTemplates.Add(new RoadmapButtonTemplate(quiz, OpenQuizPreviewCommand, quizStatus));
                // Debug.WriteLine($"Added quiz with ID {quiz.Id} in section {_section.Id}");
            }
            RoadmapButtonTemplate.QUIZ_STATUS examStatus = RoadmapButtonTemplate.QUIZ_STATUS.LOCKED;
            if (isCompleted)
            {
                examStatus = RoadmapButtonTemplate.QUIZ_STATUS.COMPLETED;
            }
            else if (section.GetAllQuizzes().Count<Quiz>() == nrOfCompletedQuizzes)
            {
                examStatus = RoadmapButtonTemplate.QUIZ_STATUS.INCOMPLETE;
            }
            Debug.WriteLine($"++++++++++ Populating exam -> {examStatus}");
            examButtonTemplate = new RoadmapButtonTemplate(section.GetFinalExam(), OpenQuizPreviewCommand, examStatus);

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
