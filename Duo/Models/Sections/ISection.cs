using System.Collections.Generic;

namespace Duo.Models.Sections;

interface ISection<TQuiz, TExam>
{
    public bool AddQuiz(TQuiz quiz);
    public bool AddExam(TExam newExam);
    public bool IsValid();
    public IEnumerable<TQuiz> GetAllQuizzes();
    public TExam GetFinalExam();
}
