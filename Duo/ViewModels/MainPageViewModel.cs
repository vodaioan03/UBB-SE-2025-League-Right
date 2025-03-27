

using Duo.ViewModels.Base;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using System;
using System.Diagnostics;
using System.Security.AccessControl;

namespace Duo.ViewModels
{
    public class MainPageViewModel: ViewModelBase
    {
        public event EventHandler<Type> NavigationRequested;

        public MainPageViewModel()
        {

        }

        public void HandleNavigationSelectionChanged(NavigationViewSelectionChangedEventArgs args)
        {
            try
            {
                if (args.SelectedItem is NavigationViewItem selectedItem)
                {
                    var tag = selectedItem.Tag?.ToString();

                    if (string.IsNullOrEmpty(tag))
                        return;

                    if (selectedItem.MenuItems.Count > 0)
                        return;

                    Type? pageType = null;

                    switch (tag)
                    {
                        case "QuizParent":
                            pageType = typeof(Views.Pages.RoadmapMainPage);
                            break;
                        case "QuizAdminParent":
                            pageType = typeof(Views.Pages.AdminMainPage);
                            break;
                        default:
                            Debug.WriteLine($"Unknown page tag: {tag}");
                            break;
                    }

                    NavigationRequested?.Invoke(this, pageType);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in navigation selection: {ex.Message}");

            }
        }

    }

}