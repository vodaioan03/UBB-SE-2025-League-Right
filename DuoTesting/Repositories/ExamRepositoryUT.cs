using Microsoft.VisualStudio.TestTools.UnitTesting;
using Duo.Models.Quizzes;
using Duo.Repositories;
using DuoTesting.MockClasses;
using DuoTesting.Helper; 
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

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

            var expected = new Exam(newId, null);
            var actual = await _repository.GetByIdAsync(newId);

            Assert.IsTrue(new ExamComparer().Equals(expected, actual));

            await _repository.DeleteAsync(newId);
        }

        [TestMethod]
        public async Task GetAllAsync_ShouldReturnExams()
        {
            var e1 = new Exam(0, 1);
            var e2 = new Exam(0, null);

            var id1 = await _repository.AddAsync(e1);
            var id2 = await _repository.AddAsync(e2);

            var expected = new List<Exam>
            {
                new Exam(id1, 1),
                new Exam(id2, null)
            };

            var actual = await _repository.GetAllAsync();

            var comparer = new ExamComparer();
            Assert.AreEqual(expected.Count, actual.Count, "Count mismatch.");

            for (int i = 0; i < expected.Count; i++)
            {
                Assert.IsTrue(comparer.Equals(expected[i], actual[i]), $"Exam at index {i} does not match.");
            }
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

            var expected = new Exam(examId, null);
            Assert.IsTrue(new ExamComparer().Equals(expected, updated));

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

            var actual = await _repository.GetByIdAsync(examId);
            Assert.IsTrue(new ExamComparer().Equals(updatedExam, actual));

            await _repository.DeleteAsync(examId);
        }

        [TestMethod]
        public async Task GetByIdAsync_NonExistent_ShouldThrow()
        {
            await Assert.ThrowsExceptionAsync<KeyNotFoundException>(async () =>
            {
                await _repository.GetByIdAsync(-1);
            });
        }

        [TestMethod]
        public async Task AddExerciseToExam_NonExistentExam_ShouldThrow()
        {
            await Assert.ThrowsExceptionAsync<KeyNotFoundException>(async () =>
            {
                await _repository.AddExerciseToExam(-123, 99);
            });
        }

        [TestMethod]
        public async Task RemoveExerciseFromExam_NonExistentExam_ShouldThrow()
        {
            await Assert.ThrowsExceptionAsync<KeyNotFoundException>(async () =>
            {
                await _repository.RemoveExerciseFromExam(-999, 88);
            });
        }

        [TestMethod]
        public async Task UpdateExamSection_NonExistent_ShouldThrow()
        {
            await Assert.ThrowsExceptionAsync<KeyNotFoundException>(async () =>
            {
                await _repository.UpdateExamSection(-1, 123);
            });
        }

        [TestMethod]
        public async Task UpdateAsync_NonExistent_ShouldThrow()
        {
            var exam = new Exam(9999, 1);
            await Assert.ThrowsExceptionAsync<KeyNotFoundException>(async () =>
            {
                await _repository.UpdateAsync(exam);
            });
        }

        [TestMethod]
        public async Task GetExercisesForExam_ShouldReturnAddedExercises()
        {
            int examId = await _repository.AddAsync(new Exam(0, null));
            await _repository.AddExerciseToExam(examId, 101);
            await _repository.AddExerciseToExam(examId, 202);

            var result = _repository.GetExercisesForExam(examId);

            CollectionAssert.AreEquivalent(new List<int> { 101, 202 }, result.ToList());
        }

        [TestMethod]
        public void GetExercisesForExam_NonExistent_ShouldThrow()
        {
            Assert.ThrowsException<KeyNotFoundException>(() =>
            {
                _repository.GetExercisesForExam(-42);
            });
        }


        [TestMethod]
        public void ClearAll_ShouldResetRepository()
        {
            var inMemory = new InMemoryExamRepository();
            var idTask = inMemory.AddAsync(new Exam(0, 1));
            idTask.Wait();

            inMemory.ClearAll();

            Assert.ThrowsException<KeyNotFoundException>(() =>
            {
                inMemory.GetExercisesForExam(idTask.Result);
            });
        }

    }
}
