namespace Duo.Models.Exercises;
public class FillInTheBlankExercise : Exercise
{
    public List<string> PossibleCorrectAnswers { get; }

    public FillInTheBlankExercise(int id, string question, Difficulty difficulty, List<string> possibleCorrectAnswers)
        : base(id, question, difficulty)
    {
        PossibleCorrectAnswers = possibleCorrectAnswers;
    }

    public bool ValidateAnswer(List<string> userAnswers)
    {
        if (userAnswers == null || userAnswers.Count != PossibleCorrectAnswers.Count)
            return false;

        for (int i = 0; i < PossibleCorrectAnswers.Count; i++)
        {
            if (!string.Equals(userAnswers[i].Trim(), PossibleCorrectAnswers[i].Trim(), StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
        }
        return true;
    }

}