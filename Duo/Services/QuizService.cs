using System.Collections.Generic;
using System.Threading.Tasks;
using Duo.Models.Exercises;
using Duo.Models.Quizzes;
using Duo.Repositories;

namespace Duo.Services
{
    public class QuizService : IQuizService
    {
        private readonly IQuizRepository quizRepository;
        private readonly IExamRepository examRepository;

        public QuizService(IQuizRepository quizRepository, IExamRepository examRepository)
        {
            this.quizRepository = quizRepository;
            this.examRepository = examRepository;
        }

        public async Task<List<Quiz>> Get()
        {
            return await quizRepository.GetAllAsync();
        }

        public async Task<List<Exam>> GetAllAvailableExams()
        {
            return (List<Exam>)await examRepository.GetUnassignedAsync();
        }

        public async Task<Quiz> GetQuizById(int quizId)
        {
            return await quizRepository.GetByIdAsync(quizId);
        }

        public async Task<Exam> GetExamById(int examId)
        {
            return await examRepository.GetByIdAsync(examId);
        }

        public async Task<List<Quiz>> GetAllQuizzesFromSection(int sectionId)
        {
            return await quizRepository.GetBySectionIdAsync(sectionId);
        }

        public async Task<int> CountQuizzesFromSection(int sectionId)
        {
            return await quizRepository.CountBySectionIdAsync(sectionId);
        }

        public async Task<int> LastOrderNumberFromSection(int sectionId)
        {
            return await quizRepository.LastOrderNumberBySectionIdAsync(sectionId);
        }

        public async Task<Exam?> GetExamFromSection(int sectionId)
        {
            return await examRepository.GetBySectionIdAsync(sectionId);
        }

        public async Task DeleteQuiz(int quizId)
        {
            await quizRepository.DeleteAsync(quizId);
        }

        public async Task UpdateQuiz(Quiz quiz)
        {
            ValidationHelper.ValidateQuiz(quiz);
            await quizRepository.UpdateAsync(quiz);
        }

        public Task<int> CreateQuiz(Quiz quiz)
        {
            ValidationHelper.ValidateQuiz(quiz);
            return quizRepository.AddAsync(quiz);
        }

        public async Task AddExercisesToQuiz(int quizId, List<Exercise> exercises)
        {
            foreach (Exercise exercise in exercises)
            {
                await quizRepository.AddExerciseToQuiz(quizId, exercise.Id);
            }
        }

        public Task AddExerciseToQuiz(int quizId, int exerciseId)
        {
            return quizRepository.AddExerciseToQuiz(quizId, exerciseId);
        }

        public Task RemoveExerciseFromQuiz(int quizId, int exerciseId)
        {
            return quizRepository.RemoveExerciseFromQuiz(quizId, exerciseId);
        }

        public async Task DeleteExam(int examId)
        {
            await examRepository.DeleteAsync(examId);
        }

        public async Task UpdateExam(Exam exam)
        {
            ValidationHelper.ValidateExam(exam);
            await examRepository.UpdateAsync(exam);
        }

        public Task<int> CreateExam(Exam exam)
        {
            ValidationHelper.ValidateExam(exam);
            return examRepository.AddAsync(exam);
        }
    }
}