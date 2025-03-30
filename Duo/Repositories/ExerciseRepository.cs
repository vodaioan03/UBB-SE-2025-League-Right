using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Duo.Models.Exercises;

namespace Duo.Repositories
{
    public class ExerciseRepository
    {
        public Task<bool> CreateExercise(Exercise exercise)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteExercise(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Exercise>> GetAllExercises()
        {
            throw new NotImplementedException();
        }

        public Task<Exercise> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Exercise>> GetQuizExercises(int quizId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateExercise(Exercise exercise)
        {
            throw new NotImplementedException();
        }
    }
}
