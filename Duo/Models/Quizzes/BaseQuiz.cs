using System.Collections.Generic;
using Duo.Models.Exercises;

namespace Duo.Models.Quizzes;

public abstract class BaseQuiz
{
    public int Id { get; set; }
    public int? SectionId { get; set; }
    public List<Exercise> ExerciseList { get; set; } = new();
    private int numberOfAnswersGiven = 0;
    private int numberOfCorrectAnswers = 0;

    protected int maxExercises;
    protected double passingThreshold;

    protected BaseQuiz(int id, int? sectionId, int maxExercises, double passingThreshold)
    {
        Id = id;
        SectionId = sectionId;
        this.maxExercises = maxExercises;
        this.passingThreshold = passingThreshold;
    }

    public bool AddExercise(Exercise exercise)
    {
        if (ExerciseList.Count < maxExercises)
        {
            ExerciseList.Add(exercise);
            return true;
        }
        return false;
    }

    public bool RemoveExercise(Exercise exercise)
    {
        return ExerciseList.Remove(exercise);
    }

    public bool IsValid()
    {
        return ExerciseList.Count == maxExercises;
    }

    public double GetPassingThreshold()
    {
        return passingThreshold;
    }

    public int GetNumberOfAnswersGiven()
    {
        return ExerciseList.Count;
    }

    public int GetNumberOfCorrectAnswers()
    {
        return numberOfCorrectAnswers;
    }

    public void IncrementCorrectAnswers()
    {
        numberOfCorrectAnswers++;
    }

    public override string ToString()
    {
        var progress = numberOfAnswersGiven > 0 
            ? $"Progress: {numberOfCorrectAnswers}/{numberOfAnswersGiven} ({((double)numberOfCorrectAnswers/numberOfAnswersGiven)*100:F1}%)"
            : "Not started";
        return $"Quiz {Id} (Section: {SectionId ?? 0}) - {ExerciseList.Count}/{maxExercises} exercises - {progress}";
    }
}
