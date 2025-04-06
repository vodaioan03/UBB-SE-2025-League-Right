using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;

namespace Duo.Helpers
{
    public static class Helpers
    {
        public static T? FindParent<T>(DependencyObject child)
            where T : DependencyObject
        {
            DependencyObject? parent = VisualTreeHelper.GetParent(child);
            while (parent != null)
            {
                if (parent is T match)
                {
                    return match;
                }

                parent = VisualTreeHelper.GetParent(parent);
            }

            return null;
        }
    }
}
