using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using Duo;
using Duo.Commands;
using Duo.Models.Quizzes;
using Duo.Models.Sections;
using Duo.Services;
using Duo.ViewModels.Base;
using Duo.ViewModels.Roadmap;
using Duo.Views.Components;

public class RoadmapMainPageViewModel : ViewModelBase
{
    private RoadmapQuizPreviewViewModel _quizPreviewViewModel;
    private RoadmapSectionViewModel _sectionViewModel;
    private BaseQuiz _selectedQuiz;
    private Section _currentSection;
    private QuizService _quizService;
    private SectionService _sectionService;

    private ICommand OpenQuizPreviewCommand;
    private ICommand StartQuizCommand;

    public RoadmapQuizPreviewViewModel QuizPreviewViewModel => _quizPreviewViewModel;
    public RoadmapSectionViewModel SectionViewModel => _sectionViewModel;

    public RoadmapMainPageViewModel(QuizService quizService, SectionService sectionService)
    {
        _quizService = quizService;
        _sectionService = sectionService;

        StartQuizCommand = new RelayCommand(StartQuiz);
        OpenQuizPreviewCommand = new RelayCommandWithParameter<int>(OpenQuizPreview);

        // Pass the dependencies to the RoadmapQuizPreviewViewModel
        _quizPreviewViewModel = new RoadmapQuizPreviewViewModel(_quizService, _sectionService, StartQuizCommand);
        _sectionViewModel = new RoadmapSectionViewModel(-1, OpenQuizPreviewCommand);


    }

    private async void OpenQuizPreview(int quizId)
    {
        Debug.WriteLine($"Opening quiz with ID: {quizId}");

        _selectedQuiz = await _quizService.GetExerciseById(quizId);
        _currentSection = await _sectionService.GetSectionById((int)_selectedQuiz.SectionId);
        await _quizPreviewViewModel.OpenForQuiz(_selectedQuiz, _currentSection);
    }

    private void StartQuiz()
    {
        Console.WriteLine($"Starting quiz with ID: {_selectedQuiz?.Id}");
        // Navigation logic goes here
    }
}