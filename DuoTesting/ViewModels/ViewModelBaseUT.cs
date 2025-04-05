using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Duo.ViewModels.Base;
using Moq;
using Moq.Protected;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel;

namespace DuoTesting.ViewModels
{
    // / This is a test class for ViewModelBase
    [TestClass]
    public class ViewModelBaseUT
    {
        public partial class TestViewModel : ViewModelBase, INotifyPropertyChanged
        {
            private string _name;
            public string Name
            {
                get => _name;
                set => SetProperty(ref _name, value);
            }

            /// Action to be called when the property changes
            public Action OnAgeChanged;

            private int _age;
            public int Age
            {
                get => _age;
                set => SetProperty(ref _age, value, OnAgeChanged);
            }
        }

        [TestMethod]
        public void SetProperty_ValueIsDifferent_Fire()
        {
            // Arrange
            var viewModel = new TestViewModel();
            bool eventFired = false;
            viewModel.PropertyChanged += (sender, e) => { eventFired = true; };
            // Act
            viewModel.Name = "John"; // First set

            // Assert
            Assert.IsTrue(eventFired, "PropertyChanged event should be raised when the value is different.");
        }

        [TestMethod]
        public void SetPropertyWithAction_FireAction()
        {
            var viewModel = new TestViewModel();
            bool eventFired = false;

            // set the action to be called when the property changes
            viewModel.OnAgeChanged = () => { eventFired = true; };

            // Act
            viewModel.Age = 25; // First set

            Assert.IsTrue(eventFired, "Action should be called when the property is set to a different value.");
        }
    }
}
