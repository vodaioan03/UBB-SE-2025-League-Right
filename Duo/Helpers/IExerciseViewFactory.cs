using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Duo.Helpers
{
    internal interface IExerciseViewFactory
    {
        object CreateExerciseView(string exerciseType);
    }
}
