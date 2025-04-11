using Duo;
using Duo.Models;
using Duo.Models.Quizzes;
using Duo.Models.Roadmap;
using Duo.Models.Sections;
using Duo.Services;
using Duo.ViewModels.Roadmap;
using Duo.Views.Components;
using Moq;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace DuoTesting.ViewModels.Roadmap
{
    [TestClass]
    public class RoadmapMainPageViewModelUT
    {
        private Mock<IRoadmapService> roadmapServiceMock;
        private Mock<IUserService> userServiceMock;
        private Mock<ISectionService> sectionServiceMock;
        private Mock<IQuizService> quizServiceMock;
        private Mock<ObservableCollection<RoadmapSectionViewModel>> sectionViewModelsMock;
        private Mock<IServiceProvider> serviceProviderMock;

        [TestInitialize]
        public void SetUp()
        {
            roadmapServiceMock = new Mock<IRoadmapService>();
            userServiceMock = new Mock<IUserService>();
            sectionServiceMock = new Mock<ISectionService>();
            quizServiceMock = new Mock<IQuizService>();
            sectionViewModelsMock = new Mock<ObservableCollection<RoadmapSectionViewModel>>();

            serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(x => x.GetService(typeof(IRoadmapService)))
                .Returns(roadmapServiceMock.Object);
            serviceProviderMock.Setup(x => x.GetService(typeof(IUserService)))
                .Returns(userServiceMock.Object);
            serviceProviderMock.Setup(x => x.GetService(typeof(ISectionService)))
                .Returns(sectionServiceMock.Object);
            serviceProviderMock.Setup(x => x.GetService(typeof(IQuizService)))
                .Returns(quizServiceMock.Object);

            App.ServiceProvider = serviceProviderMock.Object;
        }

        [TestMethod]
        public async Task SetupViewModelUT()
        {
            // Arrange
            var roadmap = new Duo.Models.Roadmap.Roadmap(1, "Roadmap Name");
            var user = new User("user");
            var section = new Section();
            section.Id = 123;
            section.Quizzes = [];
            roadmapServiceMock.Setup(x => x.GetRoadmapById(1))
                .ReturnsAsync(roadmap);
            userServiceMock.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(user);
            sectionServiceMock.Setup(x => x.GetByRoadmapId(1))
                .ReturnsAsync(new List<Section> { section });
            sectionServiceMock.Setup(x => x.GetSectionById(123))
                .ReturnsAsync(section);
            quizServiceMock.Setup(x => x.GetAllQuizzesFromSection(123))
                .ReturnsAsync(new List<Quiz>());

            var roadmapMainPageViewModel = new RoadmapMainPageViewModel();
            serviceProviderMock.Setup(x => x.GetService(typeof(RoadmapMainPageViewModel)))
                .Returns(roadmapMainPageViewModel);
            serviceProviderMock.Setup(x => x.GetService(typeof(RoadmapSectionViewModel)))
                .Returns(() => new RoadmapSectionViewModel());


            // Act
            await roadmapMainPageViewModel.SetupViewModel();
        }

        [TestMethod]
        public void OpenQuizPreviewUT()
        {
            // Arrange
            var roadmapMainPageViewModel = new RoadmapMainPageViewModel();
            var quiz = new Quiz(1, 123, 1);
            var section = new Section();
            section.Id = 123;
            section.Quizzes = new List<Quiz> { quiz };
            var tuple = new Tuple<int, bool>(123, true);

            // Act
            roadmapMainPageViewModel.OpenQuizPreviewCommand.Execute(tuple);
            
            // Assert
            Assert.IsTrue(roadmapMainPageViewModel.OpenQuizPreviewCommand.CanExecute(tuple));
        }

        [TestMethod]
        public void StartQuizUT()
        {
            // Arrange
            var roadmapMainPageViewModel = new RoadmapMainPageViewModel();
            var quiz = new Quiz(1, 123, 1);
            var section = new Section();
            section.Id = 123;
            section.Quizzes = new List<Quiz> { quiz };
            var tuple = new Tuple<int, bool>(123, true);

            // Act
            roadmapMainPageViewModel.StartQuizCommand.Execute(tuple);

            // Assert
            Assert.IsTrue(roadmapMainPageViewModel.StartQuizCommand.CanExecute(tuple));
        }
    }
}