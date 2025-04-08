using Duo.Models.Roadmap;
using Duo.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DuoTesting.MockClasses
{
    public class InMemoryRoadmapRepository : IRoadmapRepository
    {
        private readonly Dictionary<int, Roadmap> _roadmaps = new();
        private int _nextId = 1;

        public Task<int> AddAsync(Roadmap roadmap)
        {
            if (string.IsNullOrWhiteSpace(roadmap.Name))
                throw new ArgumentException("Roadmap name cannot be empty.");

            var newRoadmap = new Roadmap { Id = _nextId++, Name = roadmap.Name };
            _roadmaps[newRoadmap.Id] = newRoadmap;
            return Task.FromResult(newRoadmap.Id);
        }

        public Task<Roadmap> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid ID.");

            if (!_roadmaps.TryGetValue(id, out var roadmap))
                throw new KeyNotFoundException($"Roadmap with ID {id} not found.");

            return Task.FromResult(roadmap);
        }

        public Task<Roadmap> GetByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty.");

            var roadmap = _roadmaps.Values.FirstOrDefault(r => r.Name == name);
            if (roadmap == null)
                throw new KeyNotFoundException($"Roadmap with name '{name}' not found.");

            return Task.FromResult(roadmap);
        }

        public Task<List<Roadmap>> GetAllAsync()
            => Task.FromResult(_roadmaps.Values.ToList());

        public Task DeleteAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid ID.");

            if (!_roadmaps.Remove(id))
                throw new KeyNotFoundException("Roadmap not found.");

            return Task.CompletedTask;
        }

        public void ClearAll()
        {
            _roadmaps.Clear();
            _nextId = 1;
        }
    }
}
