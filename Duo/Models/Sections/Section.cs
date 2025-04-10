using System.Collections.Generic;
using Duo.Models.Quizzes;
using Duo.Models.Exercises;

namespace Duo.Models.Sections;

public class Section
{
    public int Id { get; set; }
    public int? SubjectId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int RoadmapId { get; set; }
    public int? OrderNumber { get; set; }
    public List<Quiz> Quizzes { get; set; }
    public Exam? Exam { get; set; }

    private const int MAX_QUIZZES = 5;
    private const int MIN_QUIZZES = 2;

    public Section()
    {
        Quizzes = new List<Quiz>();
    }

    public Section(int id, int? subjectId, string title, string description, int roadmapId, int? orderNumber)
    {
        Id = id;
        SubjectId = subjectId;
        Title = title;
        Description = description;
        RoadmapId = roadmapId;
        OrderNumber = orderNumber;
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

    public override string ToString()
    {
        var examStatus = Exam != null ? "with Exam" : "without Exam";
        return $"Section {Id}: {Title} - {Quizzes.Count} quizzes {examStatus}";
    }
}
