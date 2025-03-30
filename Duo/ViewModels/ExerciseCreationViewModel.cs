using Duo.Commands;
using Duo.Services;
using Duo.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Duo.ViewModels
{
    class ExerciseCreationViewModel : ViewModelBase
    {
        private readonly ExerciseService _exerciseService;

        private string _questionText = string.Empty;

        public ExerciseCreationViewModel(ExerciseService exerciseService) 
        {
            _exerciseService = exerciseService;
        }

        public ExerciseCreationViewModel()
        {
            try
            {
                _exerciseService = (ExerciseService)App.serviceProvider.GetService(typeof(ExerciseService));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            SaveButtonCommand = new RelayCommand(CreateExercise);
        }


        public string QuestionText
        {
            get => _questionText;
            set
            {
                if (_questionText != value)
                {
                    _questionText = value;
                    OnPropertyChanged(nameof(QuestionText)); // Notify UI
                }
            }
        }

        public ICommand SaveButtonCommand { get; }

        public void CreateExercise()
        {
            Debug.WriteLine(QuestionText);
        }


    }
}
