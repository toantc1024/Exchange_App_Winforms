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
    /// Interaction logic for ProductDetail.xaml
    /// </summary>
    public partial class ProductDetail : UserControl
    {
        public ProductDetail()
        {
            InitializeComponent();
        }

        #region Dependency Properties
        
        // Product Dependency Property
        public static readonly DependencyProperty ProductProperty =
                DependencyProperty.Register("Product", typeof(Model.Product), typeof(ProductDetail), new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(ValueChanged)));

        private static void ValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ProductDetail)d).Product =((ProductDetail)d).Product;
        }
        public Model.Product Product
        {
            get { return (Model.Product)GetValue(ProductProperty); }
            set { SetValue(ProductProperty, value); }
        }

    
        // Derives Product isnto ProductID, ProductName, ProductDescription, ProductPrice, ProductImage from Product
        
        public string ProductName
        {
            get { return Product.ProductName; }
            set { ; }
        }

        public int ProductID
        {
            get { return Product.ProductID; }
            set { ; }
        }
        
        public string ProductPreviewImage
        {
            get
            {
                if (Product.Images.Count > 0)
                {
                    return Product.Images.FirstOrDefault().ImageURL;
                } else
                {
                    return "https://via.placeholder.com/150";
                }
            }
        }

        public List<Model.Image> ProductImages
        {
            get
            {
                List<Model.Image> images = Product.Images.ToList();
                return images;
            }
        }
        #endregion

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(ProductPreviewImage);
        }
    }
}
