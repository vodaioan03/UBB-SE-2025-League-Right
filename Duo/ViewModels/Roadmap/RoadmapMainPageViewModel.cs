using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Duo;
using Duo.Commands;
using Duo.Models;
using Duo.Models.Quizzes;
using Duo.Models.Sections;
using Duo.Services;
using Duo.ViewModels.Base;
using Duo.ViewModels.Roadmap;
using Duo.Views.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;

namespace Duo.ViewModels.Roadmap
{
    public class RoadmapMainPageViewModel : ViewModelBase
    {
        private RoadmapService roadmapService;
        private Duo.Models.Roadmap.Roadmap roadmap;

        private UserService userService;
        private User user;

        private BaseQuiz selectedQuiz;

        private ObservableCollection<RoadmapSectionViewModel> sectionViewModels;
        public ObservableCollection<RoadmapSectionViewModel> SectionViewModels
        {
            get => sectionViewModels;
            set => SetProperty(ref sectionViewModels, value);
        }

        public ICommand OpenQuizPreviewCommand;
        public ICommand StartQuizCommand;

        public RoadmapMainPageViewModel()
        {
            roadmapService = (RoadmapService)App.ServiceProvider.GetService(typeof(RoadmapService));
            userService = (UserService)App.ServiceProvider.GetService(typeof(UserService));

            StartQuizCommand = new RelayCommand(StartQuiz);
            OpenQuizPreviewCommand = new RelayCommandWithParameter<Tuple<int, bool>>(OpenQuizPreview);
        }

        public async Task SetupViewModel()
        {
            roadmap = await roadmapService.GetRoadmapById(1);
            user = await userService.GetByIdAsync(1);

            SectionService sectionService = (SectionService)App.ServiceProvider.GetService(typeof(SectionService));
            List<Section> sections = (List<Section>)await sectionService.GetByRoadmapId(1);

            sectionViewModels = new ObservableCollection<RoadmapSectionViewModel>();
            // sections.ForEach(section =>
            // {
            //    var sectionViewModel = (RoadmapSectionViewModel)App.serviceProvider.GetService(typeof(RoadmapSectionViewModel));
            //    sectionViewModel.SetupForSection(section.Id);
            //    _sectionViewModels.Add(sectionViewModel);
            // });
            for (int i = 1; i <= sections.Count; i += 1)
            {
                var sectionViewModel = (RoadmapSectionViewModel)App.ServiceProvider.GetService(typeof(RoadmapSectionViewModel));
                if (i <= user.NumberOfCompletedSections)
                {
                    await sectionViewModel.SetupForSection(sections[i - 1].Id, true, 0);
                }
                else if (i == user.NumberOfCompletedSections + 1)
                {
                    await sectionViewModel.SetupForSection(sections[i - 1].Id, false, user.NumberOfCompletedQuizzesInSection);
                }
                else
                {
                    await sectionViewModel.SetupForSection(sections[i - 1].Id, false, -1);
                }
                sectionViewModels.Add(sectionViewModel);
            }

            OnPropertyChanged(nameof(SectionViewModels));
        }

        private async void OpenQuizPreview(Tuple<int, bool> args)
        {
            Debug.WriteLine($"Opening quiz with ID: {args.Item1}");

            var quizPreviewViewModel = (RoadmapQuizPreviewViewModel)App.ServiceProvider.GetService(typeof(RoadmapQuizPreviewViewModel));
            await quizPreviewViewModel.OpenForQuiz(args.Item1, args.Item2);
        }

        // MAYBE MAKE THIS OBSOLETE
        private void StartQuiz()
        {
            Console.WriteLine($"Starting quiz with ID: {selectedQuiz?.Id}");
            // Navigation logic goes here
        }
    }
}