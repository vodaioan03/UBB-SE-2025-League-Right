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
    internal class ManageSectionsViewModel:ViewModelBase
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
            DeleteSectionCommand = new RelayCommandWithParameter<Quiz>(DeleteSection);
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
            }
        }

        public void UpdateSectionQuizes(Section selectedSection)
        {

        }

        public void DeleteSection(Quiz quizToBeDeleted)
        {

        }
    }
}
