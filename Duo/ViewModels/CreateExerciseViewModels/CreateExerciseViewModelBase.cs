using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Duo.Models;
using Duo.Models.Exercises;
using Duo.ViewModels.Base;

namespace Duo.ViewModels.ExerciseViewModels
{
    internal abstract class CreateExerciseViewModelBase : ViewModelBase
    {
        public abstract Exercise CreateExercise(string question, Difficulty difficulty);
    }
}
