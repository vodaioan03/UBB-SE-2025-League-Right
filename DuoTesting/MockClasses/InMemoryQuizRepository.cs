using Duo.Models.Quizzes;
using Duo.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DuoTesting.MockClasses
{
    public class InMemoryQuizRepository : IQuizRepository
    {
        private readonly List<Quiz> _quizzes = new();
        private readonly Dictionary<int, HashSet<int>> _quizExercises = new();
        private int _nextId = 1;

        public Task<int> AddAsync(Quiz quiz)
        {
            if (quiz.SectionId.HasValue && quiz.SectionId < 0)
                throw new ArgumentException("Invalid section ID.");

            var newQuiz = new Quiz(_nextId++, quiz.SectionId, quiz.OrderNumber);
            _quizzes.Add(newQuiz);
            _quizExercises[newQuiz.Id] = new HashSet<int>();
            return Task.FromResult(newQuiz.Id);
        }

        public Task<Quiz> GetByIdAsync(int id)
        {
            var quiz = _quizzes.FirstOrDefault(q => q.Id == id);
            if (quiz == null) throw new KeyNotFoundException($"Quiz {id} not found.");
            return Task.FromResult(quiz);
        }

        public Task<List<Quiz>> GetAllAsync()
            => Task.FromResult(_quizzes.ToList());

        public Task<List<Quiz>> GetUnassignedAsync()
            => Task.FromResult(_quizzes.Where(q => q.SectionId == null).ToList());

        public Task<int> CountBySectionIdAsync(int sectionId)
            => Task.FromResult(_quizzes.Count(q => q.SectionId == sectionId));

        public Task<int> LastOrderNumberBySectionIdAsync(int sectionId)
        {
            var orders = _quizzes
                .Where(q => q.SectionId == sectionId && q.OrderNumber.HasValue)
                .Select(q => q.OrderNumber!.Value);

            return Task.FromResult(orders.Any() ? orders.Max() : 0);
        }

        public Task UpdateAsync(Quiz quiz)
        {
            if (quiz.Id <= 0) throw new ArgumentException("Invalid quiz ID.");
            var index = _quizzes.FindIndex(q => q.Id == quiz.Id);
            if (index == -1) throw new KeyNotFoundException();
            _quizzes[index] = quiz;
            return Task.CompletedTask;
        }

        public Task DeleteAsync(int quizId)
        {
            if (quizId <= 0) throw new ArgumentException();
            _quizzes.RemoveAll(q => q.Id == quizId);
            _quizExercises.Remove(quizId);
            return Task.CompletedTask;
        }

        public Task AddExerciseToQuiz(int quizId, int exerciseId)
        {
            if (!_quizExercises.ContainsKey(quizId)) throw new ArgumentException("Invalid quiz ID.");
            _quizExercises[quizId].Add(exerciseId);
            return Task.CompletedTask;
        }

        public Task RemoveExerciseFromQuiz(int quizId, int exerciseId)
        {
            if (_quizExercises.TryGetValue(quizId, out var set))
                set.Remove(exerciseId);
            return Task.CompletedTask;
        }

        public Task UpdateQuizSection(int quizId, int? sectionId, int? orderNumber)
        {
            var quiz = _quizzes.FirstOrDefault(q => q.Id == quizId);
            if (quiz == null) throw new ArgumentException("Quiz not found.");

            var updated = new Quiz(quizId, sectionId, orderNumber);
            var index = _quizzes.FindIndex(q => q.Id == quizId);
            _quizzes[index] = updated;

            return Task.CompletedTask;
        }

        public Task<List<Quiz>> GetBySectionIdAsync(int sectionId)
        {
            var quizzes = _quizzes.Where(q => q.SectionId == sectionId).ToList();
            return Task.FromResult(quizzes);
        }

    }
}
