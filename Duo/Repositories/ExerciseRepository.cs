using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Duo.Models.Exercises;

namespace Duo.Repositories
{
    internal class ExerciseRepository
    {
        public Task<bool> CreateExercise(IExercise exercise)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteExercise(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<IExercise>> GetAllExercises()
        {
            throw new NotImplementedException();
        }

        public Task<IExercise> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<IExercise>> GetQuizExercises(int quizId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateExercise(IExercise exercise)
        {
            throw new NotImplementedException();
        }
    }
}
