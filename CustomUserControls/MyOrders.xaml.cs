using Exchange_App.Model;
using System;
using System.Collections.Generic;
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

namespace Exchange_App.CustomUserControls
{
    /// <summary>
    /// Interaction logic for MyOrders.xaml
    /// </summary>
    public partial class MyOrders : UserControl
    {
        public MyOrders()
        {
            InitializeComponent();
        }

        #region Dependency Properties   

        // List of Orders Dependency Property and OnChange event
        private static readonly DependencyProperty OrdersProperty =
            DependencyProperty.Register("Orders", typeof(List<User_Order>), typeof(MyOrders), new PropertyMetadata(null));

        public List<User_Order> Orders
        {
            get { return (List<User_Order>)GetValue(OrdersProperty); }
            set { SetValue(OrdersProperty, value); }
        }


        // ShowOrderDetails ICommand Dependency Property
        public static readonly DependencyProperty ShowOrderDetailsCommandProperty =
                DependencyProperty.Register("ShowOrderDetailsCommand", typeof(ICommand), typeof(MyOrders), new PropertyMetadata(null));

        public ICommand ShowOrderDetailsCommand
        {
            get { return (ICommand)GetValue(ShowOrderDetailsCommandProperty); }
            set { SetValue(ShowOrderDetailsCommandProperty, value); }
        }

        #endregion
    }
}
