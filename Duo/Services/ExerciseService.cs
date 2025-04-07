using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Duo.Models.Exercises;
using Duo.Models.Quizzes;
using Duo.Repositories;

namespace Duo.Services
{
    public class ExerciseService : IExerciseService
    {
        private readonly IExerciseRepository exerciseRepository;

        public ExerciseService(IExerciseRepository exerciseRepository)
        {
            this.exerciseRepository = exerciseRepository ?? throw new ArgumentNullException(nameof(exerciseRepository));
        }

        public async Task<List<Exercise>> GetAllExercises()
        {
            return await exerciseRepository.GetAllExercisesAsync();
        }

        public async Task<Exercise> GetExerciseById(int exerciseId)
        {
            return await exerciseRepository.GetByIdAsync(exerciseId);
        }

        public async Task<List<Exercise>> GetAllExercisesFromQuiz(int quizId)
        {
            return await exerciseRepository.GetQuizExercisesAsync(quizId);
        }
        public async Task<List<Exercise>> GetAllExercisesFromExam(int examId)
        {
            return await exerciseRepository.GetExamExercisesAsync(examId);
        }

        public async Task DeleteExercise(int exerciseId)
        {
            await exerciseRepository.DeleteExerciseAsync(exerciseId);
        }

        public async Task CreateExercise(Exercise exercise)
        {
            ValidationHelper.ValidateGenericExercise(exercise);
            await exerciseRepository.AddExerciseAsync(exercise);
        }
    }
}
