using Duo.Commands;
using Duo.Models;
using Duo.Models.Exercises;
using Duo.Repositories;
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
    class ManageExercisesViewModel: ViewModelBase
    {
        private readonly ExerciseService _exerciseService;
        public ObservableCollection<Exercise> Exercises { get; set; } = new ObservableCollection<Exercise>();

        public ManageExercisesViewModel()
        {
            try
            {
                _exerciseService = (ExerciseService)App.serviceProvider.GetService(typeof(ExerciseService));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            DeleteExerciseCommand = new RelayCommandWithParameter<Exercise>(DeleteExercise);

            initializeViewModel();
            LoadExercisesAsync();
        }

        public ICommand DeleteExerciseCommand { get; }


        // Method to load exercises asynchronously
        private async void LoadExercisesAsync()
        {
            Exercises.Clear(); // Clear the ObservableCollection
            var exercises = await _exerciseService.GetAllExercises();
            foreach (var exercise in exercises)
            {
                Debug.WriteLine(exercise); // Add each exercise to the ObservableCollection
                Exercises.Add(exercise);
            }
        }

        public void initializeViewModel()
        {
            
        }

        public async void DeleteExercise(Exercise exercise)
        {
            Debug.WriteLine(exercise);
            try
            {
                await _exerciseService.DeleteExercise(exercise.Id);
                LoadExercisesAsync();
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }

        }

    }
}
