using Duo.Models.Quizzes;
using Duo.Models.Exercises;
using System.Collections.Generic;

namespace DuoTesting.Helper
{
    public class BaseQuizComparer<T> : IEqualityComparer<T> where T : BaseQuiz
    {
        public virtual bool Equals(T? x, T? y)
        {
            if (x is null && y is null) return true;
            if (x is null || y is null) return false;

            return x.Id == y.Id &&
                   x.SectionId == y.SectionId &&
                   CompareExerciseList(x.ExerciseList, y.ExerciseList);
        }

        private bool CompareExerciseList(List<Exercise> list1, List<Exercise> list2)
        {
            if (list1.Count != list2.Count) return false;

            for (int i = 0; i < list1.Count; i++)
            {
                if (!Equals(list1[i], list2[i]))
                    return false;
            }

            return true;
        }

        public virtual int GetHashCode(T obj)
        {
            int hash = HashCode.Combine(obj.Id, obj.SectionId);
            foreach (var exercise in obj.ExerciseList)
            {
                hash = HashCode.Combine(hash, exercise.GetHashCode());
            }
            return hash;
        }
    }
}
