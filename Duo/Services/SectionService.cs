using System.Collections.Generic;
using System.Threading.Tasks;
using Duo.Models.Sections;
using Duo.Repositories;

namespace Duo.Services
{
    public class SectionService
    {
        private SectionRepository _sectionRepository;

        public SectionService(SectionRepository sectionRepository)
        {
            _sectionRepository = sectionRepository;
        }

        public Task<IEnumerable<Section>> GetAllSections()
        {
            return _sectionRepository.GetAllAsync();
        }

        public Task<Section> GetSectionById(int sectionId)
        {
            return _sectionRepository.GetByIdAsync(sectionId);
        }

        public Task<IEnumerable<Section>> GetByRoadmapId(int roadmapId)
        {
            return _sectionRepository.GetByRoadmapIdAsync(roadmapId);
        }

        public Task<int> AddSection(Section section)
        {
            return _sectionRepository.AddAsync(section);
        }

        public Task DeleteSection(int sectionId)
        {
            return _sectionRepository.DeleteAsync(sectionId);
        }

        public Task UpdateSection(Section section)
        {
            return _sectionRepository.UpdateAsync(section);
        }
    }
}