using Duo;
using Duo.Models.Exercises;
using Duo.Models.Quizzes;
using Duo.Services;
using Duo.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace DuoTesting.ViewModels
{
    [TestClass]
    public class ManageQuizesViewModelUT
    {
        private Mock<IServiceProvider> _serviceProviderMock;
        private Mock<IExerciseService> _exerciseServiceMock;
        private Mock<IQuizService> _quizServiceMock;
        private FlashcardExercise exercise1;
        private FlashcardExercise exercise2;
        private List<Exercise> exercises;

        [TestInitialize]
        public void TestInitialize()
        {
            _serviceProviderMock = new Mock<IServiceProvider>();
            App.ServiceProvider = _serviceProviderMock.Object;

            // Mock the services
            _exerciseServiceMock = new Mock<IExerciseService>();
            _serviceProviderMock
                .Setup(sp => sp.GetService(typeof(IExerciseService)))
                .Returns(_exerciseServiceMock.Object);

            _quizServiceMock = new Mock<IQuizService>();
            _serviceProviderMock
                .Setup(sp => sp.GetService(typeof(IQuizService)))
                .Returns(_quizServiceMock.Object);

            exercise1 = new FlashcardExercise(1, "question1", "answer1");
            exercise2 = new FlashcardExercise(2, "question2", "answer2");
            exercises = new List<Exercise> { exercise1, exercise2 };
        }

        [TestMethod]
        public void DeleteQuiz_ShouldCallDeleteQuizInService()
        {
            // Arrange
            var quiz = new Quiz(1, 1, 1);
            _quizServiceMock.Setup(q => q.DeleteQuiz(quiz.Id)).Returns(Task.CompletedTask);
            var viewModel = new ManageQuizesViewModel();
            // Act
            viewModel.DeleteQuizCommand.Execute(quiz);
            // Assert
            _quizServiceMock.Verify(q => q.DeleteQuiz(quiz.Id), Times.Once);
        }

        [TestMethod]
        public void DeleteQuiz_ShouldUpdateSelectedQuiz()
        {
            // Arrange
            var quiz = new Quiz(1, 1, 1);
            _quizServiceMock.Setup(q => q.DeleteQuiz(quiz.Id)).Returns(Task.CompletedTask);
            var viewModel = new ManageQuizesViewModel();
            viewModel.SelectedQuiz = quiz;
            // Act
            viewModel.DeleteQuizCommand.Execute(quiz);
            // Assert
            Assert.IsNull(viewModel.SelectedQuiz);
        }

        [TestMethod]
        public void DeleteQuiz_ShouldAddExercisesToAvailableExercises()
        {
            // Arrange
            var quiz = new Quiz(1, 1, 1);
            quiz.ExerciseList = new List<Exercise> { exercise1, exercise2 };
            _quizServiceMock.Setup(q => q.DeleteQuiz(quiz.Id)).Returns(Task.CompletedTask);
            var viewModel = new ManageQuizesViewModel();
            viewModel.SelectedQuiz = quiz;
            // Act
            viewModel.DeleteQuizCommand.Execute(quiz);
            // Assert
            Assert.IsTrue(viewModel.AvailableExercises.Contains(exercise1));
            Assert.IsTrue(viewModel.AvailableExercises.Contains(exercise2));
        }

        [TestMethod]
        public async Task InitializeViewModel_ShouldLoadExercisesAsync()
        {
            // Arrange
            _exerciseServiceMock.Setup(es => es.GetAllExercises()).ReturnsAsync(exercises);
            var viewModel = new ManageQuizesViewModel();
            // Act
            await viewModel.InitializeViewModel();
            // Assert
            Assert.AreEqual(2, viewModel.AvailableExercises.Count);
            Assert.IsTrue(viewModel.AvailableExercises.Contains(exercise1));
            Assert.IsTrue(viewModel.AvailableExercises.Contains(exercise2));
        }

        [TestMethod]
        public async Task UpdateQuizExercises_ShouldUpdateQuizExercises()
        {
            // Arrange
            _exerciseServiceMock.Setup(es => es.GetAllExercisesFromQuiz(It.IsAny<int>())).ReturnsAsync(exercises);
            var quiz = new Quiz(1, 1, 1);
            var viewModel = new ManageQuizesViewModel();
            // Act
            await viewModel.UpdateQuizExercises(quiz);
            // Assert
            Assert.AreEqual(2, viewModel.QuizExercises.Count);
        }

        [TestMethod]
        public async Task OpenSelectExercises_ShouldShowListViewModal()
        {
            // Arrange
            _exerciseServiceMock.Setup(_exerciseServiceMock => _exerciseServiceMock.GetAllExercises())
                                .ReturnsAsync(exercises);
            var viewModel = new ManageExamsViewModel();
            viewModel.ShowListViewModal += (exercises) =>
            {
                // Assert
                Assert.AreEqual(2, exercises.Count);
            };
            // Act
            viewModel.OpenSelectExercises();
        }

        [TestMethod]
        public void AddExercise_ShouldAddExerciseToQuiz()
        {
            // Arrange
            _quizServiceMock.Setup(q => q.AddExerciseToQuiz(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(Task.CompletedTask);
            _exerciseServiceMock.Setup(es => es.GetAllExercisesFromQuiz(It.IsAny<int>()))
                .ReturnsAsync(new List<Exercise> { exercise1 });
            var viewModel = new ManageQuizesViewModel();
            var quiz = new Quiz(1, 1, 1);
            viewModel.SelectedQuiz = quiz;
            viewModel.QuizExercises.Clear();
            // Act
            viewModel.AddExercise(exercise1);
            // Assert
            Assert.IsTrue(viewModel.QuizExercises.Contains(exercise1));
            _quizServiceMock.Verify(q => q.AddExerciseToQuiz(quiz.Id, exercise1.Id), Times.Once);
        }

        [TestMethod]
        public void RemoveExercise_ShouldRemoveExerciseFromQuiz()
        {
            // Arrange
            _quizServiceMock.Setup(q => q.RemoveExerciseFromQuiz(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(Task.CompletedTask);
            _exerciseServiceMock.Setup(es => es.GetAllExercisesFromQuiz(It.IsAny<int>()))
                .ReturnsAsync(new List<Exercise> { exercise1 });
            var viewModel = new ManageQuizesViewModel();
            var quiz = new Quiz(1, 1, 1);
            viewModel.SelectedQuiz = quiz;
            viewModel.QuizExercises.Add(exercise1);
            // Act
            viewModel.RemoveExerciseFromQuiz(exercise1);
            // Assert
            _quizServiceMock.Verify(q => q.RemoveExerciseFromQuiz(quiz.Id, exercise1.Id), Times.Once);
        }
    }
}
