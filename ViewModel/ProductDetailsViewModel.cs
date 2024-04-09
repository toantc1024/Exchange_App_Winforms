using Exchange_App.Model;
using Exchange_App.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Exchange_App.ViewModel
{
    internal class ProductDetailsViewModel : BaseViewModel
    {

        #region Variables
        private Model.Product _selectedProduct;
        private Model.User _currentUser;
        private int _currentImageIndex = 0;
        #endregion

        #region Properties

        public string ShowSellPrice
        {
            get
            {
                return _selectedProduct.ShowSellPrice;
            }
        }

        public List<string> Status_des
        {
            get
            {
                // init string array
                // get n of line from product.status
                List<string> status = _selectedProduct.Status_des.Split(';').ToList();
                return status;

            }
        }

        public List<string> Info_des
        {
            get
            {
                List<string> infos = _selectedProduct.Info_des.Split(';').ToList();
                return infos;

            }
        }

        public int Quantity
        {
            get
            {
                return _selectedProduct.Quantity;
            }
            set
            {
                _selectedProduct.Quantity = value;
                OnPropertyChanged();
            }
        }

        public string ShowOriginalPrice
        {
            get
            {
                return _selectedProduct.ShowOriginalPrice;
            }
        }

        public string ProductName
        {
            get
            {
                return _selectedProduct.ProductName;
            }
        }

        public string CurrentImage
        {
            get
            {
                return _selectedProduct.Images.ElementAtOrDefault(CurrentImageIndex).ImageURL;
            }
            set
            {
                ;
                OnPropertyChanged();
            }
        }

        public Model.Product SelectedProduct
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

        public string ShopName
        {
            get => _selectedProduct.User.Name;
        }

        public Model.User CurrentUser
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

        public int CurrentImageIndex
        {
            get
            {
                return _currentImageIndex;
            }

            set
            {
                if (value < 0)
                {
                    _currentImageIndex = SelectedProduct.Images.Count - 1;
                }
                else if (value >= SelectedProduct.Images.Count)
                {
                    _currentImageIndex = 0;
                }
                else
                {
                    _currentImageIndex = value;
                }
            }
        }
        #endregion

        #region Commands

        public ICommand ShowCheckoutCommand
        {
            get;
            set;
        }

        public ICommand NextImageCommand
        {
            get;
            set;
        }

        public ICommand PreviousImageCommand
        {
            get;
            set;
        }

        public ICommand AddToWishlistCommand
        {
            get;
            set;
        }

        public ICommand HideIt
        {
            get;
            set;
        }

        #endregion

        public ProductDetailsViewModel(Model.Product product, Model.User user, ICommand showCheckoutCommand)
        {
            #region Initialize
            ShowCheckoutCommand = showCheckoutCommand;
            SelectedProduct = product;
            CurrentUser = user;

            //BuyCommand = new RelayCommand<object>(
            //  (p) => {
            //      return true;
            //  },
            //  (p) => {
            //      if (SelectedProduct.User.UserID == CurrentUser.UserID)
            //      {
            //          MessageBox.Show("You can't buy your own product");
            //          return;
            //      }

            //      // Check if product is out of stock
            //      if (DataProvider.Ins.DB.Products.Where(x => x.ProductID == SelectedProduct.ProductID).FirstOrDefault().Quantity == 0)
            //      {
            //          MessageBox.Show("This product is out of stock");
            //          return;
            //      }

            //      // Create new window for checkout
            //      // Update Product quantity from database
            //      SelectedProduct = DataProvider.Ins.DB.Products.Where(x => x.ProductID == SelectedProduct.ProductID).FirstOrDefault();

            //      Quantity = SelectedProduct.Quantity;
            //  }
            //);

            AddToWishlistCommand = new RelayCommand<object>(
              (p) => {
                  return true;
              },
              (p) => {
                  if (DataProvider.Ins.DB.WishItems.Where(x => x.ProductID == SelectedProduct.ProductID && x.UserID == CurrentUser.UserID).Count() > 0)
                  {
                      MessageBox.Show("This product is already in your wishlist");
                      return;
                  }

                  DataProvider.Ins.DB.WishItems.Add(new Model.WishItem
                  {
                        ProductID = SelectedProduct.ProductID,
                        UserID = CurrentUser.UserID
                    });

                  DataProvider.Ins.DB.SaveChanges();
                  MessageBox.Show("Add to wishlist successfully");
              }
            );

            NextImageCommand = new RelayCommand<object>(
              (p) => {
                  return true;
              },
              (p) => {
                  CurrentImageIndex++;
                  OnPropertyChanged("CurrentImage");
              }
            );

            PreviousImageCommand = new RelayCommand<object>(
              (p) => {
                  return true;
              },
              (p) => {
                  CurrentImageIndex--;
                  OnPropertyChanged("CurrentImage");
              }
            );

            #endregion
        }
    }
}