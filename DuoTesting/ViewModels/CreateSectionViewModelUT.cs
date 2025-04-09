using Duo.Services;
using Moq;
using Duo;
using Duo.ViewModels;
using Duo.Models.Exercises;
using Duo.Models;
using Duo.Models.Quizzes;
using System.Diagnostics;
using Duo.Models.Sections;


namespace DuoTesting.ViewModels
{

    [TestClass]
    public class CreateSectionViewModelUT
    {
        private Mock<IQuizService> mockQuizService;
        private Mock<IExerciseService> mockExerciseService;
        private Mock<ISectionService> mockSectionService;
        private Mock<IServiceProvider> mockServiceProvider;
        public TestContext TestContext { get; set; } = null!;

        [TestInitialize]
        public void SetUp()
        {
            mockQuizService = new Mock<IQuizService>();
            mockExerciseService = new Mock<IExerciseService>();
            mockSectionService = new Mock<ISectionService>();
            mockServiceProvider = new Mock<IServiceProvider>();

            // Setup service provider
            mockServiceProvider.Setup(sp => sp.GetService(typeof(IQuizService)))
                               .Returns(mockQuizService.Object);
            mockServiceProvider.Setup(sp => sp.GetService(typeof(IExerciseService)))
                               .Returns(mockExerciseService.Object);
            mockServiceProvider.Setup(sp => sp.GetService(typeof(ISectionService)))
                                 .Returns(mockSectionService.Object);

            // Setup mock exercises
            var exercise1 = new AssociationExercise(0, "Question 1", Difficulty.Easy, new List<string>(), new List<string>());
            var exercise2 = new AssociationExercise(1, "Question 2", Difficulty.Normal, new List<string>(), new List<string>());
            var exercises = new List<Exercise> { exercise1, exercise2 };
            mockExerciseService.Setup(s => s.GetAllExercises())
                               .ReturnsAsync(exercises);
            mockExerciseService.Setup(s => s.GetAllExercisesFromQuiz(It.IsAny<int>()))
                               .ReturnsAsync(exercises);
            mockExerciseService.Setup(s => s.GetAllExercisesFromExam(It.IsAny<int>()))
                                 .ReturnsAsync(exercises);

            // Setup mock quiz service
            var quiz1 = new Quiz(0, 1, 1);
            var quiz2 = new Quiz(1, 1, 1);
            var quizzes = new List<Quiz> { quiz1, quiz2 };
            mockQuizService.Setup(s => s.GetAllQuizzesFromSection(It.IsAny<int>()))
                           .ReturnsAsync(quizzes);
            mockQuizService.Setup(s => s.UpdateQuiz(It.IsAny<Quiz>()));

            // mock GetExamFromSection from mockQuizService
            var exam1 = new Exam(0, 1);
            var exam2 = new Exam(1, 1);
            var exams = new List<Exam> { exam1, exam2 };
            mockQuizService.Setup(s => s.GetExamFromSection(It.IsAny<int>()))
                           .ReturnsAsync(exam1);

            //mock sectionService AddService
            mockSectionService.Setup(s => s.AddSection(It.IsAny<Section>()))
                             .ReturnsAsync(123);


            // Assign it to App.ServiceProvider
            App.ServiceProvider = mockServiceProvider.Object;
        }

        [TestMethod]
        public async Task GetQuizesAsync_ShouldPopulateQuizes()
        {
            // Act
            var vm = new CreateSectionViewModel();

            await vm.GetQuizesAsync();
            await Task.Delay(100); // wait for async loading

            // Assert
            Assert.AreEqual(2, vm.Quizes.Count);
        }

        [TestMethod]
        public async Task GetExamAsync_ShouldAddExamToList()
        {
            // Act
            var vm = new CreateSectionViewModel();
            await vm.GetExamAsync();
            await Task.Delay(100); // wait for async loading
            // Assert
            Assert.AreEqual(2, vm.Exams.Count);
        }


        [TestMethod]
        public async Task OpenSelectQuizes_ShouldInvokeShowListViewModalQuizes()
        {
            // Arrange
            var vm = new CreateSectionViewModel();
            await vm.GetQuizesAsync();
            var quiz1 = vm.Quizes[0];
            var quiz2 = vm.Quizes[1];
            List<Quiz> result = new List<Quiz>();
            vm.ShowListViewModalQuizes += (list) => result = list;
            // Act
            vm.OpenSelectQuizes();
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(quiz1, result[0]);
        }

        [TestMethod]
        public async Task OpenSelectExams_ShouldInvokeShowListViewModalExams()
        {
            // Arrange
            var vm = new CreateSectionViewModel();
            await vm.GetExamAsync();
            await Task.Delay(100); // wait for async loading
            var exam1 = vm.Exams[0];
            var exam2 = vm.Exams[1];
            List<Exam> result = new List<Exam>();
            vm.ShowListViewModalExams += (list) => result = list;

            // Act
            vm.OpenSelectExams();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(exam1, result[0]);

        }

        [TestMethod]
        public void GetAvailableExams_ShouldPopulateAvailableExams()
        {
            // Arrange
            var vm = new CreateSectionViewModel();
            var exam1 = new Exam(0, 1);

            // Act
            List<Exam> result = vm.GetAvailableExams();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void GetAvailableQuizes_ShouldPopulateAvailableQuizes()
        {
            // Arrange
            var vm = new CreateSectionViewModel();
            var quiz1 = new Quiz(0, 1, 1);
            var quiz2 = new Quiz(1, 1, 1);
            vm.SelectedQuizes.Add(quiz1);
            vm.Quizes.Add(quiz1);
            vm.Quizes.Add(quiz2);
            // Act
            List<Quiz> result = vm.GetAvailableQuizes();
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(quiz2, result[0]);
        }

        [TestMethod]
        public async Task RemoveSelectedQuiz_ShouldRemoveQuizFromSelectedQuizes()
        {
            // Arrange
            var vm = new CreateSectionViewModel();
            await vm.GetQuizesAsync();
            var quiz1 = vm.Quizes[0];
            var quiz2 = vm.Quizes[1];
            vm.SelectedQuizes.Add(quiz1);
            vm.SelectedQuizes.Add(quiz2);
            // Act
            vm.RemoveSelectedQuiz(quiz1);
            // Assert
            Assert.IsFalse(vm.SelectedQuizes.Contains(quiz1));
            Assert.IsTrue(vm.SelectedQuizes.Contains(quiz2));
        }

        [TestMethod]
        public async Task AddQuiz_ShouldAddQuizToSelectedQuizes()
        {
            // Arrange
            var vm = new CreateSectionViewModel();
            await vm.GetQuizesAsync();
            var quiz1 = vm.Quizes[0];
            var quiz2 = vm.Quizes[1];
            // Act
            vm.AddQuiz(quiz1);
            vm.AddQuiz(quiz2);
            // Assert
            Assert.IsTrue(vm.SelectedQuizes.Contains(quiz1));
            Assert.IsTrue(vm.SelectedQuizes.Contains(quiz2));
        }

        [TestMethod]
        public async Task AddExam_ShouldAddExamToSelectedExams()
        {
            // Arrange
            var vm = new CreateSectionViewModel();
            await vm.GetExamAsync();
            var exam1 = vm.Exams[0];
            var exam2 = vm.Exams[1];
            // Act
            vm.AddExam(exam1);
            vm.AddExam(exam2);
            // Assert
            Assert.IsTrue(vm.SelectedExams.Contains(exam1));
            Assert.IsTrue(vm.SelectedExams.Contains(exam2));
        }


        [TestMethod]
        public async Task CreateSection_ShouldCallSectionService()
        {
            // Arrange
            var vm = new CreateSectionViewModel();
            await vm.GetQuizesAsync();
            vm.SubjectText = "Math";
            vm.SelectedQuizes.Add(vm.Quizes[0]);
            vm.SelectedQuizes.Add(vm.Quizes[1]);
            vm.SelectedExams.Add(vm.Exams[0]);

            
            //check for message exam was created
            //redirect Debug output to check it here
            var stringWriter = new StringWriter();
            var listener = new TextWriterTraceListener(stringWriter);
            Trace.Listeners.Add(listener);

            try
            {
                // Act
                await vm.CreateSection();
                // Assert

                await Task.Delay(100); // wait for async operation

                var output = stringWriter.ToString();
                Assert.IsTrue(output.Contains("Section created"));
            }
            finally
            {
                Trace.Listeners.Remove(listener);
                listener.Dispose();
            }

        }


    }
}
