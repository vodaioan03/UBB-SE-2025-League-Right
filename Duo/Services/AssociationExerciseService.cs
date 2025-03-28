using Duo.Models.Exercises;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Duo.Services
{
    class AssociationExerciseService
    {
        public AssociationExerciseService() { }
        public bool CreateExercise(string question, List<string> firstAnswersList, List<string> secondAnswersList)
        {
            return false;
        }

        public bool VerifyIfAnswerIsCorrect(AssociationExercise exercise)
        {
            return false;
        }

        public List<string> RandomizeListOfAnswers(List<string> answersList)
        {
            return new List<string>();
        }
    }
}
