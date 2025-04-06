using System.Collections.Generic;
using System.Threading.Tasks;
using Duo.Models.Sections;

namespace Duo.Services
{
    public interface ISectionService
    {
        Task<int> AddSection(Section section);
        Task<int> CountSectionsFromRoadmap(int roadmapId);
        Task DeleteSection(int sectionId);
        Task<List<Section>> GetAllSections();
        Task<List<Section>> GetByRoadmapId(int roadmapId);
        Task<Section> GetSectionById(int sectionId);
        Task<int> LastOrderNumberFromRoadmap(int roadmapId);
        Task UpdateSection(Section section);
    }
}