﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using Duo.Models;
using Duo.Models.Exercises;
using Duo.Models.Quizzes;
using Duo.Models.Sections;
using Duo.Services;
using Duo.ViewModels.Base;

namespace Duo.ViewModels
{
    public class QuizExamViewModel : ViewModelBase
    {
        private readonly IExerciseService exerciseService;
        private readonly IQuizService quizService;
        private int quizId;
        private int examId;
        private Quiz currentQuiz;
        private Exam currentExam;
        private List<Exercise> exercises;
        private Exercise currentExercise;
        private int currentExerciseIndex;
        private bool? validatedCurrent = null;

        public int QuizId
        {
            get => quizId;
        }

        public int ExamId
        {
            get => examId;
        }

        private async Task LoadQuizData()
        {
            await LoadQuiz();
            await LoadExercises();
        }

        private async Task LoadExamData()
        {
            Debug.WriteLine("Salut, intra in chestia de load data de la exam, adica e bine");
            await LoadExam();
            await LoadExercises();
        }

        public async Task SetQuizIdAsync(int value)
        {
            quizId = value;
            examId = -1;
            OnPropertyChanged(nameof(QuizId));
            await LoadQuizData();
        }

        public async Task SetExamIdAsync(int value)
        {
            examId = value;
            quizId = -1;
            OnPropertyChanged(nameof(ExamId));
            await LoadExamData();
        }

        public List<Exercise> Exercises
        {
            get => exercises;
            private set
            {
                exercises = value;
                OnPropertyChanged(nameof(Exercises));
            }
        }

        public Exercise CurrentExercise
        {
            get => currentExercise;
            set
            {
                currentExercise = value;
                OnPropertyChanged(nameof(CurrentExercise));
            }
        }

        public Quiz CurrentQuiz
        {
            get => currentQuiz;
            set
            {
                currentQuiz = value;
                OnPropertyChanged(nameof(CurrentQuiz));
            }
        }

        public Exam CurrentExam
        {
            get => currentExam;
            set
            {
                currentExam = value;
                OnPropertyChanged(nameof(CurrentExam));
            }
        }

        public int CurrentExerciseIndex
        {
            get => currentExerciseIndex;
            set
            {
                currentExerciseIndex = value;
                OnPropertyChanged(nameof(CurrentExerciseIndex));
            }
        }

        public bool? ValidatedCurrent
        {
            get => validatedCurrent;
            set
            {
                validatedCurrent = value;
                OnPropertyChanged(nameof(ValidatedCurrent));
            }
        }

        public string CorrectAnswersText
        {
            get
            {
                if (QuizId != -1)
                {
                    return $"{CurrentQuiz.GetNumberOfCorrectAnswers()}/{CurrentQuiz.GetNumberOfAnswersGiven()}";
                }
                return $"{CurrentExam.GetNumberOfCorrectAnswers()}/{CurrentExam.GetNumberOfAnswersGiven()}";
            }
        }
        public string PassingPercentText
        {
            get
            {
                return $"{(int)Math.Round(GetPercentageCorrect() * 100)}%";
            }
        }
        public string IsPassedText
        {
            get
            {
                if (IsPassed())
                {
                    return "Great job! You passed this one.";
                }
                return "You need to redo this one.";
            }
        }

        public QuizExamViewModel(IExerciseService exerciseService)
        {
            this.exerciseService = exerciseService;
        }

        public QuizExamViewModel()
        {
            try
            {
                exerciseService = (IExerciseService)App.ServiceProvider.GetService(typeof(IExerciseService));
                quizService = (IQuizService)App.ServiceProvider.GetService(typeof(IQuizService));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public async Task LoadQuiz()
        {
            try
            {
            Debug.WriteLine("We load qwuiz");
                CurrentQuiz = await quizService.GetQuizById(quizId);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading Quiz: {ex.Message}");
            }
        }

        public async Task LoadExam()
        {
            Debug.WriteLine("We load exam");
            try
            {
                CurrentExam = await quizService.GetExamById(examId);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading Quiz: {ex.Message}");
            }
        }

        public async Task LoadExercises()
        {
            try
            {
                if (QuizId != -1)
                {
                    Exercises = await exerciseService.GetAllExercisesFromQuiz(QuizId);
                    CurrentQuiz.ExerciseList = Exercises;
                }
                else
                {
                    Exercises = await exerciseService.GetAllExercisesFromExam(ExamId);
                    CurrentExam.ExerciseList = Exercises;
                }

                CurrentExerciseIndex = 0;
                CurrentExercise = Exercises[CurrentExerciseIndex];
                ValidatedCurrent = null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading exercises: {ex.Message}");
                Exercises = new List<Exercise>();
            }
        }

        private void UpdateQuiz(bool? isExerciseValid)
        {
            if (isExerciseValid == true)
            {
                CurrentQuiz.IncrementCorrectAnswers();
            }
        }

        private void UpdateExam(bool? isExerciseValid)
        {
            if (isExerciseValid == true)
            {
                CurrentExam.IncrementCorrectAnswers();
            }
        }

        public bool? ValidateCurrentExercise(object responses)
        {
            if (ValidatedCurrent is not null)
            {
                return ValidatedCurrent;
            }

            bool isValid = false;

            if (CurrentExercise is AssociationExercise associationExercise)
            {
                isValid = associationExercise.ValidateAnswer((List<(string, string)>)responses);
            }
            else if (CurrentExercise is FillInTheBlankExercise fillInTheBlanksExercise)
            {
                isValid = fillInTheBlanksExercise.ValidateAnswer((List<string>)responses);
            }
            else if (CurrentExercise is MultipleChoiceExercise multipleChoiceExercise)
            {
                isValid = multipleChoiceExercise.ValidateAnswer((List<string>)responses);
            }
            else if (CurrentExercise is FlashcardExercise flashcardExercise)
            {
                isValid = flashcardExercise.ValidateAnswer((string)responses);
            }
            ValidatedCurrent = isValid;
            if (QuizId != -1)
            {
                UpdateQuiz(ValidatedCurrent);
            }
            else
            {
                UpdateExam(ValidatedCurrent);
            }

            return isValid;
        }

        public bool LoadNext()
        {
            if (ValidatedCurrent == null)
            {
                return false;
            }

            CurrentExerciseIndex += 1;
            if (Exercises.Count <= CurrentExerciseIndex)
            {
                CurrentExercise = null;
            }
            else
            {
                CurrentExercise = Exercises[CurrentExerciseIndex];
            }

            ValidatedCurrent = null;

            return true;
        }

        public float GetPercentageDone()
        {
            if (QuizId != -1)
            {
                return (float)CurrentQuiz.GetNumberOfAnswersGiven() / Exercises.Count;
            }
            return (float)CurrentExam.GetNumberOfAnswersGiven() / Exercises.Count;
        }

        private float GetPercentageCorrect()
        {
            if (QuizId != -1)
            {
                return (float)CurrentQuiz.GetNumberOfCorrectAnswers() / Exercises.Count;
            }
            return (float)CurrentExam.GetNumberOfCorrectAnswers() / Exercises.Count;
        }

        private bool IsPassed()
        {
            int percentCorrect = (int)Math.Round(GetPercentageCorrect() * 100);
            if (QuizId != -1)
            {
                return percentCorrect >= CurrentQuiz.GetPassingThreshold();
            }
            return percentCorrect >= CurrentExam.GetPassingThreshold();
        }

        public string GetCurrentExerciseCorrectAnswer()
        {
            string correctAnswers = string.Empty;

            if (CurrentExercise is AssociationExercise associationExercise)
            {
                for (int i = 0; i < associationExercise.FirstAnswersList.Count; i++)
                {
                    correctAnswers += associationExercise.FirstAnswersList[i] + " - " + associationExercise.SecondAnswersList[i] + "\n";
                }
            }
            else if (CurrentExercise is FillInTheBlankExercise fillInTheBlanksExercise)
            {
                foreach (var answer in fillInTheBlanksExercise.PossibleCorrectAnswers)
                {
                    correctAnswers += answer + "\n";
                }
            }
            else if (CurrentExercise is MultipleChoiceExercise multipleChoiceExercise)
            {
                correctAnswers += multipleChoiceExercise.Choices
                    .Where(choice => choice.IsCorrect)
                    .Select(choice => choice.Answer)
                    .First();
            }
            else if (CurrentExercise is FlashcardExercise flashcardExercise)
            {
                correctAnswers += string.Join(", ", flashcardExercise.GetCorrectAnswer());
            }

            return correctAnswers;
        }

        public async Task MarkUserProgression()
        {
            if (GetPercentageDone() != 1)
            {
                return;
            }
            if (IsPassed())
            {
                UserService userService = (UserService)App.ServiceProvider.GetService(typeof(UserService));
                SectionService sectionService = (SectionService)App.ServiceProvider.GetService(typeof(SectionService));

                User user = await userService.GetByIdAsync(1);

                List<Section> sections = await sectionService.GetByRoadmapId(1);
                Section currentUserSection = sections[user.NumberOfCompletedSections];
                List<Quiz> currentSectionQuizzes = await quizService.GetAllQuizzesFromSection(currentUserSection.Id);

                if (quizId == -1)
                {
                    if (currentExam == null)
                    {
                        return;
                    }
                    if (user.NumberOfCompletedQuizzesInSection != currentSectionQuizzes.Count())
                    {
                        return;
                    }
                    if (currentUserSection.GetFinalExam().Id != examId)
                    {
                        return;
                    }
                }
                else
                {
                    if (currentQuiz == null)
                    {
                        return;
                    }
                    if (user.NumberOfCompletedQuizzesInSection == currentSectionQuizzes.Count())
                    {
                        return;
                    }
                    if (currentSectionQuizzes[user.NumberOfCompletedQuizzesInSection].Id != quizId)
                    {
                        return;
                    }
                }
                userService.IncrementUserProgressAsync(1);
            }
        }
    }
}
