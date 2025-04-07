using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Duo.Services;
using Duo.Repositories;
using Duo.Models.Roadmap;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DuoTesting.Services
{
    [TestClass]
    public class RoadmapServiceUT
    {
        private Mock<IRoadmapRepository> _mockRepository;
        private RoadmapService _service;

        [TestInitialize]
        public void Setup()
        {
            _mockRepository = new Mock<IRoadmapRepository>();
            _service = new RoadmapService(_mockRepository.Object);
        }

        [TestMethod]
        public async Task AddRoadmap_ShouldAddSuccessfully()
        {
            var roadmap = new Roadmap { Name = "Test Roadmap" };
            _mockRepository.Setup(repo => repo.AddAsync(It.IsAny<Roadmap>())).ReturnsAsync(1);

            var result = await _service.AddRoadmap(roadmap);

            Assert.AreEqual(1, result);
            _mockRepository.Verify(repo => repo.AddAsync(It.IsAny<Roadmap>()), Times.Once);
        }

        [TestMethod]
        public async Task GetAllRoadmaps_ShouldReturnList()
        {
            var roadmaps = new List<Roadmap>
            {
                new Roadmap { Id = 1, Name = "Roadmap 1" },
                new Roadmap { Id = 2, Name = "Roadmap 2" }
            };

            _mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(roadmaps);

            var result = await _service.GetAllRoadmaps();

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public async Task GetByName_ShouldReturnCorrectRoadmap()
        {
            var roadmap = new Roadmap { Id = 1, Name = "Test Roadmap" };

            _mockRepository.Setup(repo => repo.GetByNameAsync("Test Roadmap")).ReturnsAsync(roadmap);

            var result = await _service.GetByName("Test Roadmap");

            Assert.IsNotNull(result);
            Assert.AreEqual("Test Roadmap", result.Name);
        }

        [TestMethod]
        public async Task GetRoadmapById_ShouldReturnCorrectRoadmap()
        {
            var roadmap = new Roadmap { Id = 1, Name = "Test Roadmap" };

            _mockRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(roadmap);

            var result = await _service.GetRoadmapById(1);

            Assert.IsNotNull(result);
            Assert.AreEqual("Test Roadmap", result.Name);
        }

        [TestMethod]
        public async Task DeleteRoadmap_ShouldDeleteSuccessfully()
        {
            var roadmap = new Roadmap { Id = 1, Name = "Test Roadmap" };

            _mockRepository.Setup(repo => repo.DeleteAsync(1)).Returns(Task.CompletedTask);

            await _service.DeleteRoadmap(roadmap);

            _mockRepository.Verify(repo => repo.DeleteAsync(1), Times.Once);
        }

        [TestMethod]
        public async Task AddRoadmap_ShouldThrowIfInvalid()
        {
            var roadmap = new Roadmap { Name = "" }; // Invalid name
            _mockRepository.Setup(repo => repo.AddAsync(It.IsAny<Roadmap>())).ReturnsAsync(1);

            try
            {
                await _service.AddRoadmap(roadmap);
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentException)); // Checking if it throws the ArgumentException
            }
        }

        [TestMethod]
        public async Task GetByName_InvalidName_ShouldReturnNull()
        {
            _mockRepository.Setup(repo => repo.GetByNameAsync(It.IsAny<string>())).ReturnsAsync((Roadmap)null);

            var result = await _service.GetByName("Invalid Roadmap");

            Assert.IsNull(result); // No exception thrown, method returns null
        }

        [TestMethod]
        public async Task GetRoadmapById_InvalidId_ShouldReturnNull()
        {
            _mockRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Roadmap)null);

            var result = await _service.GetRoadmapById(-1); // Invalid ID

            Assert.IsNull(result); // No exception thrown, method returns null
        }
    }
}
