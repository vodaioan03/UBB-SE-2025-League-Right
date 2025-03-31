using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Duo.Models.Quizzes;
using Duo.Models.Sections;
using Duo.ViewModels.Base;

namespace Duo.ViewModels.Roadmap
{
    public class RoadmapSectionViewModel : ViewModelBase
    {
        private int _sectionId;
        private Section _section = new Section
        {
            Id = 1,
            SubjectId = 1,
            Title = "Countries",
            Description = "This is the first section",
            RoadmapId = 1,
            OrderNumber = 1,
            Quizzes = new List<Quiz>
            {
            new Quiz(1, null, null),
            new Quiz(2, null, null),
            new Quiz(3, null, null),
            new Quiz(4, null, null),
            new Quiz(5, null, null),
            new Quiz(6, null, null),
            new Quiz(7, null, null)
            },
            Exam = new Exam(2, null)
        };

        // TEMPORARY DATA
        private ObservableCollection<Quiz> _quizzes = new ObservableCollection<Quiz>
        {
            new Quiz(1, null, null),
            new Quiz(2, null, null),
            new Quiz(3, null, null),
            new Quiz(4, null, null),
            new Quiz(5, null, null),
            new Quiz(6, null, null),
            new Quiz(7, null, null)
        };
        private Exam _exam = new Exam(2, null);


        public ICommand OpenQuizPreviewCommand { get; }
        public RoadmapSectionViewModel(int sectionId, ICommand openQuizPreviewCommand)
        {
            _sectionId = sectionId; 
            OpenQuizPreviewCommand = openQuizPreviewCommand;

            _quizButtonTemplates = new ObservableCollection<RoadmapButtonTemplate>();
            foreach (Quiz quiz in _quizzes)
            {
                _quizButtonTemplates.Add(new RoadmapButtonTemplate(quiz, OpenQuizPreviewCommand));
            }
            _examButtonTemplate = new RoadmapButtonTemplate(_exam, OpenQuizPreviewCommand);
        }

        private ObservableCollection<RoadmapButtonTemplate> _quizButtonTemplates;

        public ObservableCollection<RoadmapButtonTemplate> QuizButtonTemplates
        {
            get => _quizButtonTemplates;
        }

        private RoadmapButtonTemplate _examButtonTemplate;

        public RoadmapButtonTemplate ExamButtonTemplate
        {
            get => _examButtonTemplate;
        }
    }

    public class RoadmapButtonTemplate
    {
        public BaseQuiz Quiz;
        public ICommand OpenQuizPreviewCommand;

        public RoadmapButtonTemplate(BaseQuiz quiz, ICommand openQuizPreviewCommand)
        {
            this.Quiz = quiz;
            this.OpenQuizPreviewCommand = openQuizPreviewCommand;
        }
    }
}
