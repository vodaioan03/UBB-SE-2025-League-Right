using System.Collections.Generic;

namespace Duo.Models.Quizzes;

public abstract class BaseQuiz<T> : IQuiz<T>
{
    public int Id { get; set; }
    protected List<T> exerciseList;
    private int numberOfAnswersGiven = 0;
    private int numberOfCorrectAnswers = 0;

    protected int maxExercises;
    protected double passingThreshold;


    public BaseQuiz(int id, int maxExercises, double passingThreshold)
    {
        Id = id;
        this.maxExercises = maxExercises;
        this.passingThreshold = passingThreshold;
        exerciseList = new List<T>();
    }

    public bool AddExercise(T exercise)
    {
        if (exerciseList.Count < maxExercises)
        {
            exerciseList.Add(exercise);

            return true;
        }

        return false;
    }

    public bool IsValid()
    {
        return exerciseList.Count == maxExercises;
    }

    public double GetPassingThreshold()
    {
        return passingThreshold;
    }

    public int GetNumberOfAnswersGiven()
    {
        return numberOfAnswersGiven;
    }

    public int GetNumberOfCorrectAnswers()
    {
        return numberOfCorrectAnswers;
    }

    public void IncrementCorrectAnswers()
    {
        numberOfCorrectAnswers++;
    }

    public void IncrementNumberOfAnswersGiven()
    {
        numberOfAnswersGiven++;
    }

    public int GetExerciseCount()
    {
        return exerciseList.Count;
    }
}
