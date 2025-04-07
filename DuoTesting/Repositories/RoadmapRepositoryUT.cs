using Microsoft.VisualStudio.TestTools.UnitTesting;
using Duo.Repositories;
using Duo.Models.Roadmap;
using System.Threading.Tasks;
using System;
using System.Linq;
using DuoTesting.Helper;
using System.Data.Common;

namespace DuoTesting.Repositories
{
    [TestClass]
    public class RoadmapRepositoryUT : TestBase
    {
        private IRoadmapRepository _repository = null!;

        [TestInitialize]
        public void Setup()
        {
            _repository = new RoadmapRepository(DbConnection);
        }

        [TestMethod]
        public async Task AddAndGetById_ShouldReturnSameRoadmap()
        {
            var roadmap = new Roadmap { Name = "Test Roadmap" };
            var newId = await _repository.AddAsync(roadmap);

            var result = await _repository.GetByIdAsync(newId);
            Assert.AreEqual("Test Roadmap", result.Name);

            await _repository.DeleteAsync(newId);
        }

        [TestMethod]
        public async Task GetAllAsync_ShouldReturnList()
        {
            var all = await _repository.GetAllAsync();
            Assert.IsNotNull(all);
        }

        [TestMethod]
        public async Task AddAndGetByName_ShouldWork()
        {
            var name = $"TempRoadmap_{Guid.NewGuid()}";
            var id = await _repository.AddAsync(new Roadmap { Name = name });

            var byName = await _repository.GetByNameAsync(name);
            Assert.IsNotNull(byName);
            Assert.AreEqual(name, byName.Name);

            await _repository.DeleteAsync(id);
        }

        [TestMethod]
        public async Task AddAsync_InvalidName_ShouldThrow()
        {
            await Assert.ThrowsExceptionAsync<ArgumentException>(() =>
                _repository.AddAsync(new Roadmap { Name = " " }));
        }

        [TestMethod]
        public async Task GetByIdAsync_InvalidId_ShouldThrow()
        {
            await Assert.ThrowsExceptionAsync<ArgumentException>(() => _repository.GetByIdAsync(0));
        }

        [TestMethod]
        public async Task GetByNameAsync_Invalid_ShouldThrow()
        {
            await Assert.ThrowsExceptionAsync<ArgumentException>(() => _repository.GetByNameAsync(""));
        }

        [TestMethod]
        public async Task DeleteAsync_InvalidId_ShouldThrow()
        {
            await Assert.ThrowsExceptionAsync<ArgumentException>(() => _repository.DeleteAsync(0));
        }

        [TestMethod]
        public async Task DeleteAsync_ShouldRemoveRoadmap()
        {
            var id = await _repository.AddAsync(new Roadmap { Name = "ToBeDeleted" });

            await _repository.DeleteAsync(id);

            await Assert.ThrowsExceptionAsync<KeyNotFoundException>(() => _repository.GetByIdAsync(id));
        }
    }
}
