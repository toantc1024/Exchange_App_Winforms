using Exchange_App.Model;
using Exchange_App.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Exchange_App.View
{
    /// <summary>
    /// Interaction logic for CheckoutView.xaml
    /// </summary>
    public partial class CheckoutView : UserControl
    {
        public class CurrencyToStringConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return ((decimal)value).ToString("C0", CultureInfo.CreateSpecificCulture("vi-VN"));
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return null;
            }
        }

        public CheckoutView()
        {
            InitializeComponent();
        }

        private void Run_ColorChanged(object sender, RoutedPropertyChangedEventArgs<Color> e)
        {

        }

        private void Button_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {

        }
    }
}
