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


    }
}
