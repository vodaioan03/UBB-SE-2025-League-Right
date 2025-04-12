using Duo.Models.Sections;
using Duo.Models.Quizzes;
using DuoTesting.Helpers;
using System.Collections.Generic;
using System.Linq;
using DuoTesting.Helper;
using System;

namespace DuoTesting.Helpers
{
    public class SectionComparer : IEqualityComparer<Section>
    {
        private readonly QuizComparer _quizComparer = new();
        private readonly ExamComparer _examComparer = new();

        public bool Equals(Section? x, Section? y)
        {
            if (x is null || y is null) return false;

            return x.Id == y.Id &&
                   x.SubjectId == y.SubjectId &&
                   x.Title == y.Title &&
                   x.Description == y.Description &&
                   x.RoadmapId == y.RoadmapId &&
                   x.OrderNumber == y.OrderNumber &&
                   CompareQuizzes(x.Quizzes, y.Quizzes) &&
                   CompareExam(x.Exam, y.Exam);
        }

        public int GetHashCode(Section obj)
        {
            int hash = HashCode.Combine(obj.Id, obj.SubjectId, obj.Title, obj.Description, obj.RoadmapId, obj.OrderNumber);
            foreach (var quiz in obj.Quizzes)
            {
                hash = HashCode.Combine(hash, _quizComparer.GetHashCode(quiz));
            }
            if (obj.Exam != null)
            {
                hash = HashCode.Combine(hash, _examComparer.GetHashCode(obj.Exam));
            }
            return hash;
        }

        private bool CompareQuizzes(List<Quiz> q1, List<Quiz> q2)
        {
            if (q1.Count != q2.Count) return false;
            for (int i = 0; i < q1.Count; i++)
            {
                if (!_quizComparer.Equals(q1[i], q2[i])) return false;
            }
            return true;
        }

        private bool CompareExam(Exam? e1, Exam? e2)
        {
            if (e1 == null && e2 == null) return true;
            if (e1 == null || e2 == null) return false;
            return _examComparer.Equals(e1, e2);
        }
    }
}
