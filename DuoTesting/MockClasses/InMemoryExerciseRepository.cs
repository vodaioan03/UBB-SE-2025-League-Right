using Duo.Models.Exercises;
using Duo.Models;
using Duo.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace DuoTesting.MockClasses
{
    public class InMemoryExerciseRepository : IExerciseRepository
    {
        private readonly Dictionary<int, Exercise> _exercises = new();
        private int _nextId = 1;

        public Task<int> AddExerciseAsync(Exercise exercise)
        {
            if (exercise is null)
                throw new ArgumentNullException(nameof(exercise));

            var clone = CloneWithNewId(exercise, _nextId++);
            _exercises[clone.Id] = clone;
            return Task.FromResult(clone.Id);
        }

        public Task<Exercise> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid ID", nameof(id));

            if (!_exercises.TryGetValue(id, out var exercise))
                throw new KeyNotFoundException($"Exercise with ID {id} not found.");

            return Task.FromResult(exercise);
        }

        public Task DeleteExerciseAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid ID", nameof(id));

            _exercises.Remove(id);
            return Task.CompletedTask;
        }

        public Task<List<Exercise>> GetAllExercisesAsync()
        {
            return Task.FromResult(_exercises.Values.ToList());
        }

        public Task<List<Exercise>> GetQuizExercisesAsync(int quizId)
        {
            return Task.FromResult(_exercises.Values.ToList());
        }

        public Task<List<Exercise>> GetExamExercisesAsync(int examId)
        {
            return Task.FromResult(_exercises.Values.ToList());
        }

        public void ClearAll()
        {
            _exercises.Clear();
            _nextId = 1;
        }

        private Exercise CloneWithNewId(Exercise exercise, int newId)
        {
            return exercise switch
            {
                MultipleChoiceExercise mc => new MultipleChoiceExercise(
                    newId,
                    mc.Question,
                    mc.Difficulty,
                    mc.Choices.Select(c => new MultipleChoiceAnswerModel(c.Answer, c.IsCorrect)).ToList()),

                FlashcardExercise f => new FlashcardExercise(
                    newId,
                    f.Question,
                    f.Answer,
                    f.TimeInSeconds,
                    f.Difficulty),

                FillInTheBlankExercise fib => new FillInTheBlankExercise(
                    newId,
                    fib.Question,
                    fib.Difficulty,
                    new List<string>(fib.PossibleCorrectAnswers)),

                AssociationExercise a => new AssociationExercise(
                    newId,
                    a.Question,
                    a.Difficulty,
                    new List<string>(a.FirstAnswersList),
                    new List<string>(a.SecondAnswersList)),

                _ => throw new NotSupportedException($"Exercise type '{exercise.GetType().Name}' is not supported.")
            };
        }
    }
}
