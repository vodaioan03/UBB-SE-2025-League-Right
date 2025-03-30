using System.Collections.Generic;
using Duo.Models.Quizzes;
using Duo.Models.Exercises;

namespace Duo.Models.Sections;

public class Section
{
    public required int Id { get; set; }
    public required int SubjectId { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required int RoadmapId { get; set; }
    public required int OrderNumber { get; set; }
    public List<Quiz> Quizzes { get; set; }
    public Exam? Exam { get; set; }

    private const int MAX_QUIZZES = 5;
    private const int MIN_QUIZZES = 2;

    public Section()
    {
        Quizzes = new List<Quiz>();
    }

    public bool AddQuiz(Quiz quiz)
    {
        if (Quizzes.Count < MAX_QUIZZES)
        {
            Quizzes.Add(quiz);
            return true;
        }
        return false;
    }

    public bool AddExam(Exam newExam)
    {
        if (Exam == null)
        {
            Exam = newExam;
            return true;
        }
        return false;
    }

    public bool IsValid()
    {
        return Quizzes.Count >= MIN_QUIZZES && Exam != null;
    }

    public IEnumerable<Quiz> GetAllQuizzes()
    {
        return Quizzes;
    }

    public Exam? GetFinalExam()
    {
        return Exam;
    }
}
