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
        Task<IExercise> GetById(int id);

        Task<List<IExercise>> GetAllExercises();

        Task<List<IExercise>> GetQuizExercises(int quizId);

        Task<bool> CreateExercise(IExercise exercise);

        Task<bool> UpdateExercise(IExercise exercise);

        Task<bool> DeleteExercise(int id);
    }
}
