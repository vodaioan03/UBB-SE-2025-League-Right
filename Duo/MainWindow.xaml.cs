using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Windowing;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.
namespace Duo
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        private AppWindow appWindow;

        public MainWindow()
        {
            this.InitializeComponent();
            this.CustomizeTitleBar();
        }

        private void CustomizeTitleBar()
        {
            ExtendsContentIntoTitleBar = true;

            try
            {
                if (AppWindowTitleBar.IsCustomizationSupported())
                {
                    var titleBar = AppWindow.TitleBar;
                    titleBar.ExtendsContentIntoTitleBar = true;

                    titleBar.ButtonBackgroundColor = Colors.Transparent;
                    titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Titlebar customization not supported: {ex.Message}");
            }
        }

        private void SetDefaultScreenSizeSettings()
        {
            var displayArea = DisplayArea.GetFromWindowId(appWindow.Id, DisplayAreaFallback.Primary);

            int desiredWidth = 1200;
            int desiredHeight = 700;

            var centerX = (displayArea.WorkArea.Width - desiredWidth) / 2;
            var centerY = (displayArea.WorkArea.Height - desiredHeight) / 2;

            appWindow.MoveAndResize(new RectInt32(
                    centerX,
                    centerY,
                    desiredWidth,
                    desiredHeight));
        }
    }
}
