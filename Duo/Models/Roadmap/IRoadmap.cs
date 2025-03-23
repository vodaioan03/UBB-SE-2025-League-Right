using System.Collections.Generic;

namespace Duo.Models.Roadmap;

interface IRoadmap<T>
{
    public void AddSection(T section);
    public IEnumerable<T> GetAllSections();
}