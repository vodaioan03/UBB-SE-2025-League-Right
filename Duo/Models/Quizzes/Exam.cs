namespace Duo.Models.Quizzes;

public class Exam : BaseQuiz
{
    private const int MAX_EXERCISES = 25;
    private const double PASSING_THRESHOLD = 90;

    public Exam(int id, int? sectionId) 
        : base(id, sectionId, MAX_EXERCISES, PASSING_THRESHOLD) { }
}
