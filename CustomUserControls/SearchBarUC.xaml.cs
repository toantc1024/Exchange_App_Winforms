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
    /// Interaction logic for SearchBarUC.xaml
    /// </summary>
    public partial class SearchBarUC : UserControl
    {
        public SearchBarUC()
        {
            InitializeComponent();
        }

        // OnSearchCommand 
        public static readonly DependencyProperty OnSearchCommandProperty =
                DependencyProperty.Register("OnSearchCommand", typeof(ICommand), typeof(SearchBarUC), new PropertyMetadata(null));
        public ICommand OnSearchCommand
        {
            get { return (ICommand)GetValue(OnSearchCommandProperty); }
            set { SetValue(OnSearchCommandProperty, value); }
        }

        // HintText string Dependency Property

        public static readonly DependencyProperty HintTextProperty =
                DependencyProperty.Register("HintText", typeof(string), typeof(SearchBarUC), new PropertyMetadata(null));

        public string HintText
        {
            get { return (string)GetValue(HintTextProperty); }
            set { SetValue(HintTextProperty, value); }
        }

       

    }
}
