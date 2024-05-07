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
    /// Interaction logic for UCReview.xaml
    /// </summary>
    public partial class UCReview : UserControl
    {
        public UCReview()
        {
            InitializeComponent();
        }

        // dependency property Review
        public static readonly DependencyProperty ReviewProperty = DependencyProperty.Register("Review", typeof(Model.Review), typeof(UCReview));

        public Model.Review Review
        {
            get { return (Model.Review)GetValue(ReviewProperty); }
            set { SetValue(ReviewProperty, value); }
        }
    }
}
