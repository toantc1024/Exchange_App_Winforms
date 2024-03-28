﻿using Exchange_App.Model;
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
    /// Interaction logic for MyProducts.xaml
    /// </summary>
    public partial class MyProducts : UserControl
    {
        public MyProducts()
        {
            InitializeComponent();
        }

        #region Dependency Properties

        // ChangeView ICommand Dependency Property

        public static readonly DependencyProperty ChangeViewCommandProperty =
                DependencyProperty.Register("ChangeViewCommand", typeof(ICommand), typeof(MyProducts), new PropertyMetadata(null));
        public ICommand ChangeViewCommand
        {
            get { return (ICommand)GetValue(ChangeViewCommandProperty); }
            set { SetValue(ChangeViewCommandProperty, value); }
        }


        // List of Products Dependency Property and OnChange event
        private static readonly DependencyProperty ProductsProperty =
            DependencyProperty.Register("Products", typeof(List<Product>), typeof(MyProducts), new PropertyMetadata(null));

        public List<Product> Products
        {
            get { return (List<Product>)GetValue(ProductsProperty); }
            set { SetValue(ProductsProperty, value); }
        }

        // ICommand GetProductListCommand Dependency Property
        public static readonly DependencyProperty GetProductListCommandProperty =
                DependencyProperty.Register("GetProductListCommand", typeof(ICommand), typeof(MyProducts), new PropertyMetadata(null));
        public ICommand GetProductListCommand
        {
            get { return (ICommand)GetValue(GetProductListCommandProperty); }
            set { SetValue(GetProductListCommandProperty, value); }
        }

        // ShowPreviewCommand Dependency Property
        public static readonly DependencyProperty ShowPreviewCommandProperty =
                DependencyProperty.Register("ShowPreviewCommand", typeof(ICommand), typeof(MyProducts), new PropertyMetadata(null));

        public ICommand ShowPreviewCommand
        {
            get { return (ICommand)GetValue(ShowPreviewCommandProperty); }
            set { SetValue(ShowPreviewCommandProperty, value); }
        }

        // SelectionChangedCommand Dependency Property
        public static readonly DependencyProperty SelectionChangedCommandProperty =
                DependencyProperty.Register("SelectionChangedCommand", typeof(ICommand), typeof(MyProducts), new PropertyMetadata(null));
        public ICommand SelectionChangedCommand
        {
            get { return (ICommand)GetValue(SelectionChangedCommandProperty); }
            set { SetValue(SelectionChangedCommandProperty, value); }
        }

        // DeleteProductCommand Dependency Property
        public static readonly DependencyProperty DeleteProductCommandProperty =
                DependencyProperty.Register("DeleteProductCommand", typeof(ICommand), typeof(MyProducts), new PropertyMetadata(null));

        public ICommand DeleteProductCommand
        {
            get { return (ICommand)GetValue(DeleteProductCommandProperty); }
            set { SetValue(DeleteProductCommandProperty, value); }
        }

        #endregion

    }
}
