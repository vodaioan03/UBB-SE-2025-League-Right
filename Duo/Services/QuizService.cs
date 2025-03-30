using Duo.Models.Quizzes;
using Duo.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duo.Services
{
    public class QuizService
    {
        private readonly QuizRepository _quizRepository;

        public QuizService(QuizRepository quizRepository)
        {
            _quizRepository = quizRepository;
        }

        public async Task<List<Quiz>> Get()
        {
            return (List<Quiz>)await _quizRepository.GetAllAsync();
        }

        public async Task<Quiz> GetExerciseById(int quizId)
        {
            return await _quizRepository.GetByIdAsync(quizId);
        }

        public async Task<List<Quiz>> GetAllExercisesFromQuiz(int sectionId)
        {
            return (List<Quiz>)await _quizRepository.GetBySectionIdAsync(sectionId);
        }

        public async Task DeleteQuiz(int quizId)
        {
            await _quizRepository.DeleteAsync(quizId);
        }

        public async Task UpdateQuiz(Quiz quiz)
        {
            await _quizRepository.UpdateAsync(quiz);
        }

        public Task CreateQuiz(Quiz quiz)
        {
            return _quizRepository.AddAsync(quiz);
        }

    }
}