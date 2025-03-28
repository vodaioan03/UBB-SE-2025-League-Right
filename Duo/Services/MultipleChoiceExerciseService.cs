using Duo.Models.Exercises;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Duo.Services
{
    class MultipleChoiceExerciseService
    {
        public MultipleChoiceExerciseService() { }

        public bool CreateExercise(string question, List<string> possibleCorrectAnswers, string correctAnswer)
        {
            return false;
        }

        public bool VerifyIfAnswerIsCorrect(MultipleChoiceExercise exercise, List<string> userAnswer)
        {
            return exercise.ValidateAnswer(userAnswer);
        }
    }
}
