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

namespace Exchange_App.View
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : UserControl
    {
        public HomeView()
        {
            InitializeComponent();
        }


        #region Dependency Properties

        // ShowProductDetailCommand Dependency Property
        public static readonly DependencyProperty ShowProductDetailCommandProperty =
                DependencyProperty.Register("ShowProductDetailCommand", typeof(ICommand), typeof(HomeView), new PropertyMetadata(null));

        public ICommand ShowProductDetailCommand
        {
            get { return (ICommand)GetValue(ShowProductDetailCommandProperty); }
            set { SetValue(ShowProductDetailCommandProperty, value); }
        }

        #endregion

        private void StackPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("StackPanel_MouseLeftButtonDown");
        }

        private void TextBlock_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {

        }

        
    }
}
