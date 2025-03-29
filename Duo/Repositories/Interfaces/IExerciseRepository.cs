using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Duo.Data;
using Duo.Models.Exercises;

namespace Duo.Repositories.Interfaces
{
    internal interface IExerciseRepository
    {
        Task<Exercise> GetById(int id);

        Task<List<Exercise>> GetAllExercises();

        Task<List<Exercise>> GetQuizExercises(int quizId);

        Task<bool> CreateExercise(IExercise exercise);
        
        Task<bool> UpdateExercise(IExercise exercise);

        Task<bool> DeleteExercise(int id);
    }
}
