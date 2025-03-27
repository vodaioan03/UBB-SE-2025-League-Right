namespace Duo.Models.Exercises;

public class MultipleChoiceExercise : Exercise
{
    public List<(string Answer, bool IsCorrect)> Answers { get; }
    public string CorrectAnswer { get; }

    public MultipleChoiceExercise(int id, string question, Difficulty difficulty, List<(string, bool)> answers, string correctAnswer)
        : base(id, question, difficulty)
    {
        Answers = answers;
        CorrectAnswer = correctAnswer;
    }

    public bool ValidateAnswer(List<string> userAnswers)
    {
        if (userAnswers == null || userAnswers.Count == 0)
            return false;

        var correctAnswers = Answers.Where(a => a.IsCorrect).Select(a => a.Answer).OrderBy(a => a).ToList();
        var userSelection = userAnswers.OrderBy(a => a).ToList();

        return correctAnswers.SequenceEqual(userSelection);
    }
}

