using System.Collections.Generic;
using System.Threading.Tasks;
using Duo.Models.Quizzes;

namespace Duo.Repositories
{
    public interface IExamRepository
    {
        Task<List<Exam>> GetAllAsync();
        Task<Exam> GetByIdAsync(int examId);
        Task<Exam?> GetBySectionIdAsync(int sectionId);
        Task<List<Exam>> GetUnassignedAsync();
        Task<int> AddAsync(Exam exam);
        Task UpdateAsync(Exam exam);
        Task DeleteAsync(int examId);
        Task AddExerciseToExam(int examId, int exerciseId);
        Task RemoveExerciseFromExam(int examId, int exerciseId);
        Task UpdateExamSection(int examId, int? sectionId);
        IReadOnlyCollection<int> GetExercisesForExam(int examId);
    }
}
