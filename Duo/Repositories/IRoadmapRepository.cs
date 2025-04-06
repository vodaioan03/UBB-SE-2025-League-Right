using System.Collections.Generic;
using System.Threading.Tasks;
using Duo.Models.Roadmap;

namespace Duo.Repositories
{
    public interface IRoadmapRepository
    {
        Task<List<Roadmap>> GetAllAsync();
        Task<Roadmap> GetByIdAsync(int roadmapId);
        Task<Roadmap> GetByNameAsync(string roadmapName);
        Task<int> AddAsync(Roadmap roadmap);
        Task DeleteAsync(int roadmapId);
    }
}
