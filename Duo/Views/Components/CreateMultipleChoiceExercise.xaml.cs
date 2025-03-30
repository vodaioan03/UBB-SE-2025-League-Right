using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Duo.Views.Components
{
    public sealed partial class CreateMultipleChoiceExercise : UserControl
    {

        public CreateMultipleChoiceExercise()
        {
            this.InitializeComponent();
        }

        /*
        public async void AddNewAnswerButton_Click(object sender, RoutedEventArgs e)
        {
            if(AnswerItems.Count == MAXIMUM_ANSWERS)
            {
                await ShowErrorMessage("Cannot add more answers", $"Maximum number of answers ({MAXIMUM_ANSWERS}) reached.");
                return;
            }
            AnswerItems.Add("");
        }*/

        private async Task ShowErrorMessage(string title, string message)
        {
            var dialog = new ContentDialog
            {
                Title = title,
                Content = message,
                CloseButtonText = "OK",
                XamlRoot = this.XamlRoot
            };

            await dialog.ShowAsync();
        }
    }
}
