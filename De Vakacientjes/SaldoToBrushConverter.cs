using System;
using System.Collections.Generic;
using System.Windows.Media; //Right name space for Brushes here!!!
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace De_Vakacientjes
{
    class SaldoToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int input = System.Convert.ToInt32(value);

            if (input >= 0)
                return Brushes.LightGreen;
            else
                return Brushes.OrangeRed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
