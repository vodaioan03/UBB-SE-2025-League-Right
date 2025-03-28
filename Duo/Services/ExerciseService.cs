using Duo.Models.Exercises;
using Duo.Models.Quizzes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Duo.Services
{
    class ExerciseService
    {

        public ExerciseService() {
        
        }

        public List<IExercise> GetAllExercises()
        {
            return new List<IExercise>();
        }

        public IExercise GetExerciseById(int exerciseId)
        {
            return null;
        }

        public List<IExercise> GetAllExercisesFromQuiz(int quizId)
        {
            return new List<IExercise>();
        }

        public bool DeleteExercise(int exerciseId)
        {
            return false;
        }

    }
}
