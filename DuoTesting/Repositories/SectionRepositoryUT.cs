using Microsoft.VisualStudio.TestTools.UnitTesting;
using Duo.Repositories;
using Duo.Models.Sections;
using DuoTesting.Helpers;
using DuoTesting.MockClasses;
using System.Threading.Tasks;
using System;

namespace DuoTesting.Repositories
{
    [TestClass]
    public class SectionRepositoryUT
    {
        private ISectionRepository _repository = null!;
        private const int DummyRoadmapId = 1;

        [TestInitialize]
        public void Setup()
        {
            _repository = new InMemorySectionRepository();
        }

        private Section CreateSampleSection() => new Section
        {
            Title = "Test Section",
            Description = "Sample Description",
            RoadmapId = DummyRoadmapId,
            SubjectId = 1,
            OrderNumber = 1
        };

        [TestMethod]
        public async Task AddGetDelete_Section_ShouldWork()
        {
            var section = CreateSampleSection();
            int id = await _repository.AddAsync(section);

            var fetched = await _repository.GetByIdAsync(id);
            var expected = section;
            expected.Id = id;

            Assert.IsTrue(new SectionComparer().Equals(expected, fetched));

            await _repository.DeleteAsync(id);
        }

        [TestMethod]
        public async Task GetAll_ShouldReturnList()
        {
            await _repository.AddAsync(CreateSampleSection());
            var result = await _repository.GetAllAsync();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
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
        }

        [TestMethod]
        public async Task GetByRoadmapId_ShouldReturn()
        {
            var section = CreateSampleSection();
            int id = await _repository.AddAsync(section);

            var list = await _repository.GetByRoadmapIdAsync(DummyRoadmapId);
            Assert.IsTrue(list.Count > 0);
        }

        [TestMethod]
        public async Task LastOrderNumber_ShouldReturn()
        {
            var section = CreateSampleSection();
            await _repository.AddAsync(section);

            var last = await _repository.LastOrderNumberByRoadmapIdAsync(DummyRoadmapId);
            Assert.IsTrue(last >= 1);
        }

        [TestMethod]
        public async Task CountByRoadmap_ShouldReturn()
        {
            await _repository.AddAsync(CreateSampleSection());
            var count = await _repository.CountByRoadmapIdAsync(DummyRoadmapId);
            Assert.IsTrue(count >= 1);
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
