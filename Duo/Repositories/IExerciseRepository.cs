using System.Collections.Generic;
using System.Threading.Tasks;
using Duo.Models.Exercises;

namespace Duo.Repositories
{
    public interface IExerciseRepository
    {
        Task<List<Exercise>> GetAllExercisesAsync();
        Task<Exercise> GetByIdAsync(int id);
        Task<List<Exercise>> GetQuizExercisesAsync(int quizId);
        Task<List<Exercise>> GetExamExercisesAsync(int examId);
        Task<int> AddExerciseAsync(Exercise exercise);
        Task DeleteExerciseAsync(int id);
    }
}
