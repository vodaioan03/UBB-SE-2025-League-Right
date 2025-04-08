using Microsoft.VisualStudio.TestTools.UnitTesting;
using DuoTesting.Helpers;
using Duo.Repositories;
using Duo.Models.Quizzes;
using DuoTesting.MockClasses;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using DuoTesting.Helper;

namespace DuoTesting.Repositories
{
    [TestClass]
    public class QuizRepositoryUT
    {
        private IQuizRepository _repository = null!;

        [TestInitialize]
        public void Setup()
        {
            _repository = new InMemoryQuizRepository();
        }

        [TestMethod]
        public async Task AddUpdateDeleteQuiz_ShouldWork()
        {
            var quiz = new Quiz(0, 1, 1);
            int quizId = await _repository.AddAsync(quiz);

            var fromRepo = await _repository.GetByIdAsync(quizId);
            var expected = new Quiz(quizId, 1, 1);
            Assert.IsTrue(new QuizComparer().Equals(expected, fromRepo));

            await _repository.UpdateAsync(new Quiz(quizId, 1, 2));
            var updated = await _repository.GetByIdAsync(quizId);
            Assert.AreEqual(2, updated.OrderNumber);

            await _repository.DeleteAsync(quizId);
        }

        [TestMethod]
        public async Task AddAsync_InvalidSection_ShouldThrow()
        {
            var quiz = new Quiz(0, -1, 1);
            await Assert.ThrowsExceptionAsync<ArgumentException>(() => _repository.AddAsync(quiz));
        }

        [TestMethod]
        public async Task UpdateAsync_InvalidQuiz_ShouldThrow()
        {
            var quiz = new Quiz(0, 1, 1);
            await Assert.ThrowsExceptionAsync<ArgumentException>(() => _repository.UpdateAsync(quiz));
        }

        [TestMethod]
        public async Task DeleteAsync_InvalidQuiz_ShouldThrow()
        {
            await Assert.ThrowsExceptionAsync<ArgumentException>(() => _repository.DeleteAsync(0));
        }

        [TestMethod]
        public async Task GetAllAsync_ShouldReturnList()
        {
            await _repository.AddAsync(new Quiz(0, 1, 1));
            var result = await _repository.GetAllAsync();
            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public async Task GetUnassignedAsync_ShouldReturnList()
        {
            await _repository.AddAsync(new Quiz(0, null, 1));
            var result = await _repository.GetUnassignedAsync();
            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public async Task CountBySectionIdAsync_ShouldReturnZeroOrMore()
        {
            await _repository.AddAsync(new Quiz(0, 42, 1));
            var count = await _repository.CountBySectionIdAsync(42);
            Assert.IsTrue(count >= 1);
        }

        [TestMethod]
        public async Task LastOrderNumberBySectionIdAsync_ShouldReturnZeroOrMore()
        {
            await _repository.AddAsync(new Quiz(0, 10, 1));
            await _repository.AddAsync(new Quiz(0, 10, 3));
            var last = await _repository.LastOrderNumberBySectionIdAsync(10);
            Assert.AreEqual(3, last);
        }

        [TestMethod]
        public async Task AddAndRemoveExerciseToQuiz_ShouldWork()
        {
            int quizId = await _repository.AddAsync(new Quiz(0, null, null));
            int exerciseId = 1;

            await _repository.AddExerciseToQuiz(quizId, exerciseId);
            await _repository.RemoveExerciseFromQuiz(quizId, exerciseId);
            await _repository.DeleteAsync(quizId);
        }

        [TestMethod]
        public async Task UpdateQuizSection_ShouldWork()
        {
            int quizId = await _repository.AddAsync(new Quiz(0, null, null));
            await _repository.UpdateQuizSection(quizId, 5, 3);

            var updated = await _repository.GetByIdAsync(quizId);
            Assert.AreEqual(5, updated.SectionId);
            Assert.AreEqual(3, updated.OrderNumber);
        }
    }
}
