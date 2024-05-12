using Exchange_App.Model;
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
        #endregion


        public OrderViewModel(User user)
        {


            CurrentUser = user;
            Orders = DataProvider.Ins.DB.User_Order.Where(x => x.UserID == CurrentUser.UserID).ToList();

            ShowReviewCommand = new RelayCommand<object>(
                (p) =>
                {
                    return true;
                }, (p) =>
                {

                    ReviewView reviewView = new ReviewView();
                    reviewView.DataContext = new ReviewViewModel(CurrentUser, CurrentOrder.Product);
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
                    Orders = Orders.OrderByDescending(x => x.Product.ProductName).ToList();
                } else
                {
                    Orders = Orders.OrderBy(x => x.Product.ProductName).ToList();
                }
            });

            SortProductByPriceCommand = new RelayCommand<object>(p =>
            {
                return true;
            }, p =>
            {
                Orders = Orders.OrderByDescending(x => 0).ToList();
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
                if (CurrentUser.RoleID == 2)
                {
                    Orders = DataProvider.Ins.DB.User_Order.Where(x => x.Product.ProductName.Contains(keyword)).ToList();
                }
                else
                {
                    Orders = CurrentUser.User_Order.Where(x => x.Product.ProductName.Contains(keyword)).ToList();
                }
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
