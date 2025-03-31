using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Duo.ViewModels.Roadmap;
using System;
using Microsoft.UI.Xaml.Media;

namespace Duo.Views.Components
{
    public sealed partial class RoadmapQuizPreview : UserControl
    {
        public RoadmapQuizPreview()
        {
            this.InitializeComponent();

            //BuildUI();
        }

        //private void BuildUI()
        //{
        //    var stackPanel = new StackPanel();

        //    // Bind Visibility to IsPreviewVisible
        //    var visibilityBinding = new Binding
        //    {
        //        Source = ViewModel,
        //        Path = new PropertyPath("IsPreviewVisible"),
        //        Converter = (IValueConverter)Resources["BoolToVisibilityConverter"],
        //        Mode = BindingMode.OneWay
        //    };
        //    stackPanel.SetBinding(StackPanel.VisibilityProperty, visibilityBinding);

        //    // Section Title
        //    var sectionTitleTextBlock = new TextBlock
        //    {
        //        FontSize = 32,
        //        HorizontalAlignment = HorizontalAlignment.Center
        //    };
        //    sectionTitleTextBlock.SetBinding(TextBlock.TextProperty, new Binding
        //    {
        //        Source = ViewModel,
        //        Path = new PropertyPath("SectionTitle"),
        //        Mode = BindingMode.OneWay
        //    });

        //    // Quiz Order Number
        //    var quizOrderTextBlock = new TextBlock
        //    {
        //        FontSize = 32,
        //        HorizontalAlignment = HorizontalAlignment.Center
        //    };
        //    quizOrderTextBlock.SetBinding(TextBlock.TextProperty, new Binding
        //    {
        //        Source = ViewModel,
        //        Path = new PropertyPath("QuizOrderNumber"),
        //        Mode = BindingMode.OneWay
        //    });

        //    // Start Quiz Button
        //    var startQuizButton = new Button
        //    {
        //        Content = "Start quiz",
        //        HorizontalAlignment = HorizontalAlignment.Center,
        //        Margin = new Thickness(0, 20, 0, 0)
        //    };
        //    startQuizButton.SetBinding(Button.CommandProperty, new Binding
        //    {
        //        Source = ViewModel,
        //        Path = new PropertyPath("StartQuizCommand")
        //    });
        //    startQuizButton.SetBinding(Button.CommandParameterProperty, new Binding
        //    {
        //        Source = ViewModel,
        //        Path = new PropertyPath("Quiz")
        //    });

        //    // Back Button
        //    var backButton = new Button
        //    {
        //        Content = "Back",
        //        HorizontalAlignment = HorizontalAlignment.Center,
        //        Margin = new Thickness(0, 20, 0, 0)
        //    };
        //    backButton.SetBinding(Button.CommandProperty, new Binding
        //    {
        //        Source = ViewModel,
        //        Path = new PropertyPath("BackButtonCommand")
        //    });

        //    // Add controls to stack panel
        //    stackPanel.Children.Add(sectionTitleTextBlock);
        //    stackPanel.Children.Add(quizOrderTextBlock);
        //    stackPanel.Children.Add(startQuizButton);
        //    stackPanel.Children.Add(backButton);

        //    // Add stack panel to the main container
        //    MainContainer.Children.Add(stackPanel);
        //}
    }
}
