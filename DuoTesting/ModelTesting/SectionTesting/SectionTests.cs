using Duo.Models.Quizzes;
using Duo.Models.Sections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuoTesting.ModelTesting.SectionTesting
{
    [TestClass]
    public class SectionTests
    {
        [TestMethod]
        public void DefaultConstructor_InitializesEmptyQuizList()
        {
            // Act
            var section = new Section();

            // Assert
            Assert.IsNotNull(section.Quizzes, "Quizzes list should be initialized.");
            Assert.AreEqual(0, section.Quizzes.Count, "Quizzes list should be empty initially.");
            Assert.IsNull(section.Exam, "Exam should be null initially.");
        }

        [TestMethod]
        public void ParameterizedConstructor_SetsPropertiesCorrectly()
        {
            // Arrange
            int id = 10;
            int? subjectId = 100;
            string title = "Section Title";
            string description = "Section Description";
            int roadmapId = 1;
            int? orderNumber = 2;

            // Act
            var section = new Section(id, subjectId, title, description, roadmapId, orderNumber);

            // Assert
            Assert.AreEqual(id, section.Id);
            Assert.AreEqual(subjectId, section.SubjectId);
            Assert.AreEqual(title, section.Title);
            Assert.AreEqual(description, section.Description);
            Assert.AreEqual(roadmapId, section.RoadmapId);
            Assert.AreEqual(orderNumber, section.OrderNumber);
            Assert.IsNotNull(section.Quizzes, "Quizzes list should be initialized.");
            Assert.AreEqual(0, section.Quizzes.Count);
            Assert.IsNull(section.Exam, "Exam should be null initially.");
        }

        [TestMethod]
        public void AddQuiz_UnderLimit_AddsQuizAndReturnsTrue()
        {
            // Arrange
            var section = new Section();
            var quiz = new Quiz(1, 1, 1);

            // Act
            bool result = section.AddQuiz(quiz);

            // Assert
            Assert.IsTrue(result, "Adding a quiz under the limit should return true.");
            Assert.AreEqual(1, section.Quizzes.Count);
        }

        [TestMethod]
        public void AddQuiz_AtLimit_ReturnsFalse()
        {
            // Arrange
            var section = new Section();
            // MAX_QUIZZES is 5; add 5 quizzes.
            for (int i = 0; i < 5; i++)
            {
                bool added = section.AddQuiz(new Quiz(i, 1, i));
                Assert.IsTrue(added, $"Quiz {i + 1} should be added successfully.");
            }

            // Act
            bool result = section.AddQuiz(new Quiz(100, 1, 100));

            // Assert
            Assert.IsFalse(result, "Adding a quiz beyond the limit should return false.");
            Assert.AreEqual(5, section.Quizzes.Count);
        }

        [TestMethod]
        public void AddExam_WhenNotSet_AddsExamAndReturnsTrue()
        {
            // Arrange
            var section = new Section();
            var exam = new Exam(1, 1);

            // Act
            bool result = section.AddExam(exam);

            // Assert
            Assert.IsTrue(result, "Adding an exam when none exists should return true.");
            Assert.AreEqual(exam, section.Exam);
        }

        [TestMethod]
        public void AddExam_WhenAlreadySet_ReturnsFalse()
        {
            // Arrange
            var section = new Section();
            var exam1 = new Exam(1, 1);
            var exam2 = new Exam(2, 1);
            bool firstAdd = section.AddExam(exam1);
            Assert.IsTrue(firstAdd, "First exam addition should succeed.");

            // Act
            bool result = section.AddExam(exam2);

            // Assert
            Assert.IsFalse(result, "Adding a second exam should return false.");
            Assert.AreEqual(exam1, section.Exam);
        }

        [TestMethod]
        public void IsValid_ReturnsFalse_WhenQuizzesBelowMinimum()
        {
            // Arrange
            var section = new Section();
            // MIN_QUIZZES is 2; add only one quiz.
            section.AddQuiz(new Quiz(1, 1, 1));
            section.AddExam(new Exam(1, 1));

            // Act
            bool isValid = section.IsValid();

            // Assert
            Assert.IsFalse(isValid, "Section should be invalid if fewer than 2 quizzes are added.");
        }

        [TestMethod]
        public void IsValid_ReturnsFalse_WhenExamIsNull()
        {
            // Arrange
            var section = new Section();
            // Add two quizzes to meet MIN_QUIZZES.
            section.AddQuiz(new Quiz(1, 1, 1));
            section.AddQuiz(new Quiz(2, 1, 2));

            // Act
            bool isValid = section.IsValid();

            // Assert
            Assert.IsFalse(isValid, "Section should be invalid if exam is not set.");
        }

        [TestMethod]
        public void IsValid_ReturnsTrue_WhenMinimumQuizzesAndExamSet()
        {
            // Arrange
            var section = new Section();
            section.AddQuiz(new Quiz(1, 1, 1));
            section.AddQuiz(new Quiz(2, 1, 2));
            var exam = new Exam(1, 1);
            section.AddExam(exam);

            // Act
            bool isValid = section.IsValid();

            // Assert
            Assert.IsTrue(isValid, "Section should be valid when minimum quizzes and exam are set.");
        }

        [TestMethod]
        public void GetAllQuizzes_ReturnsAllAddedQuizzes()
        {
            // Arrange
            var section = new Section();
            var quiz1 = new Quiz(1, 1, 1);
            var quiz2 = new Quiz(2, 1, 2);
            section.AddQuiz(quiz1);
            section.AddQuiz(quiz2);

            // Act
            IEnumerable<Quiz> quizzes = section.GetAllQuizzes();

            // Assert
            CollectionAssert.AreEqual(new List<Quiz> { quiz1, quiz2 }, quizzes.ToList());
        }

        [TestMethod]
        public void GetFinalExam_ReturnsAddedExam()
        {
            // Arrange
            var section = new Section();
            var exam = new Exam(1, 1);
            section.AddExam(exam);

            // Act
            var finalExam = section.GetFinalExam();

            // Assert
            Assert.AreEqual(exam, finalExam);
        }

        [TestMethod]
        public void ToString_ReturnsExpectedFormat_WithExam()
        {
            // Arrange
            var section = new Section(10, 1, "Section Title", "Description", 5, 2);
            section.AddQuiz(new Quiz(1, 1, 1));
            section.AddQuiz(new Quiz(2, 1, 2));
            section.AddQuiz(new Quiz(3, 1, 3));
            var exam = new Exam(1, 1);
            section.AddExam(exam);

            // Act
            string result = section.ToString();

            // Assert
            string expected = "Section 10: Section Title - 3 quizzes with Exam";
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void ToString_ReturnsExpectedFormat_WithoutExam()
        {
            // Arrange
            var section = new Section(20, 2, "Another Section", "Description", 6, 1);
            section.AddQuiz(new Quiz(1, 2, 1));
            section.AddQuiz(new Quiz(2, 2, 2));

            // Act
            string result = section.ToString();

            // Assert
            string expected = "Section 20: Another Section - 2 quizzes without Exam";
            Assert.AreEqual(expected, result);
        }
    }
}
