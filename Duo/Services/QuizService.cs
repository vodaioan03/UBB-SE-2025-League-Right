using Duo.Models.Exercises;
using Duo.Models.Quizzes;
using Duo.Repositories;
using System.Collections.Generic;
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
            return await _quizRepository.GetAllAsync();
        }

        public async Task<List<Exam>> GetAllAvailableExams()
        {
            return (List<Exam>)await _examRepository.GetUnassignedAsync();
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
            return await _quizRepository.GetBySectionIdAsync(sectionId);
        }

        public async Task<int> CountQuizzesFromSection(int sectionId)
        {
            return await _quizRepository.CountBySectionIdAsync(sectionId);
        }

        public async Task<int> LastOrderNumberFromSection(int sectionId)
        {
            return await _quizRepository.LastOrderNumberBySectionIdAsync(sectionId);
        }

        public async Task<Exam?> GetExamFromSection(int sectionId)
        {
            return await _examRepository.GetBySectionIdAsync(sectionId);
        }

        public async Task DeleteQuiz(int quizId)
        {
            await _quizRepository.DeleteAsync(quizId);
        }

        public async Task UpdateQuiz(Quiz quiz)
        {
            ValidationHelper.ValidateQuiz(quiz);
            await _quizRepository.UpdateAsync(quiz);
        }

        public Task<int> CreateQuiz(Quiz quiz)
        {
            ValidationHelper.ValidateQuiz(quiz);
            return _quizRepository.AddAsync(quiz);
        }

        public async Task AddExercisesToQuiz(int quizId, List<Exercise> exercises)
        {
            foreach (Exercise exercise in exercises)
            {
                await _quizRepository.AddExerciseToQuiz(quizId, exercise.Id);
            }
        }

        public Task AddExerciseToQuiz(int quizId, int exerciseId)
        {
            return _quizRepository.AddExerciseToQuiz(quizId, exerciseId);
        }

        public Task RemoveExerciseFromQuiz(int quizId, int exerciseId)
        {
            return _quizRepository.RemoveExerciseFromQuiz(quizId, exerciseId);
        }

        public async Task DeleteExam(int examId)
        {
            await _examRepository.DeleteAsync(examId);
        }

        public async Task UpdateExam(Exam exam)
        {
            ValidationHelper.ValidateExam(exam);
            await _examRepository.UpdateAsync(exam);
        }

        public Task<int> CreateExam(Exam exam)
        {
            ValidationHelper.ValidateExam(exam);
            return _examRepository.AddAsync(exam);
        }

    }
}