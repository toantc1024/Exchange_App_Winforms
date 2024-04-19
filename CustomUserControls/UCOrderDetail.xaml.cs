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
    /// Interaction logic for UCOrderDetail.xaml
    /// </summary>
    public partial class UCOrderDetail : UserControl
    {
        public UCOrderDetail()
        {
            InitializeComponent();
        }

        // Dependency Properties
        public static readonly DependencyProperty OrderDetailProperty =
                DependencyProperty.Register("OrderDetail", typeof(Model.OrderDetail), typeof(UCOrderDetail), new PropertyMetadata(null));
        public Model.OrderDetail OrderDetail
        {
            get { return (Model.OrderDetail)GetValue(OrderDetailProperty); }
            set { SetValue(OrderDetailProperty, value); }
        }
    }
}
