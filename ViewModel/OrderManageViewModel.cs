using Exchange_App.Model;
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
        private User_Order _currentOrder;
        private List<User_Order> _orders;
        
        public ICommand ShowOrderDetailsCommand { get; set; }
        public ICommand SetOrderStatusCommand { get; set; }


        private int _orderStatusIndex = 0;
        string[] orderStatus = { "Pending",  "Rejected", "Shipping", "Delivered" };


        public OrderManageViewModel(Model.User user)
        {
            CurrentUser = user;
            GetOrdersOfUser();

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
                User_Order user_Order = CurrentOrder;
                user_Order.OrderStatus = orderStatus[OrderStatusIndex];
                DataProvider.Ins.DB.SaveChanges();
            });
        }

        private void GetOrdersOfUser()
        {
            var products = CurrentUser.Products;
            var orders = new List<User_Order>();
            foreach (Product product in products)
            {
                product.User_Order.ToList().ForEach(order => orders.Add(order));
            }
            Orders = orders;

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

        public List<User_Order> Orders { get => _orders; set {
                _orders=value;
                OnPropertyChanged();
            } }

        public User_Order CurrentOrder { get => _currentOrder; set {
                _currentOrder=value;
                OrderStatusIndex = 0;
                // declare array of string
                // get the index of the order status
                OrderStatusIndex = Array.IndexOf(orderStatus, value.OrderStatus);
                OnPropertyChanged();

            } }

        public int OrderStatusIndex { get => _orderStatusIndex; set {
                _orderStatusIndex=value;
                OnPropertyChanged();
            }
        }
    }
}
