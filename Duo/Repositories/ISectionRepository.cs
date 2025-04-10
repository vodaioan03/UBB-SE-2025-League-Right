using System.Collections.Generic;
using System.Threading.Tasks;
using Duo.Models.Sections;

namespace Duo.Repositories
{
    public interface ISectionRepository
    {
        Task<List<Section>> GetAllAsync();
        Task<Section> GetByIdAsync(int sectionId);
        Task<List<Section>> GetByRoadmapIdAsync(int roadmapId);
        Task<int> LastOrderNumberByRoadmapIdAsync(int roadmapId);
        Task<int> CountByRoadmapIdAsync(int roadmapId);
        Task<int> AddAsync(Section section);
        Task UpdateAsync(Section section);
        Task DeleteAsync(int sectionId);
    }
}
