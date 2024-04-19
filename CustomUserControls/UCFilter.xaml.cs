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
    /// Interaction logic for UCFilter.xaml
    /// </summary>
    public partial class UCFilter : UserControl
    {
        public UCFilter()
        {
            InitializeComponent();
        }

        //Dependency Property
        public static readonly DependencyProperty SortProductByDateCommandProperty =
                 DependencyProperty.Register("SortProductByDateCommand", typeof(ICommand), typeof(UCFilter), new PropertyMetadata(null));
        public ICommand SortProductByDateCommand
        {
            get { return (ICommand)GetValue(SortProductByDateCommandProperty); }
            set { SetValue(SortProductByDateCommandProperty, value); }
        }

        // SortProductByPriceCommand Dependency Property
        public static readonly DependencyProperty SortProductByPriceCommandProperty =
                DependencyProperty.Register("SortProductByPriceCommand", typeof(ICommand), typeof(UCFilter), new PropertyMetadata(null));
        public ICommand SortProductByPriceCommand
        {
            get { return (ICommand)GetValue(SortProductByPriceCommandProperty); }
            set { SetValue(SortProductByPriceCommandProperty, value); }
        }

          // ICommand SortAlphabetCommand 
          public static readonly DependencyProperty SortAlphabetCommandProperty =
                DependencyProperty.Register("SortAlphabetCommand", typeof(ICommand), typeof(UCFilter), new PropertyMetadata(null));
        public ICommand SortAlphabetCommand
        {
            get { return (ICommand)GetValue(SortAlphabetCommandProperty); }
            set { SetValue(SortAlphabetCommandProperty, value); }
        }

        
    }
}
