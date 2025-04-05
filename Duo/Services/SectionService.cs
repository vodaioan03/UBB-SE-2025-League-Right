using System.Collections.Generic;
using System.Threading.Tasks;
using Duo.Models.Exercises;
using Duo.Models.Quizzes;
using Duo.Models.Sections;
using Duo.Repositories;

namespace Duo.Services
{
    public class SectionService : ISectionService
    {
        private SectionRepository _sectionRepository;

        public SectionService(SectionRepository sectionRepository)
        {
            _sectionRepository = sectionRepository;
        }

        public async Task<List<Section>> GetAllSections()
        {
            return await _sectionRepository.GetAllAsync();
        }

        public Task<Section> GetSectionById(int sectionId)
        {
            return _sectionRepository.GetByIdAsync(sectionId);
        }

        public Task<List<Section>> GetByRoadmapId(int roadmapId)
        {
            return _sectionRepository.GetByRoadmapIdAsync(roadmapId);
        }

        public async Task<int> CountSectionsFromRoadmap(int roadmapId)
        {
            return await _sectionRepository.CountByRoadmapIdAsync(roadmapId);
        }

        public async Task<int> LastOrderNumberFromRoadmap(int roadmapId)
        {
            return await _sectionRepository.LastOrderNumberByRoadmapIdAsync(roadmapId);
        }

        public async Task<int> AddSection(Section section)
        {
            ValidationHelper.ValidateSection(section);
            List<Section> allSections = await GetAllSections();
            int orderNumber = allSections.Count;
            section.OrderNumber = orderNumber + 1;
            return await _sectionRepository.AddAsync(section);
        }

        public Task DeleteSection(int sectionId)
        {
            return _sectionRepository.DeleteAsync(sectionId);
        }

        public Task UpdateSection(Section section)
        {
            ValidationHelper.ValidateSection(section);
            return _sectionRepository.UpdateAsync(section);
        }
    }
}