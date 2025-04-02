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
            _exerciseRepository = exerciseRepository ?? throw new ArgumentNullException(nameof(exerciseRepository));
        }

        public async Task<List<Exercise>> GetAllExercises()
        {
            return await _exerciseRepository.GetAllExercisesAsync();
        }

        public async Task<Exercise> GetExerciseById(int exerciseId)
        {
            return await _exerciseRepository.GetByIdAsync(exerciseId);
        }

        public async Task<List<Exercise>> GetAllExercisesFromQuiz(int quizId)
        {
            return await _exerciseRepository.GetQuizExercisesAsync(quizId);
        }
        public async Task<List<Exercise>> GetAllExercisesFromExam(int examId)
        {
            return await _exerciseRepository.GetExamExercisesAsync(examId);
        }

        public async Task DeleteExercise(int exerciseId)
        {
            await _exerciseRepository.DeleteExerciseAsync(exerciseId);
        }

        public async Task CreateExercise(Exercise exercise)
        {
            ValidationHelper.ValidateGenericExercise(exercise);
            await _exerciseRepository.AddExerciseAsync(exercise);
        }
    }
}
