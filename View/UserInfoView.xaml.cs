﻿using System;
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
    /// Interaction logic for UserInfoView.xaml
    /// </summary>
    public partial class UserInfoView : UserControl
    {
        public UserInfoView()
        {
            InitializeComponent();
        }

        #region Dependency Properties

        // ShowProductDetailCommand Dependency Property
        public static readonly DependencyProperty ShowWishItemDetailCommandProperty =
                DependencyProperty.Register("ShowWishItemDetailCommand", typeof(ICommand), typeof(UserInfoView), new PropertyMetadata(null));

        public ICommand ShowWishItemDetailCommand
        {
            get { return (ICommand)GetValue(ShowWishItemDetailCommandProperty); }
            set { SetValue(ShowWishItemDetailCommandProperty, value); }
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
