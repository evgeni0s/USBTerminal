using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace USBTetminal2.Utils
{
    //Takes legen box visibility state and visibility of selected graph
    //returns true is both are fine
    public class LegendDetailsVisibilityConverter : IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values.Count()!= 2 || !(values[0] is Visibility) || !(values[1] is bool))
                return Visibility.Hidden;
            bool isLegendVisible = (Visibility)values[0] == Visibility.Visible;
            bool isChecked = (bool)values[1];
            return(isChecked && isLegendVisible) ? Visibility.Visible : Visibility.Collapsed; 
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
