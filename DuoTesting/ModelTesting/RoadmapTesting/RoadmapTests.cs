using Duo.Models.Roadmap;
using Duo.Models.Sections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuoTesting.ModelTesting.RoadmapTesting
{
    [TestClass]
    public class RoadmapTests
    {
        [TestMethod]
        public void DefaultConstructor_InitializesEmptySectionsAndEmptyName()
        {
            // Act
            var roadmap = new Roadmap();

            // Assert
            Assert.IsNotNull(roadmap.Sections, "Sections list should be initialized.");
            Assert.AreEqual(0, roadmap.Sections.Count, "Sections list should be empty initially.");
            Assert.AreEqual(string.Empty, roadmap.Name, "Name should be an empty string initially.");
        }

        [TestMethod]
        public void ParameterizedConstructor_SetsPropertiesCorrectly()
        {
            // Arrange
            int id = 1;
            string name = "Test Roadmap";

            // Act
            var roadmap = new Roadmap(id, name);

            // Assert
            Assert.AreEqual(id, roadmap.Id);
            Assert.AreEqual(name, roadmap.Name);
            Assert.IsNotNull(roadmap.Sections, "Sections list should be initialized.");
            Assert.AreEqual(0, roadmap.Sections.Count);
        }

        [TestMethod]
        public void AddSection_AddsSectionToSectionsList()
        {
            // Arrange
            var roadmap = new Roadmap(1, "Roadmap");
            var section = new Section();

            // Act
            roadmap.AddSection(section);

            // Assert
            Assert.AreEqual(1, roadmap.Sections.Count);
            Assert.AreEqual(section, roadmap.Sections[0]);
        }

        [TestMethod]
        public void RemoveSection_RemovesSectionFromSectionsList()
        {
            // Arrange
            var roadmap = new Roadmap(1, "Roadmap");
            var section1 = new Section();
            var section2 = new Section();
            roadmap.AddSection(section1);
            roadmap.AddSection(section2);

            // Act
            roadmap.RemoveSection(section1);

            // Assert
            Assert.AreEqual(1, roadmap.Sections.Count);
            Assert.AreEqual(section2, roadmap.Sections[0]);
        }

        [TestMethod]
        public void GetAllSections_ReturnsAllAddedSections()
        {
            // Arrange
            var roadmap = new Roadmap(1, "Roadmap");
            var section1 = new Section();
            var section2 = new Section();
            roadmap.AddSection(section1);
            roadmap.AddSection(section2);

            // Act
            IEnumerable<Section> sections = roadmap.GetAllSections();

            // Assert
            CollectionAssert.AreEqual(new List<Section> { section1, section2 }, new List<Section>(sections));
        }

        [TestMethod]
        public void ToString_ReturnsExpectedFormat()
        {
            // Arrange
            var roadmap = new Roadmap(2, "Sample Roadmap");
            roadmap.AddSection(new Section());
            roadmap.AddSection(new Section());

            // Act
            string result = roadmap.ToString();

            // Assert
            string expected = "Roadmap 2: Sample Roadmap - 2 sections";
            Assert.AreEqual(expected, result);
        }
    }
}
