using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Duo.Data;
using Duo.Models.Quizzes;
using Duo.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace DuoTesting.Repositories
{
    [TestClass]
    public class ExamRepositoryUT
    {
        private IExamRepository _repository = null!;
        private DatabaseConnection _dbConnection = null!;

        [TestInitialize]
        public void Setup()
        {
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                  
                    { "DbConnection", "Server=DESKTOP-BVGO48P\\SQLEXPRESS;Database=new-league;Trusted_Connection=True;TrustServerCertificate=true;" }
                })
                .Build();

            _dbConnection = new DatabaseConnection(config);
            _repository = new ExamRepository(_dbConnection);
        }

        [TestMethod]
        public async Task AddAndGetById_ShouldReturnSameExam()
        {
            var newExam = new Exam(0, null);

            var newId = await _repository.AddAsync(newExam);
            var examFromDb = await _repository.GetByIdAsync(newId);

            Assert.IsNotNull(examFromDb);
            Assert.AreEqual(newId, examFromDb.Id);
            Assert.IsNull(examFromDb.SectionId);

            await _repository.DeleteAsync(newId);
        }

        [TestMethod]
        public async Task GetAllAsync_ShouldReturnExams()
        {
            var exams = await _repository.GetAllAsync();
            Assert.IsNotNull(exams);
        }

        [TestMethod]
        public async Task GetUnassignedAsync_ShouldReturnUnassignedExams()
        {
            var unassigned = await _repository.GetUnassignedAsync();
            Assert.IsNotNull(unassigned);
        }

        private async Task<int> AddTestSectionAsync()
        {
            using var conn = await _dbConnection.CreateConnectionAsync();
            using var cmd = conn.CreateCommand();

            cmd.CommandText = @"
            INSERT INTO Roadmaps (Name) VALUES ('TempTestRoadmap');
            DECLARE @roadmapId INT = SCOPE_IDENTITY();

            INSERT INTO Sections (SubjectId, Title, Description, RoadmapId, OrderNumber)
            VALUES (1, 'TestSection', 'Desc', @roadmapId, 1);

            SELECT SCOPE_IDENTITY();";

            await conn.OpenAsync();
            var result = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }


        [TestMethod]
        public async Task AddUpdateDelete_ShouldUpdateExamSection()
        {
            int testSectionId = await AddTestSectionAsync();

            var examId = await _repository.AddAsync(new Exam(0, testSectionId)); 

            await _repository.UpdateExamSection(examId, null);
            var updated = await _repository.GetByIdAsync(examId);
            Assert.IsNull(updated.SectionId);

            await _repository.DeleteAsync(examId);
        }


        [TestMethod]
        public async Task GetBySectionIdAsync_Invalid_ShouldThrow()
        {
            await Assert.ThrowsExceptionAsync<ArgumentException>(() => _repository.GetBySectionIdAsync(-1));
        }


        [TestMethod]
        public async Task Delete_NonExistent_ShouldThrow()
        {
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                await _repository.DeleteAsync(-999);
            });
        }


        [TestMethod]
        public async Task AddExerciseToExam_And_RemoveExerciseFromExam()
        {
            int examId = await _repository.AddAsync(new Exam(0, null));

            int exerciseId = 1;

            await _repository.AddExerciseToExam(examId, exerciseId);


            await _repository.RemoveExerciseFromExam(examId, exerciseId);
            await _repository.DeleteAsync(examId);
        }

        [TestMethod]
        public async Task UpdateAsync_ShouldUpdateExam()
        {
            var examId = await _repository.AddAsync(new Exam(0, null));

            var updatedExam = new Exam(examId, null); 
            await _repository.UpdateAsync(updatedExam);

            var result = await _repository.GetByIdAsync(examId);
            Assert.IsNull(result.SectionId);

            await _repository.DeleteAsync(examId);
        }


        [TestMethod]
        public async Task AddAsync_WithInvalidSection_Throws()
        {
            var exam = new Exam(0, -5); 
            await Assert.ThrowsExceptionAsync<ArgumentException>(() => _repository.AddAsync(exam));
        }


        [TestMethod]
        public async Task UpdateExamSection_InvalidId_Throws()
        {
            await Assert.ThrowsExceptionAsync<ArgumentException>(() => _repository.UpdateExamSection(-1, 1));
        }
    }
}
