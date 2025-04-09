using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Duo.Commands;
using Duo.Models;
using Duo.Models.Exercises;
using Duo.Repositories;
using Duo.Services;
using Duo.ViewModels.Base;

namespace Duo.ViewModels
{
    partial class ManageExercisesViewModel : AdminBaseViewModel
    {
        private readonly ExerciseService exerciseService;
        public ObservableCollection<Exercise> Exercises { get; set; } = new ObservableCollection<Exercise>();

        public ManageExercisesViewModel()
        {
            try
            {
                exerciseService = (ExerciseService)App.ServiceProvider.GetService(typeof(ExerciseService));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                RaiseErrorMessage(ex.Message, string.Empty);
            }
            DeleteExerciseCommand = new RelayCommandWithParameter<Exercise>(exercise => _ = DeleteExercise(exercise));

            InitializeViewModel();
            LoadExercisesAsync();
        }

        public ICommand DeleteExerciseCommand { get; }

        // Method to load exercises asynchronously
        private async Task LoadExercisesAsync()
        {
            try
            {
                Exercises.Clear(); // Clear the ObservableCollection

                var exercises = await exerciseService.GetAllExercises();

                foreach (var exercise in exercises)
                {
                    Debug.WriteLine(exercise); // Add each exercise to the ObservableCollection
                    Exercises.Add(exercise);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error during LoadExercisesAsync: {ex.Message}");
                Debug.WriteLine(ex.StackTrace);
                // Optional: RaiseErrorMessage("Failed to load exercises.", ex.Message);
            }
        }

        public void InitializeViewModel()
        {
        }

        public async Task DeleteExercise(Exercise exercise)
        {
            Debug.WriteLine(exercise);
            try
            {
                await exerciseService.DeleteExercise(exercise.Id);
                await LoadExercisesAsync();
            }
            catch (Exception ex)
            {
                RaiseErrorMessage(ex.Message, string.Empty);
                Debug.WriteLine(ex);
            }
        }
    }
}
