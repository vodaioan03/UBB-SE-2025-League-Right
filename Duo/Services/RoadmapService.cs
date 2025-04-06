using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Duo.Models.Roadmap;
using Duo.Repositories;

namespace Duo.Services
{
    public class RoadmapService : IRoadmapService
    {
        private RoadmapRepository roadmapRepository;

        public RoadmapService(RoadmapRepository roadmapRepository)
        {
            this.roadmapRepository = roadmapRepository;
        }

        public Task<List<Roadmap>> GetAllRoadmaps()
        {
            return roadmapRepository.GetAllAsync();
        }

        public Task<Roadmap> GetRoadmapById(int roadmapId)
        {
            return roadmapRepository.GetByIdAsync(roadmapId);
        }

        public Task<Roadmap> GetByName(string roadmapName)
        {
            return roadmapRepository.GetByNameAsync(roadmapName);
        }

        public Task<int> AddRoadmap(Roadmap roadmap)
        {
            return roadmapRepository.AddAsync(roadmap);
        }

        public Task DeleteRoadmap(Roadmap roadmap)
        {
            return roadmapRepository.DeleteAsync(roadmap.Id);
        }
    }
}
