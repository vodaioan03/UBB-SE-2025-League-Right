namespace Duo.Models.Quizzes;

public class Quiz : BaseQuiz
{
    private const int MAX_EXERCISES = 10;
    private const double PASSING_THRESHOLD = 75;
    public int? OrderNumber { get; set; }

    public Quiz(int id, int? sectionId, int? orderNumber) 
        : base(id, sectionId, MAX_EXERCISES, PASSING_THRESHOLD)
    {
        OrderNumber = orderNumber;
    }

    public override string ToString()
    {
        return $"{base.ToString()} - Order: {OrderNumber ?? 0}";
    }
}
