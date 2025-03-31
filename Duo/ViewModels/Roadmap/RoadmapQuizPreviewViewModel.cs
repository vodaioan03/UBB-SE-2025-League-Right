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

namespace Duo.ViewModels.Roadmap
{
    public class RoadmapQuizPreviewViewModel : ViewModelBase
    {
        private BaseQuiz _quiz;
        private Section _section;
        private bool _isPreviewVisible;
        private readonly QuizService _quizService;
        private readonly SectionService _sectionService;

        public bool IsPreviewVisible
        { get => _isPreviewVisible; }

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
        }

        public string SectionTitle
        {
            get => _section?.Title ?? "Unknown Section";
        }
        public ICommand StartQuizCommand { get; }

        public RoadmapQuizPreviewViewModel(QuizService quizService, SectionService sectionService, ICommand startQuizCommand)
        {
            _quizService = quizService;
            _sectionService = sectionService;
            _isPreviewVisible = false;

            StartQuizCommand = startQuizCommand;
        }

        public async Task OpenForQuiz(BaseQuiz quiz, Section section)
        {
            _quiz = quiz;
            _section = section;
            _isPreviewVisible = true;

            OnPropertyChanged(nameof(QuizOrderNumber));
            OnPropertyChanged(nameof(SectionTitle));
            OnPropertyChanged(nameof(IsPreviewVisible));
        }

    }
}
