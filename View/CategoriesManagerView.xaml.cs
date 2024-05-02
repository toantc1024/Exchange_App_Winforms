using Exchange_App.ViewModel;
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
using System.Windows.Shapes;

namespace Exchange_App.View
{
    /// <summary>
    /// Interaction logic for CategoriesManagerView.xaml
    /// </summary>
    public partial class CategoriesManagerView : Window
    {
        public CategoryManagerViewModel ViewModel
        {
            get;
            set;
        }
        public CategoriesManagerView()
        {
            InitializeComponent();
            this.DataContext = ViewModel = new CategoryManagerViewModel();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
