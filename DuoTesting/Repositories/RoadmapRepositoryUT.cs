using Microsoft.VisualStudio.TestTools.UnitTesting;
using Duo.Repositories;
using Duo.Models.Roadmap;
using DuoTesting.Helpers;
using DuoTesting.MockClasses;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace DuoTesting.Repositories
{
    [TestClass]
    public class RoadmapRepositoryUT
    {
        private IRoadmapRepository _repository = null!;

        [TestInitialize]
        public void Setup()
        {
            _repository = new InMemoryRoadmapRepository();
        }

        [TestMethod]
        public async Task AddAndGetById_ShouldReturnSameRoadmap()
        {
            var roadmap = new Roadmap { Name = "Test Roadmap" };
            var newId = await _repository.AddAsync(roadmap);

            var expected = new Roadmap { Id = newId, Name = "Test Roadmap" };
            var result = await _repository.GetByIdAsync(newId);

            Assert.IsTrue(new RoadmapComparer().Equals(expected, result));
        }

        [TestMethod]
        public async Task GetAllAsync_ShouldReturnList()
        {
            await _repository.AddAsync(new Roadmap { Name = "Sample" });
            var all = await _repository.GetAllAsync();
            Assert.IsNotNull(all);
            Assert.IsTrue(all.Count > 0);
        }

        [TestMethod]
        public async Task AddAndGetByName_ShouldWork()
        {
            var name = $"TempRoadmap_{Guid.NewGuid()}";
            var id = await _repository.AddAsync(new Roadmap { Name = name });

            var expected = new Roadmap { Id = id, Name = name };
            var byName = await _repository.GetByNameAsync(name);

            Assert.IsTrue(new RoadmapComparer().Equals(expected, byName));
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

        [TestMethod]
        public async Task GetByNameAsync_NotFound_ShouldThrow()
        {
            await Assert.ThrowsExceptionAsync<KeyNotFoundException>(() =>
                _repository.GetByNameAsync("DoesNotExist123"));
        }

        [TestMethod]
        public async Task DeleteAsync_NotFound_ShouldThrow()
        {
            await Assert.ThrowsExceptionAsync<KeyNotFoundException>(() =>
                _repository.DeleteAsync(999));
        }

        [TestMethod]
        public async Task ClearAll_ShouldEmptyRepository()
        {
            var repo = new InMemoryRoadmapRepository();
            var id = await repo.AddAsync(new Roadmap { Name = "Clearable" });

            repo.ClearAll();

            await Assert.ThrowsExceptionAsync<KeyNotFoundException>(() => repo.GetByIdAsync(id));
        }

    }
}
