using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Duo.Helpers
{
    public static class Helpers
    {
        public static T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parent = VisualTreeHelper.GetParent(child);
            while (parent != null)
            {
                if (parent is T tParent)
                    return tParent;
                parent = VisualTreeHelper.GetParent(parent);
            }
            return null;
        }
    }
}
