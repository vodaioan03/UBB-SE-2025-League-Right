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
            _ = result[0]; 
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

        [TestMethod]
        public async Task GetByIdAsync_Nonexistent_ShouldThrow()
        {
            await Assert.ThrowsExceptionAsync<KeyNotFoundException>(() =>
                _repository.GetByIdAsync(999));
        }

        [TestMethod]
        public async Task Update_Nonexistent_ShouldThrow()
        {
            var fake = CreateSampleSection();
            fake.Id = 999;

            await Assert.ThrowsExceptionAsync<KeyNotFoundException>(() =>
                _repository.UpdateAsync(fake));
        }

        [TestMethod]
        public async Task Delete_Nonexistent_ShouldThrow()
        {
            await Assert.ThrowsExceptionAsync<KeyNotFoundException>(() =>
                _repository.DeleteAsync(999));
        }

        [TestMethod]
        public async Task GetByRoadmapId_Empty_ShouldReturnEmptyList()
        {
            var result = await _repository.GetByRoadmapIdAsync(999); 
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public async Task Add_WhitespaceTitle_ShouldThrow()
        {
            var section = CreateSampleSection();
            section.Title = "   ";
            await Assert.ThrowsExceptionAsync<ArgumentException>(() => _repository.AddAsync(section));
        }

        [TestMethod]
        public async Task LastOrderNumber_NoSections_ShouldReturnZero()
        {
            var result = await _repository.LastOrderNumberByRoadmapIdAsync(999);
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public async Task CountByRoadmap_NoSections_ShouldReturnZero()
        {
            var count = await _repository.CountByRoadmapIdAsync(999);
            Assert.AreEqual(0, count);
        }
        [TestMethod]
        public async Task LastOrderNumber_SectionWithoutOrder_ShouldBeSkipped()
        {
            var section = new Section
            {
                Title = "NoOrder",
                Description = "None",
                RoadmapId = DummyRoadmapId,
                SubjectId = 1,
                OrderNumber = null 
            };

            await _repository.AddAsync(section);
            var result = await _repository.LastOrderNumberByRoadmapIdAsync(DummyRoadmapId);

            Assert.AreEqual(0, result); 
        }



    }
}
