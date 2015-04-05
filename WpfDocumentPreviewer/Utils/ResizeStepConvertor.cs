using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WpfDocumentPreviewer.Utils
{
    //REsizing Excel preview is slow. This convertor enables resizing with given persision. 
    public class ResizeStepConvertor: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (parameter == null) return value;
            int precision = 0;
            if (!int.TryParse(parameter.ToString(), out precision))
             return value;  
            double originalValue = (double)value;
            return originalValue -(originalValue % precision);

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
