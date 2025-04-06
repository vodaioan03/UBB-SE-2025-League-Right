using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Duo.Data;
using Duo.Repositories;
using Duo.Models;
using Duo.Models.Exercises;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace DuoTesting.Repositories
{
    [TestClass]
    public class ExerciseRepositoryUT
    {
        private IExerciseRepository _repository = null!;
        private DatabaseConnection _dbConnection = null!;

        [TestInitialize]
        public void Setup()
        {
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { "DbConnection", "Server=DESKTOP-BVGO48P\\SQLEXPRESS;Database=new-league;Trusted_Connection=True;TrustServerCertificate=true;" }
                })
                .Build();

            _dbConnection = new DatabaseConnection(config);
            _repository = new ExerciseRepository(_dbConnection);
        }

        [TestMethod]
        public async Task AddGetDelete_MultipleChoiceExercise_WorksCorrectly()
        {
            var exercise = new MultipleChoiceExercise(
                id: 0,
                question: "What is 2 + 2?",
                difficulty: Difficulty.Easy,
                choices: new List<MultipleChoiceAnswerModel>
                {
                    new MultipleChoiceAnswerModel("4", true),
                    new MultipleChoiceAnswerModel("3", false),
                    new MultipleChoiceAnswerModel("5", false)
                });

            int id = await _repository.AddExerciseAsync(exercise);
            Assert.IsTrue(id > 0);

            var fromDb = await _repository.GetByIdAsync(id);
            Assert.AreEqual("What is 2 + 2?", fromDb.Question);
            Assert.IsInstanceOfType(fromDb, typeof(MultipleChoiceExercise));

            await _repository.DeleteExerciseAsync(id);
        }
        [TestMethod]
        public async Task AddDelete_FlashcardExercise_ShouldWork()
        {
            var exercise = new FlashcardExercise(
                id: 0,
                question: "Capital of France?",
                answer: "Paris",
                difficulty: Difficulty.Easy);

            int id = await _repository.AddExerciseAsync(exercise);
            Assert.IsTrue(id > 0);

            var fetched = await _repository.GetByIdAsync(id);
            Assert.AreEqual("Paris", ((FlashcardExercise)fetched).Answer);

            await _repository.DeleteExerciseAsync(id);
        }
        [TestMethod]
        public async Task AddDelete_FillInTheBlankExercise_ShouldWork()
        {
            var exercise = new FillInTheBlankExercise(
                id: 0,
                question: "____ is the largest planet.",
                difficulty: Difficulty.Easy,
                possibleCorrectAnswers: new List<string> { "Jupiter" });

            int id = await _repository.AddExerciseAsync(exercise);
            var result = await _repository.GetByIdAsync(id);
            Assert.IsInstanceOfType(result, typeof(FillInTheBlankExercise));

            await _repository.DeleteExerciseAsync(id);
        }

        [TestMethod]
        public async Task AddDelete_AssociationExercise_ShouldWork()
        {
            var exercise = new AssociationExercise(
                0,                               
                "Match countries to capitals",    
                Difficulty.Hard,                 
                new List<string> { "Germany" },   
                new List<string> { "Berlin" }     
            );

            int id = await _repository.AddExerciseAsync(exercise);
            var result = await _repository.GetByIdAsync(id);
            Assert.IsInstanceOfType(result, typeof(AssociationExercise));

            await _repository.DeleteExerciseAsync(id);
        }
        [TestMethod]
        public async Task AddExerciseAsync_Null_ShouldThrow()
        {
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => _repository.AddExerciseAsync(null!));
        }

        [TestMethod]
        public async Task GetByIdAsync_Invalid_ShouldThrow()
        {
            await Assert.ThrowsExceptionAsync<ArgumentException>(() => _repository.GetByIdAsync(0));
        }

        [TestMethod]
        public async Task DeleteExerciseAsync_Invalid_ShouldThrow()
        {
            await Assert.ThrowsExceptionAsync<ArgumentException>(() => _repository.DeleteExerciseAsync(0));
        }
    }
}
