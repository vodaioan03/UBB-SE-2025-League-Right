using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Duo.Views.Components
{
    public sealed partial class AdminPageManageCard : UserControl
    {
        public event EventHandler? AddButtonClicked; // Define event
        public event EventHandler? ManageButtonClicked; // Define event
        public AdminPageManageCard()
        {
            this.InitializeComponent();
        }
        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public string ButtonText
        {
            get => (string)GetValue(ButtonTextProperty);
            set => SetValue(ButtonTextProperty, value);
        }
        public IconElement Icon
        {
            get => (IconElement)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register(nameof(Icon), typeof(IconElement), typeof(UserControl), new PropertyMetadata(null));

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(UserControl), new PropertyMetadata("Default Title"));

        public static readonly DependencyProperty ButtonTextProperty =
            DependencyProperty.Register(nameof(ButtonText), typeof(string), typeof(UserControl), new PropertyMetadata("add"));


        public void HandleClick(object sender, TappedRoutedEventArgs e)
        {
            // Handle click event
            AddButtonClicked.Invoke(this, EventArgs.Empty);
            e.Handled = true;
        }

        public void HandleCardClick(object sender, TappedRoutedEventArgs e)
        {
            ManageButtonClicked.Invoke(this, EventArgs.Empty);
        }
    }
}
