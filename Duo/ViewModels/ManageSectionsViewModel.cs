using Duo.Commands;
using Duo.Models.Exercises;
using Duo.Models.Quizzes;
using Duo.Models.Sections;
using Duo.Services;
using Duo.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Duo.ViewModels
{
    internal class ManageSectionsViewModel: AdminBaseViewModel
    {
        private readonly SectionService _sectionService;
        private readonly QuizService _quizService;
        public ObservableCollection<Section> Sections { get; set; } = new ObservableCollection<Section>();
        public ObservableCollection<Quiz> SectionQuizes { get; private set; } = new ObservableCollection<Quiz>();

        private Section _selectedSection;

        public ICommand DeleteSectionCommand;

        public ManageSectionsViewModel()
        {
            try
            {
                _sectionService = (SectionService)App.serviceProvider.GetService(typeof(SectionService));
                _quizService = (QuizService)App.serviceProvider.GetService(typeof(QuizService));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            LoadSectionsAsync();
            DeleteSectionCommand = new RelayCommandWithParameter<Section>(DeleteSection);
        }
        public Section SelectedSection
        {
            get => _selectedSection;
            set
            {
                _selectedSection = value;
                UpdateSectionQuizes(SelectedSection);
                OnPropertyChanged();
            }
        }

        public async void LoadSectionsAsync()
        {
            try
            {
                var sections = await _sectionService.GetAllSections();
                Sections.Clear();
                foreach (var section in sections)
                {
                    Sections.Add(section);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                RaiseErrorMessage(ex.Message, "");
            }
        }

        public async void UpdateSectionQuizes(Section selectedSection)
        {
            Debug.WriteLine("Updating quiz exercises...");
            SectionQuizes.Clear();
            if (SelectedSection == null)
                return;

            List<Quiz> quizzesOfSelectedQuiz = await _quizService.GetAllQuizzesFromSection(selectedSection.Id);
            foreach (var quiz in quizzesOfSelectedQuiz)
            {
                Debug.WriteLine(quiz);
                SectionQuizes.Add(quiz);
            }
        }

        public async void DeleteSection(Section sectionToBeDeleted)
        {
            Debug.WriteLine("Deleting section...");

            if (sectionToBeDeleted == SelectedSection)
            {
                SelectedSection = null;
            }

            try
            {
                await _sectionService.DeleteSection(sectionToBeDeleted.Id);
                Sections.Remove(sectionToBeDeleted);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                RaiseErrorMessage(ex.Message, "");
            }
        }
    }
}
