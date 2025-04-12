using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Duo.Commands;
using Duo.Models.Exercises;
using Duo.Models.Quizzes;
using Duo.Models.Sections;
using Duo.Services;
using Duo.ViewModels.Base;

namespace Duo.ViewModels
{
    internal class ManageSectionsViewModel : AdminBaseViewModel
    {
        private readonly SectionService sectionService;
        private readonly QuizService quizService;
        public ObservableCollection<Section> Sections { get; set; } = new ObservableCollection<Section>();
        public ObservableCollection<Quiz> SectionQuizes { get; private set; } = new ObservableCollection<Quiz>();

        private Section selectedSection;

        public ICommand DeleteSectionCommand;

        public ManageSectionsViewModel()
        {
            try
            {
                sectionService = (SectionService)App.ServiceProvider.GetService(typeof(SectionService));
                quizService = (QuizService)App.ServiceProvider.GetService(typeof(QuizService));
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
            get => selectedSection;
            set
            {
                selectedSection = value;
                UpdateSectionQuizes(SelectedSection);
                OnPropertyChanged();
            }
        }

        public async void LoadSectionsAsync()
        {
            try
            {
                var sections = await sectionService.GetAllSections();
                Sections.Clear();
                foreach (var section in sections)
                {
                    Sections.Add(section);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                RaiseErrorMessage(ex.Message, string.Empty);
            }
        }

        public async void UpdateSectionQuizes(Section selectedSection)
        {
            Debug.WriteLine("Updating quiz exercises...");
            SectionQuizes.Clear();
            if (SelectedSection == null)
            {
                return;
            }

            List<Quiz> quizzesOfSelectedQuiz = await quizService.GetAllQuizzesFromSection(selectedSection.Id);
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
                await sectionService.DeleteSection(sectionToBeDeleted.Id);
                Sections.Remove(sectionToBeDeleted);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                RaiseErrorMessage(ex.Message, string.Empty);
            }
        }
    }
}
