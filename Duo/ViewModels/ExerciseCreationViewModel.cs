using Duo.Services;
using Duo.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Duo.ViewModels
{
    class ExerciseCreationViewModel : ViewModelBase
    {
        private readonly ExerciseService _exerciseService;
        public ExerciseCreationViewModel(ExerciseService exerciseService) 
        {
            _exerciseService = exerciseService;
        }



    }
}
