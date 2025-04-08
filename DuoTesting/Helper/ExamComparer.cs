using Duo.Models.Quizzes;
using System.Collections.Generic;

namespace DuoTesting.Helpers
{
    public class ExamComparer : IEqualityComparer<Exam>
    {
        public bool Equals(Exam? x, Exam? y)
        {
            if (x is null && y is null) return true;
            if (x is null || y is null) return false;

            return x.Id == y.Id &&
                   x.SectionId == y.SectionId;
        }

        public int GetHashCode(Exam obj)
        {
            return HashCode.Combine(obj.Id, obj.SectionId);
        }
    }
}
