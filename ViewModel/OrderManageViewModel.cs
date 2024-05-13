using Exchange_App.Model;
using Exchange_App.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Exchange_App.ViewModel
{
    public class OrderManageViewModel : BaseViewModel
    {
        private Model.User _currentUser;
        private string _isShowOrderDetail = "Hidden";
        private List<User_Order> _userReceivedOrders;
        private User_Order _currentOrder;
        private List<OrderDetail> _currentOrderDetailsOfUser;

        public ICommand UpdateOrderDetailStatus { get; set; }
        public ICommand ShowOrderDetailsCommand { get; set; }
        public ICommand HideOrderDetailsCommand { get; set; }
        public ICommand SetOrderStatusCommand { get; set; }


        private int _orderStatusIndex = 0;
        string[] orderStatus = { "Pending",  "Rejected", "Shipping", "Delivered" };


        public OrderManageViewModel(Model.User user)
        {
            CurrentUser = user;
            GetOrdersOfUser();

            UpdateOrderDetailStatus = new RelayCommand<object>(o =>
            {
                return true;
            }, o =>
            {
                // Count how many order details are in the order is confirmed 
                int count = CurrentOrder.OrderDetails.Count(od => od.IsConfirmed);
                if(count == CurrentOrder.OrderDetails.Count)
                {
                    CurrentOrder.OrderStatus = Constants.OrderConstants.SUCCESS;
                }
                else
                {
                    CurrentOrder.OrderStatus = Constants.OrderConstants.DELIVERING;
                }

                DataProvider.Ins.DB.SaveChanges();
                Notify.ShowNotify("Successfully updated order status", 4, Notify.Success);
            });

            HideOrderDetailsCommand = new RelayCommand<object>(o =>
            {
                return true;
            }, o =>
            {
                IsShowOrderDetail = "Hidden";
            });

            ShowOrderDetailsCommand = new RelayCommand<User_Order>(o =>
            {
                return true;
            }, o =>
            {
                CurrentOrder = o;
                IsShowOrderDetail = "Visible";
            });
            SetOrderStatusCommand = new RelayCommand<object>(o =>
            {
                return true;
            }, o =>
            {
                //User_Order user_Order = CurrentOrder;
                //user_Order.OrderStatus = orderStatus[OrderStatusIndex];
                DataProvider.Ins.DB.SaveChanges();
            });
        }

        private void GetOrdersOfUser()
        {
            var orders  = DataProvider.Ins.DB.User_Order.Where(o => o.OrderDetails.Any(od => od.Product.UserID == CurrentUser.UserID)).ToList();

            UserReceivedOrders = orders;
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


        public User CurrentUser { get => _currentUser; set {
                _currentUser=value;
                OnPropertyChanged();
            } }

   
        public List<User_Order> UserReceivedOrders { get => _userReceivedOrders; set {
                _userReceivedOrders=value;
                OnPropertyChanged();
            } }

        public User_Order CurrentOrder { get => _currentOrder; set {
                _currentOrder=value;
                CurrentOrderDetailsOfUser =  value.OrderDetails.Where(od => od.Product.UserID == CurrentUser.UserID).ToList();
                OnPropertyChanged();
            } }

        public List<OrderDetail> CurrentOrderDetailsOfUser { get => _currentOrderDetailsOfUser; set {
                _currentOrderDetailsOfUser=value;
                OnPropertyChanged();
            }
        }

        //public List<User_Order> Orders { get => _orders; set {
        //        _orders=value;
        //        OnPropertyChanged();
        //    } }

        //public User_Order CurrentOrder { get => _currentOrder; set {
        //        _currentOrder=value;
        //        OrderStatusIndex = 0;
        //        // declare array of string
        //        // get the index of the order status
        //        OrderStatusIndex = Array.IndexOf(orderStatus, value.OrderStatus);
        //        OnPropertyChanged();

        //    } }

        //public int OrderStatusIndex { get => _orderStatusIndex; set {
        //        _orderStatusIndex=value;
        //        OnPropertyChanged();
        //    }
        //}
    }
}
