using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Exchange_App.Tools
{

    public class DiscountToStringConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = System.Convert.ToDouble(value);
            if(val < 0)
            {
                val = Math.Abs(val);
                return val.ToString("0.##") + "%";
            }
            return val.ToString("0.##") + "%";

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class CurrencyToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = System.Convert.ToDecimal(value);
            return val.ToString("C0", CultureInfo.CreateSpecificCulture("vi-VN"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class VNCurrencyConveter
    {
        public static string ConvertDoubleToCurrency(double value)
        {
            return value.ToString("C0", CultureInfo.CreateSpecificCulture("vi-VN"));
        }
    }
}
