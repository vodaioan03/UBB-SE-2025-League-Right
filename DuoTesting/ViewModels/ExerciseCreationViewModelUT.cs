using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Duo.ViewModels;
using Duo.Services;
using System;
using System.Threading.Tasks;
using Duo.Models.Exercises;
using Duo.Views.Components.CreateExerciseComponents;
using Duo;
using Duo.Commands;
using Duo.ViewModels.CreateExerciseViewModels;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using DuoTesting.Helper;
using System.Diagnostics;
using System.IO;
using Duo.Helpers;
using Microsoft.UI.Xaml;
using Windows.UI.Core;

namespace DuoTesting.ViewModels
{

    [TestClass]
    public class ExerciseCreationViewModelTests
    {


        [TestMethod]
        public void Constructor_ShouldInitialize_Properties()
        {
            IExerciseService exerciseService = new Mock<IExerciseService>().Object;
            IExerciseViewFactory exerciseViewFactory = new Mock<IExerciseViewFactory>().Object;

            var vm = new ExerciseCreationViewModel(exerciseService, exerciseViewFactory);

            // Verify that ExerciseTypes and Difficulties are populated
            Assert.IsNotNull(vm.ExerciseTypes);
            Assert.IsNotNull(vm.Difficulties);
        }
    }
}