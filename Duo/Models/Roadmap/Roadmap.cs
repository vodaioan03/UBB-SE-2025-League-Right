using System.Collections.Generic;
using Duo.Models.Sections;

namespace Duo.Models.Roadmap;

public class Roadmap
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Section> Sections { get; set; }

    public Roadmap()
    {
        Sections = new List<Section>();
        Name = string.Empty;
    }

    public Roadmap(int id, string name)
    {
        Id = id;
        Name = name;
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

    public override string ToString()
    {
        return $"Roadmap {Id}: {Name} - {Sections.Count} sections";
    }
}
