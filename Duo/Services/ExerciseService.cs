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

        public Task<List<IExercise>> GetAllExercises()
        {
            return _exerciseRepository.GetAllExercises();
        }

        public Task<IExercise> GetExerciseById(int exerciseId)
        {
            return _exerciseRepository.GetById(exerciseId);
        }

        public Task<List<IExercise>> GetAllExercisesFromQuiz(int quizId)
        {
            return _exerciseRepository.GetQuizExercises(quizId);
        }

        public Task<bool> DeleteExercise(int exerciseId)
        {
            return _exerciseRepository.DeleteExercise(exerciseId);
        }

        public Task<bool> UpdateExercise(IExercise exercise)
        {
            return _exerciseRepository.UpdateExercise(exercise);
        }

    }
}
