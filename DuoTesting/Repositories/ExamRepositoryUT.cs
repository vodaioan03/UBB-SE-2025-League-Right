using Microsoft.VisualStudio.TestTools.UnitTesting;
using Duo.Models.Quizzes;
using Duo.Repositories;
using DuoTesting.MockClasses; 
using System.Threading.Tasks;
using System;

namespace DuoTesting.Repositories
{
    [TestClass]
    public class ExamRepositoryUT
    {
        private IExamRepository _repository = null!;

        [TestInitialize]
        public void Setup()
        {
            _repository = new InMemoryExamRepository(); 
        }

        [TestMethod]
        public async Task AddAndGetById_ShouldReturnSameExam()
        {
            var newExam = new Exam(0, null);

            var newId = await _repository.AddAsync(newExam);
            var examFromDb = await _repository.GetByIdAsync(newId);

            Assert.IsNotNull(examFromDb);
            Assert.AreEqual(newId, examFromDb.Id);
            Assert.IsNull(examFromDb.SectionId);

            await _repository.DeleteAsync(newId);
        }

        [TestMethod]
        public async Task GetAllAsync_ShouldReturnExams()
        {
            await _repository.AddAsync(new Exam(0, 1));
            await _repository.AddAsync(new Exam(0, null));

            var exams = await _repository.GetAllAsync();
            Assert.AreEqual(2, exams.Count);
        }

        [TestMethod]
        public async Task GetUnassignedAsync_ShouldReturnUnassignedExams()
        {
            await _repository.AddAsync(new Exam(0, null));
            await _repository.AddAsync(new Exam(0, 1));

            var unassigned = await _repository.GetUnassignedAsync();
            Assert.AreEqual(1, unassigned.Count);
        }

        [TestMethod]
        public async Task AddUpdateDelete_ShouldUpdateExamSection()
        {
            var examId = await _repository.AddAsync(new Exam(0, 5));

            await _repository.UpdateExamSection(examId, null);
            var updated = await _repository.GetByIdAsync(examId);
            Assert.IsNull(updated.SectionId);

            await _repository.DeleteAsync(examId);
        }

        [TestMethod]
        public async Task GetBySectionIdAsync_ShouldReturnExam()
        {
            var examId = await _repository.AddAsync(new Exam(0, 100));
            var result = await _repository.GetBySectionIdAsync(100);

            Assert.IsNotNull(result);
            Assert.AreEqual(examId, result!.Id);
        }

        [TestMethod]
        public async Task Delete_NonExistent_ShouldThrow()
        {
            await Assert.ThrowsExceptionAsync<KeyNotFoundException>(async () =>
            {
                await _repository.DeleteAsync(-999);
            });
        }

        [TestMethod]
        public async Task AddExerciseToExam_And_RemoveExerciseFromExam()
        {
            int examId = await _repository.AddAsync(new Exam(0, null));
            int exerciseId = 1;

            await _repository.AddExerciseToExam(examId, exerciseId);
            await _repository.RemoveExerciseFromExam(examId, exerciseId);
            await _repository.DeleteAsync(examId);
        }

        [TestMethod]
        public async Task UpdateAsync_ShouldUpdateExam()
        {
            var examId = await _repository.AddAsync(new Exam(0, null));

            var updatedExam = new Exam(examId, 55);
            await _repository.UpdateAsync(updatedExam);

            var result = await _repository.GetByIdAsync(examId);
            Assert.AreEqual(55, result.SectionId);

            await _repository.DeleteAsync(examId);
        }
    }
}
