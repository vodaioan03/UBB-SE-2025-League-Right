using Duo.Models;
using Duo.Models.Exercises;
using Duo.ViewModels.CreateExerciseViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuoTesting.ViewModels.CreateExerciseViewModels
{
    [TestClass]
    public class CreateFlashcardExerciseViewModelUT
    {

        [TestMethod]
        public void CreateExercise_ValidInput_ReturnsFlashcardExercise()
        {
            // Arrange
            var viewModel = new CreateFlashcardExerciseViewModel();
            string question = "What is the capital of France?";
            viewModel.Answer = "Paris";
            Difficulty difficulty = Difficulty.Easy;
            // Act
            var exercise = viewModel.CreateExercise(question, difficulty);
            // Assert
            Assert.IsInstanceOfType(exercise, typeof(FlashcardExercise));
            Assert.AreEqual(question, exercise.Question);
            Assert.AreEqual("Paris", ((FlashcardExercise)exercise).Answer);
            Assert.AreEqual(difficulty, exercise.Difficulty);
        }
    }
}
