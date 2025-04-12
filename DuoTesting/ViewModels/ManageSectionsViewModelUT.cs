using Duo;
using Duo.Models.Quizzes;
using Duo.Models.Sections;
using Duo.Services;
using Duo.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace DuoTesting.ViewModels
{
    [TestClass]
    public class ManageSectionsViewModelUT
    {
        private Mock<IServiceProvider> _serviceProviderMock;
        private Mock<ISectionService> _sectionServiceMock;
        private Mock<IQuizService> _quizServiceMock;
        private Quiz _quiz;
        private Section _section;

        [TestInitialize]
        public void TestInitialize()
        {
            _serviceProviderMock = new Mock<IServiceProvider>();
            App.ServiceProvider = _serviceProviderMock.Object;

            // Mock the services
            _sectionServiceMock = new Mock<ISectionService>();
            _serviceProviderMock
                .Setup(sp => sp.GetService(typeof(ISectionService)))
                .Returns(_sectionServiceMock.Object);

            _quizServiceMock = new Mock<IQuizService>();
            _serviceProviderMock
                .Setup(sp => sp.GetService(typeof(IQuizService)))
                .Returns(_quizServiceMock.Object);

            _quiz = new Quiz(1, 1, 1);
            _section = new Section(1, 1, "title", "description", 1, 1);
        }

        [TestMethod]
        public void UpdateSectionQuizes_ShouldUpdateSectionQuizes()
        {
            // Arrange
            _quizServiceMock
                .Setup(q => q.GetAllQuizzesFromSection(It.IsAny<int>()))
                .ReturnsAsync(new List<Quiz> { _quiz });
            // sectionserv GetAllSections
            _sectionServiceMock
                .Setup(s => s.GetAllSections())
                .ReturnsAsync(new List<Section> { _section });
            var viewModel = new ManageSectionsViewModel();
            viewModel.SelectedSection = _section;
            // Act
            viewModel.UpdateSectionQuizes(viewModel.SelectedSection);
            // Assert
            Assert.AreEqual(1, viewModel.SectionQuizes.Count);
        }

        [TestMethod]
        public void LoadSectionsAsync_ShouldLoadSections()
        {
            // Arrange
            _sectionServiceMock
                .Setup(s => s.GetAllSections())
                .ReturnsAsync(new List<Section> { _section });
            var viewModel = new ManageSectionsViewModel();
            // Act
            viewModel.LoadSectionsAsync();
            // Assert
            Assert.AreEqual(1, viewModel.Sections.Count);
        }

        [TestMethod]
        public void DeleteSection_ShouldDeleteSection()
        {
            // Arrange
            _sectionServiceMock
                .Setup(s => s.DeleteSection(It.IsAny<int>()))
                .Returns(Task.CompletedTask);
            var viewModel = new ManageSectionsViewModel();
            viewModel.Sections.Add(_section);
            // Act
            viewModel.DeleteSection(_section);
            // Assert
            Assert.AreEqual(0, viewModel.Sections.Count);
            _sectionServiceMock.Verify(s => s.DeleteSection(_section.Id), Times.Once);
        }
    }
}
