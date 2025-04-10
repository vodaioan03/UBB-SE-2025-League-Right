using Duo.Models.Roadmap;
using Duo.Models.Sections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DuoTesting.Helpers
{
    public class RoadmapComparer : IEqualityComparer<Roadmap>
    {
        private readonly SectionComparer _sectionComparer = new();

        public bool Equals(Roadmap? x, Roadmap? y)
        {
            if (x is null || y is null) return false;

            return x.Id == y.Id &&
                   x.Name == y.Name &&
                   CompareSections(x.Sections, y.Sections);
        }

        public int GetHashCode(Roadmap obj)
        {
            int hash = HashCode.Combine(obj.Id, obj.Name);
            foreach (var section in obj.Sections)
            {
                hash = HashCode.Combine(hash, _sectionComparer.GetHashCode(section));
            }
            return hash;
        }

        private bool CompareSections(List<Section> s1, List<Section> s2)
        {
            if (s1.Count != s2.Count) return false;
            for (int i = 0; i < s1.Count; i++)
            {
                if (!_sectionComparer.Equals(s1[i], s2[i]))
                    return false;
            }
            return true;
        }
    }
}
