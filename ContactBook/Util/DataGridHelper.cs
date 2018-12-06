using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ContactBook.Util
{
    public class DataGridHelper
    {
        public static void OnMouseRightButtonUp(MouseButtonEventArgs e, Action<DataGridRow> action)
        {
            DependencyObject dep = e.OriginalSource as DependencyObject;
            while ((dep != null) && !(dep is DataGridCell))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }
            if (dep == null) return;

            while ((dep != null) && !(dep is DataGridRow))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }
            if (dep == null) return;

            DataGridRow row = dep as DataGridRow;
            action(row);
        }
    }
}
