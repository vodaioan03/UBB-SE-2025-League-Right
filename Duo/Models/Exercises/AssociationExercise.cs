namespace Duo.Models.Exercises;

public class AssociationExercise : Exercise
{
    public List<string> FirstAnswersList { get; set;  }
    public List<string> SecondAnswersList { get; set; }


    public AssociationExercise(int id, string question, Difficulty difficulty, List<string> firstAnswers, List<string> secondAnswers)
        : base(id, question, difficulty)
    {
        FirstAnswersList = firstAnswers;
        SecondAnswersList = secondAnswers;
    }

    public bool ValidateAnswer(List<(string, string)> userPairs)
    {
        if (userPairs == null || userPairs.Count != ColumnA.Count)
            return false;

        foreach (var (userA, userB) in userPairs)
        {
            int index = ColumnA.IndexOf(userA);

            if (index == -1 || ColumnB[index] != userB)
                return false;
        }

        return true;
    }
}