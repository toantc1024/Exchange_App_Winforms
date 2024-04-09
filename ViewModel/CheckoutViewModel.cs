﻿using Exchange_App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Exchange_App.ViewModel
{
    public class CheckoutViewModel : BaseViewModel
    {

        #region Variables

        private double _total = 0;

        private User _currentUser;
        private Product _selectedProduct;
        private int _orderQuantity = 1;

        private double _orderPrice = 0;
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
        public double Total
        {
            get => _total;
            set
            {

                _total = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        public ICommand HideCheckoutCommand 
        {
            get;
            set;
        }

        private readonly ClickCommand _locationItemChangedCommand;
        public ClickCommand LocationItemChangedCommand
        {
            get
            {
                return _locationItemChangedCommand ?? (new ClickCommand(obj =>
                {
                    ListBox listBox = (obj as ListBox);
                    var item = listBox.SelectedIndex;
                    if (item != -1)
                    {
                        MessageBox.Show("Please choose at least one choice");
                    }
                    if (item == 0)
                    {
                        // set address as usual
                    } else if (item == 1)
                    {
                    }
                }));
            }
        }

        public ICommand PlaceOrder
        {
            get;
            set;
        }
        public int OrderQuantity
        {
            get => _orderQuantity;
            set
            {
                _orderQuantity = value;
                OnPropertyChanged();
            }
        }
        

        public ICommand IncreaseQuantityCommand
        {
            get;
            set;
        }
        public ICommand DecreaseQuantityCommand
        {
            get;
            set;
        }
        public double OrderPrice
        {
            get => _orderPrice;
            set
            {
                _orderPrice = value;
                OnPropertyChanged();
            }
        }

        public void CalculateOrderPrice()
        {
            OrderPrice = OrderQuantity * SelectedProduct.Sell_price;
            CalculateTotal();
        }

        public void CalculateTotal()
        {

            Total = OrderPrice + 0;

        }

        #endregion
        public CheckoutViewModel(User currentUser, Product selectedProduct, ICommand hideCheckoutCommand)
        {
            #region Initialize
            CurrentUser = currentUser;
            SelectedProduct = selectedProduct;
            HideCheckoutCommand = hideCheckoutCommand;
            CalculateOrderPrice();
            #endregion

            #region Implement Commands

            IncreaseQuantityCommand = new RelayCommand<object>(
              (p) => {
                  return true;
              },
              (p) => {
                  OrderQuantity += 1;
                  CalculateOrderPrice();
              }
            );

            DecreaseQuantityCommand = new RelayCommand<object>(
              (p) => {
                  return true;
              },
              (p) => {
                  if (OrderQuantity > 0)
                  {
                      OrderQuantity -= 1;
                      CalculateOrderPrice();
                  }
              });

            PlaceOrder = new RelayCommand<object>(
              (p) => {
                  return true;
              },
              (p) => {
                  // check if the product is still available
                  if (SelectedProduct.Quantity == 0)
                  {
                      MessageBox.Show("This product is out of stock");
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

                  DataProvider.Ins.DB.SaveChanges();

                  MessageBox.Show("Order placed successfully");

                  HideCheckoutCommand.Execute(null);
              }

            );

            #endregion
        }

    }
}