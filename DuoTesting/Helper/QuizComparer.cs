using Duo.Models.Quizzes;
using System.Collections.Generic;

namespace DuoTesting.Helpers
{
    public class QuizComparer : IEqualityComparer<Quiz>
    {
        public bool Equals(Quiz? x, Quiz? y)
        {
            if (x is null && y is null) return true;
            if (x is null || y is null) return false;

            return x.Id == y.Id &&
                   x.SectionId == y.SectionId &&
                   x.OrderNumber == y.OrderNumber;
        }

        public int GetHashCode(Quiz obj)
        {
            return HashCode.Combine(obj.Id, obj.SectionId, obj.OrderNumber);
        }
    }
}
