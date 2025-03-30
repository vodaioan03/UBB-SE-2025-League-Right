using Duo.Models.Exercises;
using Duo.Models.Quizzes;
using Duo.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Duo.Services
{
    public class ExerciseService
    {
        private readonly ExerciseRepository _exerciseRepository;

        public ExerciseService() {
            //_exerciseRepository = new ExerciseRepository();
        }

        public Task<List<Exercise>> GetAllExercises()
        {
            return _exerciseRepository.GetAllExercises();
        }

        public Task<Exercise> GetExerciseById(int exerciseId)
        {
            return _exerciseRepository.GetById(exerciseId);
        }

        public Task<List<Exercise>> GetAllExercisesFromQuiz(int quizId)
        {
            return _exerciseRepository.GetQuizExercises(quizId);
        }

        public Task<bool> DeleteExercise(int exerciseId)
        {
            return _exerciseRepository.DeleteExercise(exerciseId);
        }

        public Task<bool> UpdateExercise(Exercise exercise)
        {
            return _exerciseRepository.UpdateExercise(exercise);
        }

        public Task<bool> CreateExercise(IExercise exercise)
        {
            return _exerciseRepository.CreateExercise(exercise);
        }

    }
}
