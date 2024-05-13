using Exchange_App.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Exchange_App.Tools
{
    public class CartItemTotalConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Cart cart = (Cart)value;
            double total = cart.Quantity * cart.Product.Sell_price;
            return total.ToString("C0", CultureInfo.CreateSpecificCulture("vi-VN"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
    public class CartTotalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ObservableCollection<Cart> cart = (ObservableCollection<Cart>)value;


            double total = cart.Sum(item => item.Quantity * item.Product.Sell_price);

            return total.ToString("C0", CultureInfo.CreateSpecificCulture("vi-VN"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }



}
