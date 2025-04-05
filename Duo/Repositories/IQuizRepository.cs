using System.Collections.Generic;
using System.Threading.Tasks;
using Duo.Models.Quizzes;

namespace Duo.Repositories
{
    public interface IQuizRepository
    {
        Task<List<Quiz>> GetAllAsync();
        Task<Quiz> GetByIdAsync(int quizId);
        Task<List<Quiz>> GetBySectionIdAsync(int sectionId);
        Task<List<Quiz>> GetUnassignedAsync();
        Task<int> AddAsync(Quiz quiz);
        Task UpdateAsync(Quiz quiz);
        Task DeleteAsync(int quizId);
        Task AddExerciseToQuiz(int quizId, int exerciseId);
        Task RemoveExerciseFromQuiz(int quizId, int exerciseId);
        Task UpdateQuizSection(int quizId, int? sectionId, int? orderNumber = null);
        Task<int> LastOrderNumberBySectionIdAsync(int sectionId);
        Task<int> CountBySectionIdAsync(int sectionId);
    }
}
