using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Duo.Models.Roadmap;
using Duo.Repositories;

namespace Duo.Services
{
    public class RoadmapService
    {
        private RoadmapRepository _roadmapRepository;

        public RoadmapService(RoadmapRepository roadmapRepository)
        {
            _roadmapRepository = roadmapRepository;
        }

        public Task<List<Roadmap>> GetAllRoadmaps()
        {
            return _roadmapRepository.GetAllAsync();
        }

        public Task<Roadmap> GetRoadmapById(int roadmapId)
        {
            return _roadmapRepository.GetByIdAsync(roadmapId);
        }

        public Task<Roadmap> GetByName(string roadmapName)
        {
            return _roadmapRepository.GetByNameAsync(roadmapName);
        }

        public Task<int> AddRoadmap(Roadmap roadmap)
        {
            return _roadmapRepository.AddAsync(roadmap);
        }

        public Task DeleteRoadmap(Roadmap roadmap)
        {
            return _roadmapRepository.DeleteAsync(roadmap.Id);
        }
    }
}
