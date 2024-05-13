using Exchange_App.Model;
using Exchange_App.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace Exchange_App.ViewModel
{
    public class CartViewModel : BaseViewModel
    {
        #region Variables
        private Model.User _currentUser;
        private ObservableCollection<Model.Cart> _currentCart;
        private Model.Cart _selectedCart; 
        private bool _isOpenRemoveDialog;
        private bool _isShowOrder;
        private bool _isOrderSuccess;
        #endregion

        public User CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Model.Cart> CurrentCart
        {
            get => _currentCart;
            set
            {
                _currentCart = value;
                OnPropertyChanged();
            }
        }

        public void Initialize()
        {
            CurrentCart = new ObservableCollection<Model.Cart>(CurrentUser.Carts);

        }

        #region Commands

        public ICommand ChangeToHome { get; set; }
        public ICommand OrderCommand { get; set; }
        
        public ICommand CloseOrderSuccess { get; set; }
        public ICommand  ShowOrderCommand { get; set; }
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
        public ICommand RemoveItemCommand
        {
            get;
            set;
        }
        public ICommand CheckoutCommand
        {
            get;
            set;
        }


        public ICommand ConfirmDeleteCommand
        {
            get;set;
        }

        public ICommand UpdateQuantityCommand
        {
            get;
            set;
        }
        public bool IsOpenRemoveDialog { get => _isOpenRemoveDialog; set {
                _isOpenRemoveDialog=value;
                OnPropertyChanged();
            } }

        public Cart SelectedCart { get => _selectedCart; set {
                _selectedCart=value;
                OnPropertyChanged();
            } }

        public bool IsShowOrder { get => _isShowOrder; set {
                _isShowOrder = value;
                OnPropertyChanged();
            } }
        public bool IsOrderSuccess { get => _isOrderSuccess; set {

                _isOrderSuccess=value;
                OnPropertyChanged();
            }
        }

        #endregion
        

        #region Methods



        public void UpdateCart(Model.Cart p, int type)
        {
          try
            {
                if (type == 0)
                {
                    if(p.Quantity == 1)
                    {
                        // execute
                        RemoveItemCommand.Execute(p);
                    } else
                    {
                        --p.Quantity;
                    }
                }
                else
                {
                    if (p.Quantity > p.Product.Quantity)
                    {
                        Notify.ShowNotify("Quantity is exceed the limit", 4, Notify.Error);
                        return;
                    }
                    ++p.Quantity;
                }
                DataProvider.Ins.DB.SaveChanges();

                CurrentCart = new ObservableCollection<Model.Cart>(CurrentUser.Carts);
            } catch
            {

            }
        }
        
        #endregion


        public CartViewModel(User user, ICommand ChangeToHome)
        {
            CurrentUser = user;
            Initialize();

            #region Implementations

            RemoveItemCommand = new RelayCommand<Model.Cart>(
                (p) =>
                {
                    return true;
                }, (p) =>
                {
                    SelectedCart = p;
                    IsOpenRemoveDialog = true;
                }
                );

            OrderCommand = new RelayCommand<object>(
                (p) =>
                {
                    return true;
                }, (p) =>
                {

                });

            ConfirmDeleteCommand = new RelayCommand<object>(
                (p) =>
                {
                    return true;
                }, (p) =>
                {
                    // remove p from cart
                    CurrentUser.Carts.Remove(SelectedCart);
                    DataProvider.Ins.DB.SaveChanges();
                    CurrentCart = new ObservableCollection<Model.Cart>(CurrentUser.Carts);
                    IsOpenRemoveDialog = false;
                    Notify.ShowNotify("Remove item successfully", 4, Notify.Success);

                }
                );


            IncreaseQuantityCommand = new RelayCommand<Model.Cart>(
              (p) => {
                  return true;
              },
              (p) => {
                  UpdateCart(p, 1);
              });

            DecreaseQuantityCommand = new RelayCommand<Model.Cart>(
              (p) => {
                  return true;
              },
              (p) => {
                  UpdateCart(p, 0);

              }
            );

            UpdateQuantityCommand = new RelayCommand<Model.Cart>(
              (p) => {
                  return true;
              },
              (p) => {
                  DataProvider.Ins.DB.SaveChanges();

              }
            );

            ShowOrderCommand = new RelayCommand<object>(
                               (p) =>
                               {
                                   return true;
                               }, (p) =>
                               {
                                   // Check if the cart is empty
                                   if (CurrentCart.Count == 0)
                                   {
                                       Notify.ShowNotify("Cart is empty", 4, Notify.Warning);
                                       return;
                                   } 
                                   IsShowOrder = true;
                               }
                               );


            CloseOrderSuccess = new RelayCommand<object>(
                               (p) =>
                               {
                                   return true;
                               }, (p) =>
                               {
                                   IsOrderSuccess = false;
                               }
                               );

            OrderCommand = new RelayCommand<object>(
                (p) =>
                {
                    return true;
                }, (p) =>
                {
                   try
                    {
                        // create order
                        Model.User_Order order = new Model.User_Order()
                        {
                            UserID = CurrentUser.UserID,
                            OrderDate = DateTime.Now,
                            OrderStatus = "PENDING",
                            TotalPrice = CurrentCart.Sum(x => x.Quantity * x.Product.Sell_price)
                        };
                        var orderID = DataProvider.Ins.DB.User_Order.Add(order);

                        // create order detail
                        foreach (var item in CurrentCart)
                        {
                            Model.OrderDetail orderDetail = new Model.OrderDetail()
                            {
                                OrderID = orderID.OrderID,
                                ProductID = item.ProductID,
                                Quantity = item.Quantity,
                            };
                            DataProvider.Ins.DB.OrderDetails.Add(orderDetail);

                            // decrease quantity of product
                            var product = DataProvider.Ins.DB.Products.Where(x => x.ProductID == item.ProductID).FirstOrDefault();
                            product.Quantity -= item.Quantity;


                            // remove this item from cart
                            DataProvider.Ins.DB.Carts.Remove(item);
                        }
                        DataProvider.Ins.DB.SaveChanges();

                        CurrentCart = new ObservableCollection<Model.Cart>(CurrentUser.Carts);
                        IsOrderSuccess = true;
                        IsShowOrder = false;
                    }
                    catch
                    {
                        Notify.ShowNotify("Error while ordering", 4, Notify.Error);
                    }
                });
            #endregion

        }
    }
}