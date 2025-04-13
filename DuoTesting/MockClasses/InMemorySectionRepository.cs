using Duo.Models.Sections;
using Duo.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DuoTesting.MockClasses
{
    public class InMemorySectionRepository : ISectionRepository
    {
        private readonly List<Section> _sections = new();
        private int _nextId = 1;

        public Task<int> AddAsync(Section section)
        {
            if (string.IsNullOrWhiteSpace(section.Title))
                throw new ArgumentException("Title cannot be empty");

            var newSection = new Section(
                _nextId++,
                section.SubjectId,
                section.Title,
                section.Description,
                section.RoadmapId,
                section.OrderNumber
            );

            _sections.Add(newSection);
            return Task.FromResult(newSection.Id);
        }

        public Task<List<Section>> GetAllAsync()
        {
            return Task.FromResult(_sections.ToList());
        }

        public Task<Section> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid ID");

            var section = _sections.FirstOrDefault(s => s.Id == id);
            if (section == null)
                throw new KeyNotFoundException("Section not found");

            return Task.FromResult(section);
        }

        public Task<List<Section>> GetByRoadmapIdAsync(int roadmapId)
        {
            return Task.FromResult(_sections.Where(s => s.RoadmapId == roadmapId).ToList());
        }

        public Task<int> LastOrderNumberByRoadmapIdAsync(int roadmapId)
        {
            var sections = _sections.Where(s => s.RoadmapId == roadmapId && s.OrderNumber.HasValue).ToList();
            var max = sections.Any() ? sections.Max(s => s.OrderNumber.Value) : 0;
            return Task.FromResult(max);
        }

        public Task<int> CountByRoadmapIdAsync(int roadmapId)
        {
            return Task.FromResult(_sections.Count(s => s.RoadmapId == roadmapId));
        }

        public Task UpdateAsync(Section section)
        {
            var index = _sections.FindIndex(s => s.Id == section.Id);
            if (index == -1) throw new KeyNotFoundException("Section not found");

            _sections[index] = section;
            return Task.CompletedTask;
        }

        public Task DeleteAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("Invalid ID");

            var removed = _sections.RemoveAll(s => s.Id == id);
            if (removed == 0)
                throw new KeyNotFoundException("Section not found");

            return Task.CompletedTask;
        }


    }
}
