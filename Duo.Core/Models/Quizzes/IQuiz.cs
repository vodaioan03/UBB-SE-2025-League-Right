namespace Duo.Models.Quizzes;

public interface IQuiz<T>
{
    bool AddExercise(T exercise);
    bool IsValid();
    double GetPassingThreshold();
    public int GetNumberOfAnswersGiven();
    public int GetNumberOfCorrectAnswers();
    public void IncrementCorrectAnswers();
    public void IncrementNumberOfAnswersGiven();
}