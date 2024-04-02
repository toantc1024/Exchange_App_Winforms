using Exchange_App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Exchange_App.ViewModel
{
    public class CheckoutViewModel : BaseViewModel
    {

        #region Variables
        private User _currentUser;
        private Product _selectedProduct;
        private int _orderQuantity = 0;
        #endregion

        #region Properties
        public User CurrentUser
        {
            get
            {
                return _currentUser;
            }
            set
            {
                _currentUser = value;
                OnPropertyChanged("CurrentUser");
            }
        }

        public Product SelectedProduct
        {
            get
            {
                return _selectedProduct;
            }
            set
            {
                _selectedProduct = value;
                OnPropertyChanged("SelectedProduct");
            }
        }

        #endregion

        #region Commands

        public ICommand PlaceOrder
        {
            get;
            set;
        }
        public int OrderQuantity { get => _orderQuantity; set
            {
                _orderQuantity=value;
                OnPropertyChanged();
            }
        }



        #endregion
        public CheckoutViewModel(User currentUser, Product selectedProduct)
        {
            #region Initialize
            CurrentUser = currentUser;
            SelectedProduct = selectedProduct;

            #endregion

            #region Implement Commands

            PlaceOrder = new RelayCommand<Window>(
              (p) => {
                  if (p == null)
                      return false;
                  return true;
              },
              (p) => {
                  // check if the product is still available
                  if (SelectedProduct.Quantity == 0)
                  {
                        System.Windows.MessageBox.Show("This product is out of stock");
                        return;
                    }

                  User_Order order = 
                  DataProvider.Ins.DB.User_Order.Add(new User_Order
                  {
                      UserID = CurrentUser.UserID,
                      OrderDate = DateTime.Now,
                      Status = "Pending"
                  });

                  OrderDetail orderDetail = DataProvider.Ins.DB.OrderDetails.Add(new OrderDetail
                  {
                        OrderID = order.OrderID,
                        ProductID = SelectedProduct.ProductID,
                        Quantity = 1,
                    });

                  SelectedProduct.Quantity -= 1;
                  DataProvider.Ins.DB.SaveChanges();

                  MessageBox.Show("Order placed successfully");

                  p.Close();

              }

            );

            #endregion
        }
    }
}