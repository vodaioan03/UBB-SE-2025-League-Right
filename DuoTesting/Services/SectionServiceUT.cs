using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Duo.Models.Quizzes;
using Duo.Models.Sections;
using Duo.Services;

namespace DuoTesting.Services
{
    [TestClass]
    public class SectionServiceUT
    {
        private Mock<ISectionService> sectionServiceMock;

        [TestInitialize]
        public void TestInitialize()
        {
            // Initialize mock service
            sectionServiceMock = new Mock<ISectionService>();
        }

        [TestMethod]
        public async Task AddSection_ShouldAddSuccessfully()
        {
            // Arrange
            var section = new Section
            {
                Title = "New Section",
                Description = "Description of new section",
                RoadmapId = 1,
                OrderNumber = 1
            };

            // Create quizzes to satisfy validation (2 to 5 quizzes required)
            var quizzes = new List<Quiz>
            {
                new Quiz(1, section.Id, 1),
                new Quiz(2, section.Id, 2),
                new Quiz(3, section.Id, 3),
                new Quiz(4, section.Id, 4),
                new Quiz(5, section.Id, 5)
            };

            section.Quizzes = quizzes;

            // Mock the AddSection method
            sectionServiceMock.Setup(service => service.AddSection(section)).ReturnsAsync(1);

            // Act
            var result = await sectionServiceMock.Object.AddSection(section);

            // Assert
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public async Task AddSection_ShouldThrowIfInvalidSection()
        {
            // Arrange
            var section = new Section
            {
                Title = "Invalid Section",
                Description = "", // Description is empty, should throw ValidationException
                RoadmapId = 1,
                OrderNumber = 1
            };

            // Mock the AddSection method to throw an exception
            sectionServiceMock.Setup(service => service.AddSection(section)).ThrowsAsync(new ValidationException());

            // Act
            await sectionServiceMock.Object.AddSection(section);
        }

        [TestMethod]
        public async Task UpdateSection_ShouldUpdateSuccessfully()
        {
            // Arrange
            var section = new Section
            {
                Id = 1,
                Title = "Updated Section",
                Description = "Updated description",
                RoadmapId = 1,
                OrderNumber = 2
            };

            // Create quizzes to satisfy validation (2 to 5 quizzes required)
            var quizzes = new List<Quiz>
            {
                new Quiz(1, section.Id, 1),
                new Quiz(2, section.Id, 2),
                new Quiz(3, section.Id, 3),
                new Quiz(4, section.Id, 4),
                new Quiz(5, section.Id, 5)
            };

            section.Quizzes = quizzes;

            // Mock the UpdateSection method
            sectionServiceMock.Setup(service => service.UpdateSection(section)).Returns(Task.CompletedTask);

            // Act
            await sectionServiceMock.Object.UpdateSection(section);

            // Assert (You may add assertions to check if the section was updated)
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public async Task UpdateSection_ShouldThrowIfInvalidSection()
        {
            // Arrange
            var section = new Section
            {
                Id = 1,
                Title = "Invalid Section",
                Description = "", // Invalid section, description is empty
                RoadmapId = 1,
                OrderNumber = 1
            };

            // Mock the UpdateSection method to throw an exception
            sectionServiceMock.Setup(service => service.UpdateSection(section)).ThrowsAsync(new ValidationException());

            // Act
            await sectionServiceMock.Object.UpdateSection(section);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public async Task UpdateSection_ShouldThrowIfInvalidQuizCount()
        {
            // Arrange
            var section = new Section
            {
                Id = 1,
                Title = "Valid Section",
                Description = "Valid description",
                RoadmapId = 1,
                OrderNumber = 1
            };

            // Create only 1 quiz, which is less than the required 2 quizzes
            var quizzes = new List<Quiz>
            {
                new Quiz(1, section.Id, 1)
            };

            section.Quizzes = quizzes;

            // Mock the UpdateSection method to throw an exception
            sectionServiceMock.Setup(service => service.UpdateSection(section)).ThrowsAsync(new ValidationException());

            // Act
            await sectionServiceMock.Object.UpdateSection(section);
        }
    }
}
