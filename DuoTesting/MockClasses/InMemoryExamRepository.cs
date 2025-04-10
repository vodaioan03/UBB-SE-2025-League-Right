using Duo.Models.Quizzes;
using Duo.Repositories;
using System.Linq;

namespace DuoTesting.MockClasses
{
    public class InMemoryExamRepository : IExamRepository
    {
        private readonly List<Exam> _inMemoryExams = new();
        private readonly Dictionary<int, HashSet<int>> _examExercises = new();
        private int _nextId = 1;

        public Task<List<Exam>> GetAllAsync()
            => Task.FromResult(_inMemoryExams.ToList());

        public Task<Exam> GetByIdAsync(int examId)
        {
            var exam = _inMemoryExams.FirstOrDefault(e => e.Id == examId);
            if (exam == null)
                throw new KeyNotFoundException($"Exam with ID {examId} not found.");
            return Task.FromResult(exam);
        }

        public Task<Exam?> GetBySectionIdAsync(int sectionId)
            => Task.FromResult(_inMemoryExams.FirstOrDefault(e => e.SectionId == sectionId));

        public Task<List<Exam>> GetUnassignedAsync()
            => Task.FromResult(_inMemoryExams.Where(e => e.SectionId == null).ToList());

        public Task<int> AddAsync(Exam exam)
        {
            var newExam = new Exam(_nextId++, exam.SectionId);
            _inMemoryExams.Add(newExam);
            _examExercises[newExam.Id] = new HashSet<int>();
            return Task.FromResult(newExam.Id);
        }

        public Task UpdateAsync(Exam exam)
        {
            var index = _inMemoryExams.FindIndex(e => e.Id == exam.Id);
            if (index == -1)
                throw new KeyNotFoundException($"Exam with ID {exam.Id} not found.");

            _inMemoryExams[index] = exam;
            return Task.CompletedTask;
        }

        public Task DeleteAsync(int examId)
        {
            var exam = _inMemoryExams.FirstOrDefault(e => e.Id == examId);
            if (exam == null)
                throw new KeyNotFoundException($"Exam with ID {examId} not found.");

            _inMemoryExams.Remove(exam);
            _examExercises.Remove(examId);
            return Task.CompletedTask;
        }

        public Task AddExerciseToExam(int examId, int exerciseId)
        {
            if (!_examExercises.ContainsKey(examId))
                throw new KeyNotFoundException($"Exam with ID {examId} not found.");

            _examExercises[examId].Add(exerciseId);
            return Task.CompletedTask;
        }

        public Task RemoveExerciseFromExam(int examId, int exerciseId)
        {
            if (!_examExercises.ContainsKey(examId))
                throw new KeyNotFoundException($"Exam with ID {examId} not found.");

            _examExercises[examId].Remove(exerciseId);
            return Task.CompletedTask;
        }

        public Task UpdateExamSection(int examId, int? sectionId)
        {
            var index = _inMemoryExams.FindIndex(e => e.Id == examId);
            if (index == -1)
                throw new KeyNotFoundException($"Exam with ID {examId} not found.");

            var updated = new Exam(examId, sectionId);
            _inMemoryExams[index] = updated;
            return Task.CompletedTask;
        }

        public void ClearAll()
        {
            _inMemoryExams.Clear();
            _examExercises.Clear();
            _nextId = 1;
        }

        public IReadOnlyCollection<int> GetExercisesForExam(int examId)
        {
            if (!_examExercises.ContainsKey(examId))
                throw new KeyNotFoundException($"Exam with ID {examId} not found.");
            return _examExercises[examId].ToList();
        }
    }
}
