using Duo;
using Duo.Models;
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
    public class ManageExamsViewModelUT
    {
        private Mock<IServiceProvider> _serviceProviderMock;
        private Mock<IExerciseService> _exerciseServiceMock;
        private Mock<IQuizService> _quizServiceMock;
        private Exercise exercise1;
        private FlashcardExercise exercise2;

        [TestInitialize]
        public void setup()
        {
            _serviceProviderMock = new Mock<IServiceProvider>();
            App.ServiceProvider = _serviceProviderMock.Object;

            // Setup mock services
            _exerciseServiceMock = new Mock<IExerciseService>();
            _serviceProviderMock.Setup(sp => sp.GetService(typeof(IExerciseService)))
                                .Returns(_exerciseServiceMock.Object);

            _quizServiceMock = new Mock<IQuizService>();
            _serviceProviderMock.Setup(sp => sp.GetService(typeof(IQuizService)))
                                .Returns(_quizServiceMock.Object);

            exercise1 = new FlashcardExercise(1, "question1", "answer1");
            exercise2 = new FlashcardExercise(2, "question2", "answer2");
        }

        [TestMethod]
        public void SetSelectedExam_ShouldSetSelectedExam()
        {
            // Arrange
            var viewModel = new ManageExamsViewModel();
            var exam = new Exam(1, 1);
            // Act
            viewModel.SelectedExam = exam;
            // Assert
            Assert.AreEqual(exam, viewModel.SelectedExam);
        }

        [TestMethod]
        public void UpdateExamExercises_ShouldUpdateExamExercises()
        {
            // Arrange
            var viewModel = new ManageExamsViewModel();
            _exerciseServiceMock.Setup(_exerciseServiceMock => _exerciseServiceMock.GetAllExercisesFromExam(It.IsAny<int>()))
                                .ReturnsAsync(new List<Exercise>(new[] { exercise1, exercise2 }));
            var exam = new Exam(1, 1);
            // Act
            viewModel.UpdateExamExercises(exam);
            // Assert
            Assert.AreEqual(2, viewModel.ExamExercises.Count);
        }

        [TestMethod]
        public void DeleteExam_ShouldDeleteExam()
        {
            // Arrange
            var viewModel = new ManageExamsViewModel();
            var exam = new Exam(1, 1);
            // DeleteExam from ManageExamsViewModel
            _quizServiceMock.Setup(_quizServiceMock => _quizServiceMock.DeleteExam(It.IsAny<int>()))
                            .Returns(Task.CompletedTask);
            // Act
            viewModel.DeleteExam(exam);
            // Assert
            _quizServiceMock.Verify(q => q.DeleteExam(exam.Id), Times.Once);
        }

        [TestMethod]
        public void InitializeViewModel_ShouldInitializeViewModel()
        {
            // Arrange
            var viewModel = new ManageExamsViewModel();
            _quizServiceMock.Setup(_quizServiceMock => _quizServiceMock.GetAllAvailableExams())
                            .ReturnsAsync(new List<Exam> { new Exam(1, 1) });
            // Act
            viewModel.InitializeViewModel();
            // Assert
            Assert.AreEqual(1, viewModel.Exams.Count);
        }

        [TestMethod]
        public void OpenSelectExercises_ShouldOpenSelectExercises()
        {
            // Arrange
            var exercises = new List<Exercise> { exercise1, exercise2 };
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
        public void AddExercise_ShouldAddExercise()
        {
            // Arrange
            var viewModel = new ManageExamsViewModel();
            var exam = new Exam(1, 1);
            viewModel.SelectedExam = exam;
            _quizServiceMock.Setup(_quizServiceMock => _quizServiceMock.AddExerciseToQuiz(It.IsAny<int>(), It.IsAny<int>()))
                            .Returns(Task.CompletedTask);
            // Act
            viewModel.AddExercise(exercise1);
            // Assert
            _quizServiceMock.Verify(q => q.AddExerciseToQuiz(exam.Id, exercise1.Id), Times.Once);
        }

        [TestMethod]
        public void RemoveExerciseFromExam_ShouldRemoveExercise()
        {
            // Arrange
            var viewModel = new ManageExamsViewModel();
            var exam = new Exam(1, 1);
            viewModel.SelectedExam = exam;
            _quizServiceMock.Setup(_quizServiceMock => _quizServiceMock.RemoveExerciseFromQuiz(It.IsAny<int>(), It.IsAny<int>()))
                            .Returns(Task.CompletedTask);
            // Act
            viewModel.RemoveExerciseFromExam(exercise1);
            // Assert
            _quizServiceMock.Verify(q => q.RemoveExerciseFromQuiz(exam.Id, exercise1.Id), Times.Once);
        }
    }
}
