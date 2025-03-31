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
using Duo.ViewModels.Base;

namespace Duo.ViewModels.Roadmap
{
    public class RoadmapSectionViewModel : ViewModelBase
    {
        private int _sectionId;
        //private Section _section;

        //public Section Section
        //{
        //    get => _section;
        //}

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


        public ICommand OpenQuizPreviewCommand { get; set; }


        public RoadmapSectionViewModel()
        {
            var mainPageViewModel = (RoadmapMainPageViewModel)(App.serviceProvider.GetService(typeof(RoadmapMainPageViewModel)));
            OpenQuizPreviewCommand = mainPageViewModel.OpenQuizPreviewCommand;
            _quizButtonTemplates = new ObservableCollection<RoadmapButtonTemplate>();

            PopulateQuizButtonTemplates();
        }

        private void PopulateQuizButtonTemplates()
        {
            foreach (Quiz quiz in _quizzes)
            {
                _quizButtonTemplates.Add(new RoadmapButtonTemplate(quiz, OpenQuizPreviewCommand));
            }
            _examButtonTemplate = new RoadmapButtonTemplate(_exam, OpenQuizPreviewCommand);

            // Notify UI that properties have changed
            OnPropertyChanged(nameof(QuizButtonTemplates));
            OnPropertyChanged(nameof(ExamButtonTemplate));
        }

        public void SetSection(int sectionId)
        {
            _sectionId = sectionId;
            // TODO: Implement logic to get the section from the database
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
