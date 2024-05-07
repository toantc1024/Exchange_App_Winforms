    using Exchange_App.Model;
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

        private int _receivingMethod = 0;
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

        private readonly ClickCommand 
            _locationItemChangedCommand;
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

        public int ReceivingMethod { get => _receivingMethod; set
            {
                _receivingMethod=value;OnPropertyChanged();
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
        public CheckoutViewModel(User currentUser, Product selectedProduct, ICommand hideCheckoutCommand, int defaultQuantity)
        {
            #region Initialize
            CurrentUser = currentUser;
            SelectedProduct = selectedProduct;
            HideCheckoutCommand = hideCheckoutCommand;
            OrderQuantity = defaultQuantity;
            CalculateOrderPrice();
            #endregion

            #region Implement Commands

            IncreaseQuantityCommand = new RelayCommand<object>(
              (p) => {
                  return true;
              },
              (p) => {
                  if(OrderQuantity + 1> SelectedProduct.Quantity)
                  {
                      MessageBox.Show("Limit exceed"); 
                   } else
                  {
                      OrderQuantity += 1;
                      CalculateOrderPrice();
                  }
              }
            );

            DecreaseQuantityCommand = new RelayCommand<object>(
              (p) => {
                  return true;
              },
              (p) => {
                  if (OrderQuantity <= 1)
                  {
                      MessageBox.Show("Quantity must be at least 1");
                    }
                else
                  {
                      OrderQuantity -= 1;
                      CalculateOrderPrice();
                  }
              });

            PlaceOrder = new RelayCommand<object>(
              (p) => {
                 if(p== null)
                  {
                      return false;
                  }
                  return true;
              },
              (p) => {
                  // check if the product is still available
                  try
                  {
                      // check if UserID == Product.UserID
                      if (CurrentUser.UserID == SelectedProduct.UserID)
                      {
                          throw new Exception("You can't order your own product");
                      }
                      DataProvider.Ins.DB.User_Order.Add(new User_Order
                      {
                          UserID = CurrentUser.UserID,
                          ProductID = SelectedProduct.ProductID,
                          Quantity = OrderQuantity,
                          OrderDate = DateTime.Now,
                          OrderStatus = "Pending"
                      });
                      DataProvider.Ins.DB.Products.Where(x => x.ProductID == SelectedProduct.ProductID).FirstOrDefault().Quantity -= OrderQuantity;

                      DataProvider.Ins.DB.SaveChanges();
                      MessageBox.Show("Order successfully!");
                      HideCheckoutCommand.Execute(SelectedProduct.ProductID);

                  } catch (Exception ex)
                  {
                      MessageBox.Show(ex.Message);
                  } 
              }

            );

            #endregion
        }

    }
}