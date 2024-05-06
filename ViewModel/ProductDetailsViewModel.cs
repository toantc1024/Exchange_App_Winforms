using Exchange_App.CustomUserControls;
using Exchange_App.Model;
using Exchange_App.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        private Product _selectedRecommendProduct;
        private ObservableCollection<Product> _recommendProducts;
        private ObservableCollection<Product> _totalRecommendProducts;
        private BaseViewModel _content;
        private string _isShowContent = "Hidden";
        private string _isShowCheckout = "Hidden";
        private int _recommendCurrentIndex = 0;
        private List<string> _status_des;
        private List<string> _info_des;

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
                return _status_des;
            }
            set
            {
                _status_des = value;
                OnPropertyChanged();
            }

        }

        public List<string> Info_des
        {
            get
            {
                return _info_des;
            }
            set
            {
                _info_des = value;
                OnPropertyChanged();
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

        public List<string> stringToLine(string str)
        {
            return str.Split(';').ToList();
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
                Status_des = stringToLine(value.Status_des);
                Info_des = stringToLine(value.Info_des);

                OnPropertyChanged();
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

        public Product SelectedRecommendProduct
        {
            get => _selectedRecommendProduct;
            set
            {
                _selectedRecommendProduct = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Product> RecommendProducts
        {
            get => _recommendProducts;
            set
            {
                _recommendProducts = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Product> TotalRecommendProducts
        {
            get => _totalRecommendProducts;
            set
            {
                _totalRecommendProducts = value;
                OnPropertyChanged();
            }
        }

        public BaseViewModel Content
        {
            get => _content;
            set
            {
                _content = value;
                OnPropertyChanged();
            }

        }

        public string IsShowContent
        {
            get => _isShowContent;
            set
            {
                _isShowContent = value;
                OnPropertyChanged();
            }
        }

        public string IsShowCheckout
        {
            get => _isShowCheckout;
            set
            {
                _isShowCheckout = value;
                OnPropertyChanged();
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

        public ICommand SelectRecommendProductCommand
        {
            get;
            set;
        }

        public ICommand HideCheckoutRecommendCommand
        {
            get;
            set;
        }

        public ICommand ShowCheckoutRecommendCommand
        {
            get;
            set;
        }

        public ICommand HideRecommendProductDetailCommand
        {
            get;
            set;
        }

        public ICommand ShowRecommendProductDetailCommand
        {
            get;
            set;
        }

        public ICommand PreviousProductCommand
        {
            get;
            set;
        }

        public ICommand NextProductCommand
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
            TotalRecommendProducts = new ObservableCollection<Product>(DataProvider.Ins.DB.Products.Where(p => p.CatID == SelectedProduct.CatID).Take(20));
            RecommendProducts = new ObservableCollection<Product>(DataProvider.Ins.DB.Products.Where(p => p.CatID == SelectedProduct.CatID).Take(4));

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

            HideCheckoutRecommendCommand = new RelayCommand<Product>(
                (p) =>
                {
                    return true;
                },

                (p) =>
                {
                    IsShowCheckout = "Hidden";
                    IsShowContent = "Visible";
                    Content = new ProductDetailsViewModel(SelectedRecommendProduct, CurrentUser, ShowCheckoutRecommendCommand);

                }
            );

            ShowCheckoutRecommendCommand = new RelayCommand<Product>(
              (p) =>
              {
                  if (p != null)
                  {
                      return true;
                  }
                  return false;
              },
              (p) =>
              {
                  IsShowCheckout = "Visible";
                  Content = new CheckoutViewModel(CurrentUser, p, HideCheckoutRecommendCommand);
              }

            );

            HideRecommendProductDetailCommand = new RelayCommand<object>(
              (p) => {
                  return true;
              },
              (p) => {
                  UpdateCarousel();
                  IsShowContent = "Hidden";
                  SelectedProduct = null;
              }
            );

            SelectRecommendProductCommand = new RelayCommand<Product>(
             (p) => {
                 if (p != null)
                 {
                     return true;
                 }
                 return false;
             },
             (p) => {
                 // Increaase product view count
                 if (CurrentUser.UserID != p.ProductID)
                 {
                     // increase view_count
                     p.View_count += 1;
                     DataProvider.Ins.DB.SaveChanges();
                 }

                 SelectedProduct = p;

             }
            );

            PreviousProductCommand = new RelayCommand<object>(
                (p) =>
                {
                    return true;
                },
                (p) => {
                    _recommendCurrentIndex--;
                    if (_recommendCurrentIndex < 0)
                    {
                        _recommendCurrentIndex = TotalRecommendProducts.Count - 1;
                    }
                    UpdateCarousel();
                    OnPropertyChanged("RecommendProducts");
                }
            );

            NextProductCommand = new RelayCommand<object>(
                (p) =>
                {
                    return true;
                },
                (p) => {
                    _recommendCurrentIndex++;
                    if (_recommendCurrentIndex >= TotalRecommendProducts.Count)
                    {
                        _recommendCurrentIndex = 0;
                    }
                    UpdateCarousel();
                    OnPropertyChanged("RecommendProducts");
                }
            );


            #endregion
        }

        private void UpdateCarousel()
        {
            int count = TotalRecommendProducts.Count;
            // Clear items and add items based on current index
            RecommendProducts.Clear();
            // For simplicity, always show 5 items
            // You may need to adjust this logic based on your requirements            
            for (int i = 0; i < 4; i++)
            {
                int index = (_recommendCurrentIndex + i) % count;
                RecommendProducts.Add(TotalRecommendProducts[index]);
            }
        }
    }
}