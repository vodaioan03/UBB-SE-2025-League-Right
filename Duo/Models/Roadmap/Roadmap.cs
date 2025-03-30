using System.Collections.Generic;
using Duo.Models.Sections;

namespace Duo.Models.Roadmap;


public class Roadmap
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public List<Section> Sections { get; set; }

    public Roadmap()
    {
        Sections = new List<Section>();
    }

    public void AddSection(Section section)
    {
        Sections.Add(section);
    }

    public void RemoveSection(Section section)
    {
        Sections.Remove(section);
    }

    public IEnumerable<Section> GetAllSections()
    {
        return Sections;
    }
}
