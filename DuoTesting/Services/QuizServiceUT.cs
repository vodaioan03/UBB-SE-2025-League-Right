using Duo.Models;
using Duo.Models.Exercises;
using Duo.Models.Quizzes;
using Duo.Repositories;
using Duo.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Duo.Tests.Services
{
    [TestClass]
    public class QuizServiceUT
    {
        private Mock<IQuizRepository> quizRepositoryMock;
        private Mock<IExamRepository> examRepositoryMock;
        private QuizService quizService;

        [TestInitialize]
        public void Setup()
        {
            quizRepositoryMock = new Mock<IQuizRepository>();
            examRepositoryMock = new Mock<IExamRepository>();
            quizService = new QuizService(quizRepositoryMock.Object, examRepositoryMock.Object);
        }

        [TestMethod]
        public async Task Get_ShouldReturnAllQuizzes()
        {
            var quizzes = new List<Quiz>();
            quizRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(quizzes);
            var result = await quizService.Get();
            Assert.AreEqual(quizzes, result);
        }

        [TestMethod]
        public async Task GetAllAvailableExams_ShouldReturnAllAvailableExams()
        {
            var exams = new List<Exam>();
            examRepositoryMock.Setup(r => r.GetUnassignedAsync()).ReturnsAsync(exams);
            var result = await quizService.GetAllAvailableExams();
            Assert.AreEqual(exams, result);
        }

        [TestMethod]
        public async Task GetQuizById_ShouldReturnQuiz()
        {
            var quiz = new Quiz(1, null, 1);
            quizRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(quiz);
            var result = await quizService.GetQuizById(1);
            Assert.AreEqual(quiz, result);
        }

        [TestMethod]
        public async Task GetExamById_ShouldReturnExam()
        {
            var exam = new Exam(1, null);
            examRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(exam);
            var result = await quizService.GetExamById(1);
            Assert.AreEqual(exam, result);
        }

        [TestMethod]
        public async Task GetAllQuizzesFromSection_ShouldReturnQuizzes()
        {
            var quizzes = new List<Quiz>();
            quizRepositoryMock.Setup(r => r.GetBySectionIdAsync(1)).ReturnsAsync(quizzes);
            var result = await quizService.GetAllQuizzesFromSection(1);
            Assert.AreEqual(quizzes, result);
        }

        [TestMethod]
        public async Task CountQuizzesFromSection_ShouldReturnCount()
        {
            quizRepositoryMock.Setup(r => r.CountBySectionIdAsync(1)).ReturnsAsync(3);
            var result = await quizService.CountQuizzesFromSection(1);
            Assert.AreEqual(3, result);
        }

        [TestMethod]
        public async Task LastOrderNumberFromSection_ShouldReturnOrder()
        {
            quizRepositoryMock.Setup(r => r.LastOrderNumberBySectionIdAsync(1)).ReturnsAsync(5);
            var result = await quizService.LastOrderNumberFromSection(1);
            Assert.AreEqual(5, result);
        }

        [TestMethod]
        public async Task GetExamFromSection_ShouldReturnExam()
        {
            var exam = new Exam(1, null);
            examRepositoryMock.Setup(r => r.GetBySectionIdAsync(1)).ReturnsAsync(exam);
            var result = await quizService.GetExamFromSection(1);
            Assert.AreEqual(exam, result);
        }

        [TestMethod]
        public async Task DeleteQuiz_ShouldCallDelete()
        {
            await quizService.DeleteQuiz(1);
            quizRepositoryMock.Verify(r => r.DeleteAsync(1), Times.Once);
        }

        [TestMethod]
        public async Task UpdateQuiz_ShouldValidateAndCallUpdate()
        {
            var quiz = new Quiz(1, null, 1)
            {
                // Create exactly 10 exercises for the quiz and cast to List<Exercise>
                ExerciseList = Enumerable.Range(1, 10).Select(i => new MultipleChoiceExercise(i, $"Question {i}", Difficulty.Easy, new List<MultipleChoiceAnswerModel>
        {
            new MultipleChoiceAnswerModel("Answer A", true),
            new MultipleChoiceAnswerModel("Answer B", false)
        })).Cast<Exercise>().ToList()
            };

            await quizService.UpdateQuiz(quiz);
            quizRepositoryMock.Verify(r => r.UpdateAsync(quiz), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public async Task UpdateQuiz_ShouldThrowValidationException()
        {
            var quiz = new Quiz(1, null, 1) { ExerciseList = new List<Exercise>() };
            await quizService.UpdateQuiz(quiz);
        }

        [TestMethod]
        public async Task CreateQuiz_ShouldValidateAndReturnId()
        {
            var quiz = new Quiz(1, null, 1)
            {
                // Create exactly 10 exercises for the quiz and cast to List<Exercise>
                ExerciseList = Enumerable.Range(1, 10).Select(i => new MultipleChoiceExercise(i, $"Question {i}", Difficulty.Easy, new List<MultipleChoiceAnswerModel>
        {
            new MultipleChoiceAnswerModel("Answer A", true),
            new MultipleChoiceAnswerModel("Answer B", false)
        })).Cast<Exercise>().ToList()
            };

            quizRepositoryMock.Setup(r => r.AddAsync(quiz)).ReturnsAsync(1);
            var result = await quizService.CreateQuiz(quiz);
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public async Task CreateQuiz_ShouldThrowValidationException()
        {
            var quiz = new Quiz(1, null, 1) { ExerciseList = new List<Exercise>() };
            await quizService.CreateQuiz(quiz);
        }

        [TestMethod]
        public async Task AddExercisesToQuiz_ShouldCallAddForEach()
        {
            var exercises = new List<Exercise> { new MultipleChoiceExercise(1, "Q", Difficulty.Easy, new List<MultipleChoiceAnswerModel> { new MultipleChoiceAnswerModel("A", true), new MultipleChoiceAnswerModel("B", false) }), new MultipleChoiceExercise(2, "Q", Difficulty.Easy, new List<MultipleChoiceAnswerModel> { new MultipleChoiceAnswerModel("A", true), new MultipleChoiceAnswerModel("B", false) }) };
            await quizService.AddExercisesToQuiz(1, exercises);
            quizRepositoryMock.Verify(r => r.AddExerciseToQuiz(1, 1), Times.Once);
            quizRepositoryMock.Verify(r => r.AddExerciseToQuiz(1, 2), Times.Once);
        }

        [TestMethod]
        public async Task AddExerciseToQuiz_ShouldCallAdd()
        {
            await quizService.AddExerciseToQuiz(1, 1);
            quizRepositoryMock.Verify(r => r.AddExerciseToQuiz(1, 1), Times.Once);
        }

        [TestMethod]
        public async Task RemoveExerciseFromQuiz_ShouldCallRemove()
        {
            await quizService.RemoveExerciseFromQuiz(1, 1);
            quizRepositoryMock.Verify(r => r.RemoveExerciseFromQuiz(1, 1), Times.Once);
        }

        [TestMethod]
        public async Task DeleteExam_ShouldCallDelete()
        {
            await quizService.DeleteExam(1);
            examRepositoryMock.Verify(r => r.DeleteAsync(1), Times.Once);
        }


        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public async Task UpdateExam_ShouldThrowValidationException()
        {
            var exam = new Exam(1, null) { ExerciseList = new List<Exercise>() };
            await quizService.UpdateExam(exam);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public async Task CreateExam_ShouldThrowValidationException()
        {
            var exam = new Exam(1, null) { ExerciseList = new List<Exercise>() };
            await quizService.CreateExam(exam);
        }
    }
}
