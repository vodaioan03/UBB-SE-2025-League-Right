using Duo.Models.Exercises;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Duo.Services
{
    class FIllInTheBlankExerciseService
    {
        FIllInTheBlankExerciseService() { }

        public bool CreateExercise(string sentence, List<string> possibleAnswers)
        {
            return false;
        }
        public bool VerifyIfAnswerIsCorrect(FillInTheBlankExercise exercise,List<string> userAnswers)
        {
            return exercise.ValidateAnswer(userAnswers);
        }
    }
}
