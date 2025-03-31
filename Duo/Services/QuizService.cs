using Duo.Models.Quizzes;
using Duo.Repositories;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Duo.Services
{
    public class QuizService
    {
        private readonly QuizRepository _quizRepository;
        private readonly ExamRepository _examRepository;

        public QuizService(QuizRepository quizRepository, ExamRepository examRepository)
        {
            _quizRepository = quizRepository;
            _examRepository = examRepository;
        }

        public async Task<List<Quiz>> Get()
        {
            return (List<Quiz>)await _quizRepository.GetAllAsync();
        }

        public async Task<Quiz> GetQuizById(int quizId)
        {
            return await _quizRepository.GetByIdAsync(quizId);
        }

        public async Task<Exam> GetExamById(int examId)
        {
            return await _examRepository.GetByIdAsync(examId);
        }

        public async Task<List<Quiz>> GetAllQuizzesFromSection(int sectionId)
        {
            return (List<Quiz>)await _quizRepository.GetBySectionIdAsync(sectionId);
        }

        public async Task<Exam> GetExamFromSection(int sectionId)
        {
            return await _examRepository.GetBySectionIdAsync(sectionId);
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

        public async Task DeleteExam(int examId)
        {
            await _examRepository.DeleteAsync(examId);
        }

        public async Task UpdateExam(Exam exam)
        {
            await _examRepository.UpdateAsync(exam);
        }

        public Task CreateExam(Exam exam)
        {
            return _examRepository.AddAsync(exam);
        }

    }
}