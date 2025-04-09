using Duo.Models.Exercises;
using System.Collections.Generic;
using System.Linq;

namespace DuoTesting.Helpers
{
    public class ExerciseComparer : IEqualityComparer<Exercise>
    {
        public bool Equals(Exercise? x, Exercise? y)
        {
            if (x is null || y is null) return false;
            if (x.GetType() != y.GetType()) return false;
            if (x.Id != y.Id || x.Question != y.Question || x.Difficulty != y.Difficulty)
                return false;

            return x switch
            {
                MultipleChoiceExercise mcx when y is MultipleChoiceExercise mcy =>
                    CompareChoices(mcx.Choices, mcy.Choices),

                FlashcardExercise fx when y is FlashcardExercise fy =>
                    fx.Answer == fy.Answer && fx.TimeInSeconds == fy.TimeInSeconds,

                FillInTheBlankExercise fibx when y is FillInTheBlankExercise fiby =>
                    CompareList(fibx.PossibleCorrectAnswers, fiby.PossibleCorrectAnswers),

                AssociationExercise ax when y is AssociationExercise ay =>
                    CompareList(ax.FirstAnswersList, ay.FirstAnswersList) &&
                    CompareList(ax.SecondAnswersList, ay.SecondAnswersList),

                _ => false
            };
        }

        public int GetHashCode(Exercise obj)
        {
            return HashCode.Combine(obj.Id, obj.Question, obj.Difficulty);
        }

        private bool CompareList<T>(List<T> l1, List<T> l2)
        {
            if (l1.Count != l2.Count) return false;
            return l1.SequenceEqual(l2);
        }

        private bool CompareChoices(List<MultipleChoiceAnswerModel> c1, List<MultipleChoiceAnswerModel> c2)
        {
            if (c1.Count != c2.Count) return false;
            for (int i = 0; i < c1.Count; i++)
            {
                if (c1[i].Answer != c2[i].Answer || c1[i].IsCorrect != c2[i].IsCorrect)
                    return false;
            }
            return true;
        }
    }
}
