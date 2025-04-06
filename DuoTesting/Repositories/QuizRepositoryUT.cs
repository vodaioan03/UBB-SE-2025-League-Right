using Microsoft.VisualStudio.TestTools.UnitTesting;
using DuoTesting.Helper;
using Duo.Repositories;
using Duo.Models.Quizzes;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Data.Common;

namespace DuoTesting.Repositories
{
    [TestClass]
    public class QuizRepositoryUT : TestBase
    {
        private IQuizRepository _repository = null!;

        [TestInitialize]
        public void Setup()
        {
            _repository = new QuizRepository(DbConnection);
        }

        private async Task<int> AddTestSectionAsync()
        {
            using var conn = await DbConnection.CreateConnectionAsync();
            using var cmd = conn.CreateCommand();

            cmd.CommandText = @"
                INSERT INTO Roadmaps (Name) VALUES ('TestRoadmap');
                DECLARE @roadmapId INT = SCOPE_IDENTITY();

                INSERT INTO Sections (SubjectId, Title, Description, RoadmapId, OrderNumber)
                VALUES (1, 'TestSection', 'Description', @roadmapId, 1);

                SELECT SCOPE_IDENTITY();";

            await conn.OpenAsync();
            var result = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }

        [TestMethod]
        public async Task AddUpdateDeleteQuiz_ShouldWork()
        {
            int sectionId = await AddTestSectionAsync();

            var quiz = new Quiz(0, sectionId, 1);
            int quizId = await _repository.AddAsync(quiz);

            var fromDb = await _repository.GetByIdAsync(quizId);
            Assert.AreEqual(sectionId, fromDb.SectionId);

            await _repository.UpdateAsync(new Quiz(quizId, sectionId, 2));
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
            var quizzes = await _repository.GetAllAsync();
            Assert.IsNotNull(quizzes);
        }

        [TestMethod]
        public async Task GetUnassignedAsync_ShouldReturnList()
        {
            var quizzes = await _repository.GetUnassignedAsync();
            Assert.IsNotNull(quizzes);
        }

        [TestMethod]
        public async Task CountBySectionIdAsync_ShouldReturnZeroOrMore()
        {
            int sectionId = await AddTestSectionAsync();
            var count = await _repository.CountBySectionIdAsync(sectionId);
            Assert.IsTrue(count >= 0);
        }

        [TestMethod]
        public async Task LastOrderNumberBySectionIdAsync_ShouldReturnZeroOrMore()
        {
            int sectionId = await AddTestSectionAsync();
            var last = await _repository.LastOrderNumberBySectionIdAsync(sectionId);
            Assert.IsTrue(last >= 0);
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
            int sectionId = await AddTestSectionAsync();
            int quizId = await _repository.AddAsync(new Quiz(0, null, null));

            await _repository.UpdateQuizSection(quizId, sectionId, 3);

            var updated = await _repository.GetByIdAsync(quizId);
            Assert.AreEqual(sectionId, updated.SectionId);

            await _repository.DeleteAsync(quizId);
        }
    }
}
