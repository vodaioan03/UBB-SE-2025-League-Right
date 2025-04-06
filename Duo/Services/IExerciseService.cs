using System.Collections.Generic;
using System.Threading.Tasks;
using Duo.Models.Exercises;

namespace Duo.Services
{
    public interface IExerciseService
    {
        Task CreateExercise(Exercise exercise);
        Task DeleteExercise(int exerciseId);
        Task<List<Exercise>> GetAllExercises();
        Task<List<Exercise>> GetAllExercisesFromExam(int examId);
        Task<List<Exercise>> GetAllExercisesFromQuiz(int quizId);
        Task<Exercise> GetExerciseById(int exerciseId);
    }
}