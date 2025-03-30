using Duo.Models.Exercises;
using Duo.Models.Quizzes;
using Duo.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Duo.Services
{
    public class ExerciseService
    {
        private readonly ExerciseRepository _exerciseRepository;

        public ExerciseService(ExerciseRepository exerciseRepository) {
            _exerciseRepository = exerciseRepository;
        {
            _exerciseRepository = exerciseRepository ?? throw new ArgumentNullException(nameof(exerciseRepository));
        }

        public async Task<List<Exercise>> GetAllExercises()
        {
            var exercises = await _exerciseRepository.GetAllExercisesAsync();
            return exercises.ToList();
        }

        public async Task<Exercise> GetExerciseById(int exerciseId)
        {
            return await _exerciseRepository.GetByIdAsync(exerciseId);
        }

        public async Task<List<Exercise>> GetAllExercisesFromQuiz(int quizId)
        {
            var exercises = await _exerciseRepository.GetQuizExercisesAsync(quizId);
            return exercises.ToList();
        }

        public async Task DeleteExercise(int exerciseId)
        {
            await _exerciseRepository.DeleteExerciseAsync(exerciseId);
        }

        public async Task<int> CreateExercise(Exercise exercise)
        {
            return await _exerciseRepository.AddExerciseAsync(exercise);
        }
    }
}
