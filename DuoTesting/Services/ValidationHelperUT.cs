using Microsoft.VisualStudio.TestTools.UnitTesting;
using Duo.Models;
using Duo.Models.Quizzes;
using Duo.Models.Exercises;
using Duo.Models.Sections;
using System;
using System.Collections.Generic;
using Duo.Services;
using System.ComponentModel.DataAnnotations;

namespace DuoTesting.Services
{
    [TestClass]
    public class ValidationHelperUT
    {
        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void ValidateSection_ShouldThrowIfTitleIsEmpty()
        {
            // Arrange
            var section = new Section
            {
                Title = "", // Empty title
                Description = "Valid description",
                Quizzes = new List<Quiz> { new Quiz(1, 1, 1) },
                Exam = new Exam(1, 1)
            };

            // Act
            ValidationHelper.ValidateSection(section);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void ValidateSection_ShouldThrowIfDescriptionIsEmpty()
        {
            // Arrange
            var section = new Section
            {
                Title = "Valid title",
                Description = "", // Empty description
                Quizzes = new List<Quiz> { new Quiz(1, 1, 1) },
                Exam = new Exam(1, 1)
            };

            // Act
            ValidationHelper.ValidateSection(section);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void ValidateSection_ShouldThrowIfQuizCountIsInvalid()
        {
            // Arrange
            var section = new Section
            {
                Title = "Valid title",
                Description = "Valid description",
                Quizzes = new List<Quiz> { new Quiz(1, 1, 1) }, // Less than 2 quizzes
                Exam = new Exam(1, 1)
            };

            // Act
            ValidationHelper.ValidateSection(section);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void ValidateSection_ShouldThrowIfExamIsNull()
        {
            // Arrange
            var section = new Section
            {
                Title = "Valid title",
                Description = "Valid description",
                Quizzes = new List<Quiz> { new Quiz(1, 1, 1) },
                Exam = null // Null exam
            };

            // Act
            ValidationHelper.ValidateSection(section);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void ValidateQuiz_ShouldThrowIfExerciseListIsInvalid()
        {
            // Arrange
            var quiz = new Quiz(1, 1, 1)
            {
                ExerciseList = new List<Exercise>() // Empty list of exercises
            };

            // Act
            ValidationHelper.ValidateQuiz(quiz);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void ValidateQuiz_ShouldThrowIfOrderNumberIsInvalid()
        {
            // Arrange
            var quiz = new Quiz(1, null, -1);

            // Act
            ValidationHelper.ValidateQuiz(quiz);
        }

        [TestMethod]
        public void ValidateMultipleChoiceExercise_ShouldPassIfValid()
        {
            // Arrange
            var mcExercise = new MultipleChoiceExercise(1, "What is 2 + 2?", Difficulty.Normal, new List<MultipleChoiceAnswerModel>
            {
                new MultipleChoiceAnswerModel { Answer = "4", IsCorrect = true },
                new MultipleChoiceAnswerModel { Answer = "5", IsCorrect = false }
            });

            // Act & Assert
            ValidationHelper.ValidateMultipleChoiceExercise(mcExercise); // Should not throw
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void ValidateMultipleChoiceExercise_ShouldThrowIfInvalidChoicesCount()
        {
            // Arrange
            var mcExercise = new MultipleChoiceExercise(1, "What is 2 + 2?", Difficulty.Normal, new List<MultipleChoiceAnswerModel>
            {
                new MultipleChoiceAnswerModel { Answer = "4", IsCorrect = true }
            });

            // Act
            ValidationHelper.ValidateMultipleChoiceExercise(mcExercise); // Should throw due to invalid number of choices
        }
    }
}
