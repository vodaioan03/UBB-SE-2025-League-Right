using System.Collections.Generic;

namespace Duo.Models.Roadmap;

public class Roadmap <T>: IRoadmap<T>
{
    private List<T> sectionList;

    public Roadmap()
    {
        sectionList = new List<T>();
    }

    public void AddSection(T section)
    {
        sectionList.Add(section);
    }

    public IEnumerable<T> GetAllSections()
    {
        return sectionList;
    }
}
