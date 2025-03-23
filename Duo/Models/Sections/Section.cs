using System.Collections.Generic;

namespace Duo.Models.Sections;

public class Section<TQuiz, TExam>: ISection<TQuiz, TExam>
{
    public int Id { get; set; }
    public string Title { get; set; }

    private Queue<TQuiz> quizList;
    private TExam exam;

    private const int MAX_QUIZZES = 5;

    public Section(int id, string title)
    {
        Id = id;
        Title = title;
        quizList = new Queue<TQuiz>();
    }

    public bool AddQuiz(TQuiz quiz)
    {
        if (quizList.Count < MAX_QUIZZES)
        {
            quizList.Enqueue(quiz);
            return true;
        }
        return false;
    }

    public bool AddExam(TExam newExam)
    {
        if (exam == null)
        {
            exam = newExam;
            return true;
        }
        return false;
    }

    public bool IsValid()
    {
        return quizList.Count >= 2 && exam != null;
    }

    public IEnumerable<TQuiz> GetAllQuizzes()
    {
        return quizList;
    }


    public TExam GetFinalExam()
    {
        return exam;
    }
}
