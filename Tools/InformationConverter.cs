using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Exchange_App.Tools
{
   
        public class StringToListConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
            // create list of string from string
            var val = value.ToString().Split(',').ToList();
            return val;    
        }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return null;
            }
    }
}
