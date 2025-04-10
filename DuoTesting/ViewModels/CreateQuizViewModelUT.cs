using Duo;
using Duo.Models;
using Duo.Models.Exercises;
using Duo.Models.Quizzes;
using Duo.Services;
using Duo.ViewModels;
using Moq;
using System;
using System.Diagnostics;
using System.IO;

namespace DuoTesting.ViewModels
{

    [TestClass]
    public class CreateQuizViewModelUT
    {
        private Mock<IQuizService> mockQuizService;
        private Mock<IExerciseService> mockExerciseService;
        private Mock<IServiceProvider> mockServiceProvider;
        private const int MAX_EXERCISES = 10;

        [TestInitialize]
        public void SetUp()
        {
            mockQuizService = new Mock<IQuizService>();
            mockExerciseService = new Mock<IExerciseService>();
            mockServiceProvider = new Mock<IServiceProvider>();

            // Setup service provider
            mockServiceProvider.Setup(sp => sp.GetService(typeof(IQuizService)))
                               .Returns(mockQuizService.Object);
            mockServiceProvider.Setup(sp => sp.GetService(typeof(IExerciseService)))
                               .Returns(mockExerciseService.Object);

            // Setup mock exercises
            var exercise1 = new AssociationExercise(0, "Question 1", Difficulty.Easy, new List<string>(), new List<string>());
            var exercise2 = new AssociationExercise(1, "Question 2", Difficulty.Normal, new List<string>(), new List<string>());
            var exercises = new List<Exercise> { exercise1, exercise2 };
            mockExerciseService.Setup(s => s.GetAllExercises())
                               .ReturnsAsync(exercises);

            // Assign it to App.ServiceProvider
            App.ServiceProvider = mockServiceProvider.Object;
        }

        [TestMethod]
        public async Task LoadExercisesAsync_ShouldPopulateExercises()
        {
            // Act
            var vm = new CreateQuizViewModel();
            await Task.Delay(100); // wait for async loading
                                   // Assert
            Assert.AreEqual(2, vm.Exercises.Count);
        }

        [TestMethod]
        public void OpenSelectExercises_ShouldInvokeShowListViewModal()
        {
            var vm = new CreateQuizViewModel();
            var e1 = vm.Exercises[0];
            var e2 = vm.Exercises[1];
            vm.AddExercise(e1); // selected

            List<Exercise> result = new List<Exercise>();
            vm.ShowListViewModal += (list) => result = list;

            vm.OpenSelectExercises();

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(e2, result[0]);
        }

        [TestMethod]
        public void AddExercise_ShouldAdd_IfUnderLimit()
        {
            var vm = new CreateQuizViewModel();

            var exercise = new AssociationExercise(2, "Question 3", Difficulty.Easy, new List<string>(), new List<string>());
            vm.AddExercise(exercise);

            Assert.IsTrue(vm.SelectedExercises.Contains(exercise));
        }

        [TestMethod]
        public void AddExercise_ShouldNotAdd_IfOverLimit()
        {
            var vm = new CreateQuizViewModel();

            while (vm.SelectedExercises.Count < MAX_EXERCISES)
            {
                vm.AddExercise(new AssociationExercise(vm.SelectedExercises.Count, "Question", Difficulty.Easy, new List<string>(), new List<string>()));
            }


            var extra = new AssociationExercise(25, "Question", Difficulty.Easy, new List<string>(), new List<string>());
            vm.AddExercise(extra);

            Assert.IsFalse(vm.SelectedExercises.Contains(extra));
        }

        [TestMethod]
        public void RemoveExercise_ShouldRemove_IfExists()
        {
            var vm = new CreateQuizViewModel();
            var exercise = new AssociationExercise(100, "Question", Difficulty.Easy, new List<string>(), new List<string>());
            vm.AddExercise(exercise);

            vm.RemoveExercise(exercise);

            Assert.IsFalse(vm.SelectedExercises.Contains(exercise));
        }

        [TestMethod]
        public async Task CreateQuiz_ShouldCallQuizServiceCreateExam()
        {
            var vm = new CreateQuizViewModel();
            var e1 = vm.Exercises[0];
            var e2 = vm.Exercises[1];
            vm.AddExercise(e1);
            vm.AddExercise(e2);

            mockQuizService.Setup(q => q.CreateExam(It.IsAny<Exam>())).ReturnsAsync(123); // mock returning an exam ID

            //redirect Debug output to check it here
            var stringWriter = new StringWriter();
            var listener = new TextWriterTraceListener(stringWriter);
            Trace.Listeners.Add(listener);

            try
            {
                await vm.CreateQuiz();
                await Task.Delay(100); // wait for async operation

                var output = stringWriter.ToString();
                Assert.IsTrue(output.Contains("Quiz 0 (Section: 1) - 2/10 exercises - Not started - Order: 0"));
            }
            finally
            {
                Trace.Listeners.Remove(listener);
                listener.Dispose();
            }
        }
    }
}
