using Duo.Models.Exercises;

namespace Duo.Models.Quizzes;

public class Quiz : BaseQuiz<Exercise>
{
    private const int MAX_EXERCISES = 10;
    private const double PASSING_THRESHOLD = 75
        ;
    public Quiz(int id) : base(id, MAX_EXERCISES, PASSING_THRESHOLD) { }
}
