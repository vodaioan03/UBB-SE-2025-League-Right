using Duo.Commands;
using Duo.Models;
using Duo.Models.Exercises;
using Duo.Models.Quizzes;
using Duo.Services;
using Duo.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Duo.ViewModels
{
    internal class ManageQuizesViewModel: ViewModelBase
    {

        private readonly QuizService _quizService;
        public ObservableCollection<Quiz> Quizes { get; set; } = new ObservableCollection<Quiz>();

        public ManageQuizesViewModel()
        {
            try
            {
                _quizService = (QuizService)App.serviceProvider.GetService(typeof(QuizService));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            DeleteQuizCommand = new RelayCommandWithParameter<Quiz>(DeleteQuiz);

            initializeViewModel();
        }
        public ICommand DeleteQuizCommand { get; }

        public void DeleteQuiz(Quiz quizToBeDeleted)
        {
            Debug.WriteLine("Deleting quiz...");
            Quizes.Remove(quizToBeDeleted);
        }

        public void initializeViewModel()
        {
            Quizes.Add(new Quiz(0, -1, -1));
            Quizes.Add(new Quiz(1, -1, -1));
            Quizes.Add(new Quiz(2, -1, -1));
        }

    }
}
