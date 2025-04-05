using Duo.Models.Exercises;
using Duo.Models.Quizzes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duo.Services
{
    public interface IQuizService
    {
        Task AddExercisesToQuiz(int quizId, List<Exercise> exercises);
        Task AddExerciseToQuiz(int quizId, int exerciseId);
        Task<int> CountQuizzesFromSection(int sectionId);
        Task<int> CreateExam(Exam exam);
        Task<int> CreateQuiz(Quiz quiz);
        Task DeleteExam(int examId);
        Task DeleteQuiz(int quizId);
        Task<List<Quiz>> Get();
        Task<List<Exam>> GetAllAvailableExams();
        Task<List<Quiz>> GetAllQuizzesFromSection(int sectionId);
        Task<Exam> GetExamById(int examId);
        Task<Exam?> GetExamFromSection(int sectionId);
        Task<Quiz> GetQuizById(int quizId);
        Task<int> LastOrderNumberFromSection(int sectionId);
        Task RemoveExerciseFromQuiz(int quizId, int exerciseId);
        Task UpdateExam(Exam exam);
        Task UpdateQuiz(Quiz quiz);
    }
}