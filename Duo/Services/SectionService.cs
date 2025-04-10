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
        private ISectionRepository sectionRepository;

        public SectionService(ISectionRepository sectionRepository)
        {
            this.sectionRepository = sectionRepository;
        }

        public async Task<List<Section>> GetAllSections()
        {
            return await sectionRepository.GetAllAsync();
        }

        public Task<Section> GetSectionById(int sectionId)
        {
            return sectionRepository.GetByIdAsync(sectionId);
        }

        public Task<List<Section>> GetByRoadmapId(int roadmapId)
        {
            return sectionRepository.GetByRoadmapIdAsync(roadmapId);
        }

        public async Task<int> CountSectionsFromRoadmap(int roadmapId)
        {
            return await sectionRepository.CountByRoadmapIdAsync(roadmapId);
        }

        public async Task<int> LastOrderNumberFromRoadmap(int roadmapId)
        {
            return await sectionRepository.LastOrderNumberByRoadmapIdAsync(roadmapId);
        }

        public async Task<int> AddSection(Section section)
        {
            ValidationHelper.ValidateSection(section);
            List<Section> allSections = await GetAllSections();
            int orderNumber = allSections.Count;
            section.OrderNumber = orderNumber + 1;
            return await sectionRepository.AddAsync(section);
        }

        public Task DeleteSection(int sectionId)
        {
            return sectionRepository.DeleteAsync(sectionId);
        }

        public Task UpdateSection(Section section)
        {
            ValidationHelper.ValidateSection(section);
            return sectionRepository.UpdateAsync(section);
        }
    }
}