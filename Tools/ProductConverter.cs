using Exchange_App.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Exchange_App.Tools
{
    public class SoldItemConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Product p = value as Product;
            //var sold = DataProvider.Ins.DB.User_Order.Where(x => x.ProductID == p.ProductID).ToList();
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
    public class QuantityColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = System.Convert.ToInt32(value);
            if (val > 0)
            {
                return "Green";
            }
            else
            {
                return "Red";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class QuantityTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = System.Convert.ToInt32(value);
            if (val > 0)
            {
                return val + " items";
            }
            return "Out of stock";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
    public class UploadedDateConveter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // cast value to DateTime
            var date = (DateTime)value;
            TimeSpan timeSpan = DateTime.Now - date;
            if (timeSpan.TotalDays < 1)
            {
                return "Today";
            }
            else if (timeSpan.TotalDays < 2)
            {
                return "Yesterday";
            }
            else if (timeSpan.TotalDays < 7)
            {
                return timeSpan.Days + " days ago";
            }
            else if (timeSpan.TotalDays < 14)
            {
                return "1 week ago";
            }
            else if (timeSpan.TotalDays < 30)
            {
                return timeSpan.Days / 7 + " weeks ago";
            }
            else if (timeSpan.TotalDays < 60)
            {
                return "1 month ago";
            }
            else if (timeSpan.TotalDays < 365)
            {
                return timeSpan.Days / 30 + " months ago";
            }
            else if (timeSpan.TotalDays < 730)
            {
                return "1 year ago";
            }
            else
            {
                return timeSpan.Days / 365 + " years ago";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
