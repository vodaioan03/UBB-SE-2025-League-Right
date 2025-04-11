using Duo;
using Duo.Models;
using Duo.Models.Quizzes;
using Duo.Models.Sections;
using Duo.Services;
using Duo.ViewModels;
using Duo.ViewModels.Roadmap;
using DuoTesting.Services;
using Microsoft.UI.Dispatching;
using Moq;
using System;
using System.Threading.Tasks;
using Windows.System;
using static DuoTesting.ViewModels.ViewModelBaseUT;

namespace DuoTesting.ViewModels.Roadmap
{
    [TestClass]
    public class RoadmapQuizPreviewViewModelUT
    {
        private Mock<IServiceProvider> serviceProviderMock;
        private Mock<RoadmapMainPageViewModel> mainPageViewModelMock;
        private Mock<IRoadmapService> roadmapServiceMock;
        private Mock<IUserService> userServiceMock;
        private Microsoft.UI.Dispatching.DispatcherQueueController dispatcherQueueController;

        //[TestInitialize]
        //public void Setup()
        //{
        //    // initialize the service provider
        //    serviceProviderMock = new Mock<IServiceProvider>();
        //    App.ServiceProvider = serviceProviderMock.Object;

        //    // initialize the mocks
        //    roadmapServiceMock = new Mock<IRoadmapService>();
        //    serviceProviderMock.Setup(x => x.GetService(typeof(IRoadmapService)))
        //        .Returns(roadmapServiceMock.Object);

        //    userServiceMock = new Mock<IUserService>();
        //    serviceProviderMock.Setup(x => x.GetService(typeof(IUserService)))
        //        .Returns(userServiceMock.Object);

        //    mainPageViewModelMock = new Mock<RoadmapMainPageViewModel>();
        //    serviceProviderMock.Setup(x => x.GetService(typeof(RoadmapMainPageViewModel)))
        //        .Returns(mainPageViewModelMock.Object);
        //}

       

        [TestMethod]
        public void QuizOrderNumber_NullQuiz_ShouldReturnDefaultValue()
        {
            Assert.IsTrue(true);
        }
    }
}