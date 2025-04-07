using Microsoft.VisualStudio.TestTools.UnitTesting;
using Duo.Repositories;
using Duo.Models.Sections;
using System;
using System.Threading.Tasks;
using DuoTesting.Helper;

namespace DuoTesting.Repositories
{
    [TestClass]
    public class SectionRepositoryUT : TestBase
    {
        private ISectionRepository _repository = null!;
        private int _createdRoadmapId;

        [TestInitialize]
        public async Task Setup()
        {
            base.BaseSetup();
            _repository = new SectionRepository(DbConnection); 
            _createdRoadmapId = await CreateDummyRoadmapAsync();
        }

        [TestCleanup]
        public async Task Cleanup()
        {
            using var conn = await DbConnection.CreateConnectionAsync();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM Roadmaps WHERE Id = @id";
            cmd.Parameters.AddWithValue("@id", _createdRoadmapId);
            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        private async Task<int> CreateDummyRoadmapAsync()
        {
            using var conn = await DbConnection.CreateConnectionAsync();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO Roadmaps (Name) VALUES ('TempRoadmapForTest'); SELECT SCOPE_IDENTITY();";
            await conn.OpenAsync();
            return Convert.ToInt32(await cmd.ExecuteScalarAsync());
        }

        private Section CreateSampleSection() => new Section
        {
            Title = "Test Section",
            Description = "Sample Description",
            RoadmapId = _createdRoadmapId,
            SubjectId = 1,
            OrderNumber = 1
        };

        [TestMethod]
        public async Task AddGetDelete_Section_ShouldWork()
        {
            var section = CreateSampleSection();
            int id = await _repository.AddAsync(section);

            var fetched = await _repository.GetByIdAsync(id);
            Assert.AreEqual(section.Title, fetched.Title);

            await _repository.DeleteAsync(id);
        }

        [TestMethod]
        public async Task GetAll_ShouldReturnList()
        {
            var result = await _repository.GetAllAsync();
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task Update_ShouldChangeData()
        {
            var section = CreateSampleSection();
            int id = await _repository.AddAsync(section);

            section.Id = id;
            section.Title = "Updated Section";
            await _repository.UpdateAsync(section);

            var updated = await _repository.GetByIdAsync(id);
            Assert.AreEqual("Updated Section", updated.Title);

            await _repository.DeleteAsync(id);
        }

        [TestMethod]
        public async Task GetByRoadmapId_ShouldReturn()
        {
            var section = CreateSampleSection();
            int id = await _repository.AddAsync(section);

            var list = await _repository.GetByRoadmapIdAsync(_createdRoadmapId);
            Assert.IsTrue(list.Count > 0);

            await _repository.DeleteAsync(id);
        }

        [TestMethod]
        public async Task LastOrderNumber_ShouldReturn()
        {
            var section = CreateSampleSection();
            int id = await _repository.AddAsync(section);

            var last = await _repository.LastOrderNumberByRoadmapIdAsync(_createdRoadmapId);
            Assert.IsTrue(last >= 0);

            await _repository.DeleteAsync(id);
        }

        [TestMethod]
        public async Task CountByRoadmap_ShouldReturn()
        {
            var count = await _repository.CountByRoadmapIdAsync(_createdRoadmapId);
            Assert.IsTrue(count >= 0);
        }

        [TestMethod]
        public async Task Add_InvalidTitle_ShouldThrow()
        {
            var section = CreateSampleSection();
            section.Title = "";

            await Assert.ThrowsExceptionAsync<ArgumentException>(() => _repository.AddAsync(section));
        }

        [TestMethod]
        public async Task Delete_InvalidId_ShouldThrow()
        {
            await Assert.ThrowsExceptionAsync<ArgumentException>(() => _repository.DeleteAsync(0));
        }
    }
}
