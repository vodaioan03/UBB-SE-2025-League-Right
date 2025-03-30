using Duo.Models;
using Duo.Models.Exercises;
using Duo.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Duo.ViewModels.ExerciseViewModels
{
    abstract class CreateExerciseViewModelBase: ViewModelBase
    {
        public abstract Exercise CreateExercise(string question,Difficulty difficulty);
    }
}
