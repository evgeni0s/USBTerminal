/*Used in Shell to display points with 
 * x = number
 * y = number*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace USBTetminal2
{
    public  class DataConverter : IValueConverter
    {


        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (parameter == null || parameter.ToString().ToLower() == "x")
            {
               return "x = " + value.ToString();
            }
            else if (parameter.ToString().ToLower() == "y")
            {
                return "y = " + value.ToString();
            }
            return value;

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
