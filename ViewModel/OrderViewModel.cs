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
    public class OrderViewModel : BaseViewModel
    {
        #region Variables

        private string _isShowOrderDetail = "Hidden";
        private User _currentUser;
        private User_Order _currentOrder;
        private List<User_Order> _orders;

        #endregion

        #region Properties

        public string currentOrderName
        {
            get
            {
                return CurrentOrder.OrderName;
            }
        }

        public DateTime currentOrderDate
        {
            get
            {
                return CurrentOrder.OrderDate;
            }

        }

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
            get => _currentOrder; set
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

        public ICommand ShowOrderDetailsCommand
        {
            get;
            set;
        }

        public ICommand HideOrderDetail
        {
            get; set;
        }

        #endregion

        public OrderViewModel(User user)
        {
            CurrentUser = user;
            Orders = CurrentUser.User_Order.ToList();

            ShowOrderDetailsCommand = new RelayCommand<User_Order>(p =>
            {
                if(p == null)
                {
                    return false;
                }
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
                Orders = CurrentUser.User_Order.Where(x => x.OrderName.Contains(keyword)).ToList();
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
