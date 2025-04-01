using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Duo.Models.Exercises;
using Duo.Models.Quizzes;
using Duo.Models.Sections;

namespace Duo.Services
{
    static public class ValidationHelper
    {
        static public void ValidateSection(Section section)
        {
            if (string.IsNullOrWhiteSpace(section.Title))
                throw new ValidationException("Section title must not be empty");

            if (string.IsNullOrWhiteSpace(section.Description))
                throw new ValidationException("Section description must not be empty");

            if (section.Quizzes.Count < 2 || section.Quizzes.Count > 5)
                throw new ValidationException("A section must have between 2 and 5 quizzes");

            if (section.Exam == null)
                throw new ValidationException("A section must include a final exam");

            foreach (var quiz in section.Quizzes)
            {
                ValidateQuiz(quiz);
            }

            ValidateExam(section.Exam);
        }


        static public void ValidateExam(Exam exam)
        {
            //Each exam consists of 25 exercises
            if (exam.ExerciseList == null || exam.ExerciseList.Count != 25)
                throw new ValidationException("An exam must contain exactly 25 exercises");

            if (!exam.ExerciseList.Any())
                throw new ValidationException("An exam must have exercises");

            foreach (var exercise in exam.ExerciseList)
            {
                ValidationHelper.ValidateGenericExercise(exercise);
            }
        }

        static public void ValidateQuiz(Quiz quiz)
        {
            if (quiz.ExerciseList == null || quiz.ExerciseList.Count != 10)
                throw new ValidationException("A quiz must contain exactly 10 exercises");

            if (!quiz.ExerciseList.Any())
                throw new ValidationException("A quiz must have exercises");

            if (quiz.OrderNumber.HasValue && quiz.OrderNumber.Value < 1)
                throw new ValidationException("Order number must be a positive value if set");

            foreach (var exercise in quiz.ExerciseList)
            {
                ValidationHelper.ValidateGenericExercise(exercise);
            }
        }

        static public void ValidateGenericExercise(Exercise exercise)
        {
            if (exercise == null)
                throw new ValidationException("Exercise cannot be null");

            switch (exercise)
            {
                case MultipleChoiceExercise mcEx:
                    ValidateMultipleChoiceExercise(mcEx);
                    break;
                case AssociationExercise assocEx:
                    ValidateAssociationExercise(assocEx);
                    break;
                case FillInTheBlankExercise fibEx:
                    ValidateFillInTheBlankExercise(fibEx);
                    break;
                case FlashcardExercise flashEx:
                    ValidateFlashcardExercise(flashEx);
                    break;
                default:
                    throw new ValidationException($"Unsupported exercise type: {exercise.GetType().Name}");
            }
        }

        static public void ValidateMultipleChoiceExercise(MultipleChoiceExercise ex)
        {
            if (ex.Question.Length > 200)
                throw new ValidationException("Exercise question is too long");
            if (ex.Choices == null)
                throw new ValidationException("Exercise choices are not set properly");
            if (ex.Choices.Count < 2 || ex.Choices.Count > 5)
                throw new ValidationException("Invalid exercise choice count (2<=n<=5)");
            if (!ex.Choices.Any(c => c.IsCorrect))
                throw new ValidationException("None of the choices marked as correct");
            if (ex.Choices.Any(c => c.Answer.Length > 80))
                throw new ValidationException("Exercise choice text is too long");
        }

        static public void ValidateAssociationExercise(AssociationExercise ex)
        {
            if (ex.Question.Length > 200)
                throw new ValidationException("Exercise question is too long");
            if (ex.FirstAnswersList == null || ex.SecondAnswersList == null)
                throw new ValidationException("Answer lists are not set properly");
            if (ex.FirstAnswersList.Count < 3 || ex.FirstAnswersList.Count > 5)
                throw new ValidationException("Invalid number of items in answer lists (3<=n<=5)");
            if (ex.SecondAnswersList.Count < 3 || ex.SecondAnswersList.Count > 5)
                throw new ValidationException("Invalid number of items in answer lists (3<=n<=5)");
            if (ex.FirstAnswersList.Count != ex.SecondAnswersList.Count)
                throw new ValidationException("Answer lists must have the same length");
            if (ex.FirstAnswersList.Any(a => a.Length > 30) || ex.SecondAnswersList.Any(a => a.Length > 30))
                throw new ValidationException("Answer text is too long (max 30 characters per item)");
        }

        static public void ValidateFillInTheBlankExercise(FillInTheBlankExercise ex)
        {
            if (ex.Question.Length < 1 || ex.Question.Length > 200)
                throw new ValidationException("Exercise question length must be between 1 and 200 characters");
            if (ex.PossibleCorrectAnswers == null || ex.PossibleCorrectAnswers.Count == 0)
                throw new ValidationException("No answer provided");
            if (ex.PossibleCorrectAnswers.Count < 1 || ex.PossibleCorrectAnswers.Count > 3)
                throw new ValidationException("Number of blanks (answers) must be between 1 and 3");
            if (ex.PossibleCorrectAnswers.Any(a => a.Length > 30))
                throw new ValidationException("Answer text is too long (max 30 characters per slot)");

            int blankCount = Regex.Matches(ex.Question, @"\{\}").Count;

            if (blankCount != ex.PossibleCorrectAnswers.Count)
                throw new ValidationException("Number of blanks does not match number of provided answers");
        }

        static public void ValidateFlashcardExercise(FlashcardExercise ex)
        {
            if (ex.Question.Length < 1 || ex.Question.Length > 50)
                throw new ValidationException("Flash card question length must be between 1 and 50 characters");
            if (ex.Answer == null || string.IsNullOrWhiteSpace(ex.Answer))
                throw new ValidationException("Flash card answer must not be empty");
            if (ex.Answer.Length > 30)
                throw new ValidationException("Flash card answer length must not exceed 30 characters");
        }


    }
}
