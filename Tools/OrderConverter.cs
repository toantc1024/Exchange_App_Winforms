using Exchange_App.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace Exchange_App.Tools
{
    public class ShowTotalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //Model.User_Order p = value as Model.User_Order;
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    // converter visiblity of how many other items
    public class ShowMoreConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Model.User_Order p = value as Model.User_Order;
            if (p != null)
            {
                if (p.OrderDetails.Count > 3)
                    return "Show " + (p.OrderDetails.Count - 3) + " more items";
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class OrderDetailTotalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Model.OrderDetail p = value as Model.OrderDetail;
            double total = p.Quantity * p.Product.Sell_price;
            return total.ToString("C0", CultureInfo.CreateSpecificCulture("vi-VN"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }





    public class ShowMoreVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Model.User_Order p = value as Model.User_Order;
            if (p != null)
            {
                if (p.OrderDetails.Count > 3)
                    return System.Windows.Visibility.Visible;
                else
                    return System.Windows.Visibility.Hidden;
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }



    // converter get first 3 items from User_Order
    public class ShowCancelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Model.User_Order p = value as Model.User_Order;
            if (p == null)
            {
                return System.Windows.Visibility.Hidden;
            }
            if(p.OrderStatus == Constants.OrderConstants.PENDING)
            {
                return System.Windows.Visibility.Visible;
            }
                return System.Windows.Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    // converter get first 3 items from User_Order
    public class PreviewOrderDetails : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Model.User_Order p = value as Model.User_Order;
            if (p != null)
            {
                if (p.OrderDetails.Count > 3)
                {
                    return p.OrderDetails.Take(3).ToList();
                }
                else
                {
                    return p.OrderDetails.ToList();
                }
            }   
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class PendingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            List<User_Order> orders = value as List<User_Order>;
            var result = Helper.FilterOrderByStatus(orders, Constants.OrderConstants.PENDING);
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public  class Helper
    {public static List<User_Order> FilterOrderByStatus(List<User_Order> orders, string status)
        {
            var newOrders = orders.Where(order => order.OrderStatus == status).ToList();
            return newOrders;
        }
    }
        
      

    public class SuccessConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            List<User_Order> orders = value as List<User_Order>;
            var result = Helper.FilterOrderByStatus(orders, Constants.OrderConstants.SUCCESS);
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class CancelledConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            List<User_Order> orders = value as List<User_Order>;
            var result = Helper.FilterOrderByStatus(orders, Constants.OrderConstants.CANCELLED);
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class DeliveringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            List<User_Order> orders = value as List<User_Order>;
            var result = Helper.FilterOrderByStatus(orders, Constants.OrderConstants.DELIVERING);
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class OrderStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
         {
            User_Order p = value as User_Order;
            if (p == null)
            {
                return "";
            }
            
            if(p.OrderStatus == Constants.OrderConstants.PENDING)
            {
                return "Waiting for seller to confirm";
            } else if (p.OrderStatus == Constants.OrderConstants.SUCCESS)
            {
                return "Successfully delivered";
            } else if (p.OrderStatus == Constants.OrderConstants.CANCELLED)
            {
                return "Order has been cancelled";
            } else 
            {
                return "Your order is on track!";
            }
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class OrderStatusColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
         
            var p = value as User_Order;
            if (p == null)
            {
                return "DodgerBlue";
            }
            if (p.OrderStatus == Constants.OrderConstants.PENDING)
            {
                return "Orange";
            }
            else if (p.OrderStatus == Constants.OrderConstants.SUCCESS)
            {
                return "MediumSeaGreen";
            }
            else if (p.OrderStatus == Constants.OrderConstants.CANCELLED)
            {
                return "Gray";
            }
            else
            {
                return "DodgerBlue";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }


}
