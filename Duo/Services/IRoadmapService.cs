using System.Collections.Generic;
using System.Threading.Tasks;
using Duo.Models.Roadmap;

namespace Duo.Services
{
    public interface IRoadmapService
    {
        Task<int> AddRoadmap(Roadmap roadmap);
        Task DeleteRoadmap(Roadmap roadmap);
        Task<List<Roadmap>> GetAllRoadmaps();
        Task<Roadmap> GetByName(string roadmapName);
        Task<Roadmap> GetRoadmapById(int roadmapId);
    }
}