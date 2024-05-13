using Exchange_App.Model;
using Exchange_App.Tools;
using Exchange_App.View;
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
    public class OrderViewModel : BaseViewModel
    {
        #region Variables

        private string _isShowOrderDetail = "Hidden";
        private User _currentUser;
        private User_Order _currentOrder;
        private List<User_Order> _orders;
        private bool _isShowConfirmCancelOrder;

        #endregion

        #region Properties



        public string IsShowOrderDetail
        {
            get => _isShowOrderDetail;

            set
            {
                _isShowOrderDetail = value;
                


                OnPropertyChanged();
            }
        }

        public User_Order CurrentOrder
        {
            get { return _currentOrder; }
            set
            {
                
                _currentOrder = value;
                OnPropertyChanged();
            }
        }   

        public User CurrentUser
        {
            get { return _currentUser; }
            set
            {
                _currentUser = value;
                OnPropertyChanged();
            }
        }

        public List<User_Order> Orders
        {
            get { return _orders; }
            set
            {
                _orders = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        public ICommand ChangeGroupOrderCommand { get; set; }

        public ICommand CancelOrderCommand { get; set; }
        public ICommand ShowConfirmCancelOrder
        {
            get;set;
        }

        public ICommand OnSearchCommand
        {
            get;
            set;
        }

        public ICommand ShowReviewCommand
        {
            get;set;
        }

        public ICommand ShowOrderDetailsCommand
        {
            get;
            set;
        }

        public ICommand HideOrderDetail
        {
            get; set;
        }

        public ICommand SortProductByDateCommand { get; set; }

            public ICommand SortProductByPriceCommand { get; set; }


        public ICommand SortAlphabetCommand
        {
            get;set;
        }
        public bool IsShowConfirmCancelOrder { get => _isShowConfirmCancelOrder; set {
                _isShowConfirmCancelOrder=value;
                OnPropertyChanged();
            } }
        #endregion

        public void LoadOrders(string keyword)
        {
            if (CurrentUser.RoleID == 2)
            {
                Orders = DataProvider.Ins.DB.User_Order.Where(x => x.OrderDetails.Any(product => product.Product.ProductName.Contains(keyword))).ToList();
            }
            else
            {
                Orders = CurrentUser.User_Order.Where(x => x.OrderDetails.Any(product => product.Product.ProductName.Contains(keyword))).ToList();
            }
        }


        public OrderViewModel(User user)
        {


            CurrentUser = user;
            Orders = DataProvider.Ins.DB.User_Order.Where(x => x.UserID == CurrentUser.UserID).ToList();
            ChangeGroupOrderCommand = new RelayCommand<object>(p =>
            {

                if (p == null)
                {
                    return false;
                }
                return true;
            }, p =>
            {
                string type = p.ToString();
                var originalOrders = DataProvider.Ins.DB.User_Order.Where(x => x.UserID == CurrentUser.UserID).ToList();
                Orders = originalOrders.Where(x => x.OrderStatus == type).ToList();
            });

            ChangeGroupOrderCommand.Execute(Constants.OrderConstants.PENDING);

            ShowReviewCommand = new RelayCommand<object>(
                (p) =>
                {
                    return true;
                }, (p) =>
                {

                    ReviewView reviewView = new ReviewView();
                    //reviewView.DataContext = new ReviewViewModel(CurrentUser, CurrentOrder.Product);
                    reviewView.ShowDialog();

                });

            SortAlphabetCommand = new RelayCommand<ListBox>(p =>
            {
                return true;
            }, p =>
            {
                var type = p.SelectedIndex;
                if(type ==0)
                {
                    // sort desc
                    Orders = Orders.OrderByDescending(x => x.OrderDetails.FirstOrDefault().Product.ProductName).ToList();
                } else
                {
                    Orders = Orders.OrderBy(x => x.OrderDetails.FirstOrDefault().Product.ProductName).ToList();
                }
            });

          

            SortProductByPriceCommand = new RelayCommand<object>(p =>
            {
                return true;
            }, p =>
            {
                Orders = Orders.OrderByDescending(x => x.TotalPrice).ToList();
            });

            SortProductByDateCommand = new RelayCommand<object>(p =>
            {
                return true;
            }, p =>
            {
                Orders = Orders.OrderByDescending(x => x.OrderDate).ToList();
            });


            ShowOrderDetailsCommand = new RelayCommand<User_Order>(p =>
            {
                return true;
            }, p =>
            {
                
                CurrentOrder = p;
                IsShowOrderDetail = "Visible";
            }); 
           

            OnSearchCommand = new RelayCommand<TextBox>(p =>
            {
                if(p == null)
                {
                    return false;
                }
                return true;
            }, p =>
            {
                string keyword = p.Text;
                LoadOrders(keyword);
            });

            CancelOrderCommand = new RelayCommand<object>(
                (p) =>
                {
                    return true;
                }, (p) =>
                {
                    if (CurrentOrder.OrderStatus == Constants.OrderConstants.PENDING)
                    {
                        var order = DataProvider.Ins.DB.User_Order.Where(x => x.OrderID == CurrentOrder.OrderID).FirstOrDefault().OrderStatus = Constants.OrderConstants.CANCELLED;
                        IsShowOrderDetail = "Hidden";
                        IsShowConfirmCancelOrder = false;
                        DataProvider.Ins.DB.SaveChanges();
                  
                       LoadOrders("");
                        Notify.ShowNotify("Order has been cancelled",  4, Notify.Success);
                    }
                    else
                    {
                        MessageBox.Show("You can't cancel this order");
                    }
                });

            ShowConfirmCancelOrder = new RelayCommand<object>(
                (p) =>
                {
                    return true;
                }, (p) =>
                {
                    IsShowConfirmCancelOrder = true; 
                });

            HideOrderDetail = new RelayCommand<object>(p =>
            {
                return true;
            }, p =>
            {
                IsShowOrderDetail = "Hidden";
                CurrentOrder = null;
            });
        }


    }
}

