using Duo.Models.Exercises;
using Duo.Models.Quizzes;
using Duo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuoTesting.ModelTesting.QuizzesTesting
{
    [TestClass]
    public class ExamTests
    {
        [TestMethod]
        public void Exam_AddExercise_UnderLimit_ReturnsTrue()
        {
            // Arrange
            var exam = new Exam(10, 3);
            var exercise = new DummyExercise(1, "Dummy question", Difficulty.Easy);

            // Act
            bool result = exam.AddExercise(exercise);

            // Assert
            Assert.IsTrue(result, "Should be able to add an exercise under the limit.");
            Assert.AreEqual(1, exam.ExerciseList.Count);
        }

        [TestMethod]
        public void Exam_AddExercise_AtLimit_ReturnsFalse()
        {
            // Arrange
            var exam = new Exam(10, 3);
            // Fill exam with maximum (25) exercises
            for (int i = 0; i < 25; i++)
            {
                bool added = exam.AddExercise(new DummyExercise(i, $"Question {i}", Difficulty.Normal));
                Assert.IsTrue(added, $"Exercise {i + 1} should be added successfully.");
            }

            // Act
            bool result = exam.AddExercise(new DummyExercise(100, "Extra question", Difficulty.Hard));

            // Assert
            Assert.IsFalse(result, "Adding an exercise beyond the limit should fail.");
            Assert.AreEqual(25, exam.ExerciseList.Count);
        }

        [TestMethod]
        public void Exam_IsValid_ReturnsFalse_WhenNotFull()
        {
            // Arrange
            var exam = new Exam(10, 3);
            for (int i = 0; i < 20; i++)
            {
                exam.AddExercise(new DummyExercise(i, $"Question {i}", Difficulty.Normal));
            }

            // Act
            bool isValid = exam.IsValid();

            // Assert
            Assert.IsFalse(isValid, "Exam is valid only if exactly 25 exercises are added.");
        }

        [TestMethod]
        public void Exam_IsValid_ReturnsTrue_WhenFull()
        {
            // Arrange
            var exam = new Exam(10, 3);
            for (int i = 0; i < 25; i++)
            {
                exam.AddExercise(new DummyExercise(i, $"Question {i}", Difficulty.Normal));
            }

            // Act
            bool isValid = exam.IsValid();

            // Assert
            Assert.IsTrue(isValid, "Exam should be valid when 25 exercises have been added.");
        }

        [TestMethod]
        public void Exam_GetPassingThreshold_Returns90()
        {
            // Arrange
            var exam = new Exam(10, 3);

            // Act
            double threshold = exam.GetPassingThreshold();

            // Assert
            Assert.AreEqual(90, threshold, "Exam passing threshold should be 90.");
        }

        [TestMethod]
        public void Exam_ToString_ReturnsExpectedFormat()
        {
            // Arrange
            int id = 10;
            int sectionId = 3;
            var exam = new Exam(id, sectionId);
            // Add a few exercises (e.g. 5 exercises)
            for (int i = 0; i < 5; i++)
            {
                exam.AddExercise(new DummyExercise(i, $"Question {i}", Difficulty.Normal));
            }
            string expected = $"Quiz {id} (Section: {sectionId}) - {exam.ExerciseList.Count}/25 exercises - Not started [Final Exam]";

            // Act
            string result = exam.ToString();

            // Assert
            Assert.AreEqual(expected, result);
        }
    }
}
