using Duo.Models.Exercises;
using Duo.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Duo.Services
{
    class MultipleChoiceExerciseService : ExerciseService
    {
        // TODO: Move functionality up the architecture
        public MultipleChoiceExerciseService() {
            //_exerciseRepository = new IExerciseRepository();
        }


        public bool VerifyIfAnswerIsCorrect(MultipleChoiceExercise exercise, List<string> userAnswer)
        {
            return exercise.ValidateAnswer(userAnswer);
        }
    }
}
