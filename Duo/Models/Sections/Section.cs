using System.Collections.Generic;

namespace Duo.Models.Sections;

public class Section<TQuiz, TExam>: ISection<TQuiz, TExam>
{
    public int Id { get; }
    private Queue<TQuiz> quizzes;
    private TExam finalExam;

    private const int MAX_QUIZZES = 5;

    public Section(int id)
    {
        Id = id;
        quizzes = new Queue<TQuiz>();
    }

    public bool AddQuiz(TQuiz quiz)
    {
        if (quizzes.Count < MAX_QUIZZES)
        {
            quizzes.Enqueue(quiz);
            return true;
        }
        return false;
    }

    public bool AddExam(TExam newExam)
    {
        if (finalExam == null)
        {
            finalExam = newExam;
            return true;
        }
        return false;
    }

    public bool IsValid()
    {
        return quizzes.Count >= 2 && finalExam != null;
    }

    public IEnumerable<TQuiz> GetAllQuizzes()
    {
        return quizzes;
    }


    public TExam GetFinalExam()
    {
        return finalExam;
    }
}
