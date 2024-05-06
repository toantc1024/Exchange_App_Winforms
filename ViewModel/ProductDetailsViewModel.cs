using Exchange_App.Model;
using Exchange_App.Repositories.Implementations;
using Exchange_App.Tools;
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
        private int _quantity = 1; 
        private  double _discount;

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

        private bool _isAddedToWishList;


        public Model.Product SelectedProduct    
        {
            get
            {
                return _selectedProduct;
            }
            set
            {
                _selectedProduct = value;
                Discount = 100*(value.Original_price - value.Sell_price) / value.Original_price;
                OnPropertyChanged("SelectedProduct");
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
                OnPropertyChanged();
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


        public ICommand UpdateQuantityCommand
        {
            get;set;
        }

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

        public ICommand ShowCheckoutCommandMultiple
        {
            get;
            set;
        }
        public bool IsAddedToWishList { get => _isAddedToWishList; set {
                _isAddedToWishList=value; OnPropertyChanged();
            } }

        public int Quantity { get => _quantity; set {
              if(value <= 0 || value > SelectedProduct.Quantity)
                {
                    MessageBox.Show("Quantity not exceed");
                } else
                {
                    _quantity=value; OnPropertyChanged();
                }
            } }

        public double Discount { get => _discount; set {
                _discount=value;OnPropertyChanged();
            } }

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
            ShowCheckoutCommandMultiple = new RelayCommand<object>(
                               (p) =>
                               {
                                   return true;
                               },
                                              (p) =>
                                              {
                                                  MultipleParams multipleParams = new MultipleParams();
                                                  multipleParams.currentProduct = product;
                                                  multipleParams.quantity = Quantity;
                                                  showCheckoutCommand.Execute(multipleParams);
                                              }
                                                             );
            ShowCheckoutCommand = showCheckoutCommand;
            SelectedProduct = product;
            CurrentUser = user;
            TotalRecommendProducts = new ObservableCollection<Product>(DataProvider.Ins.DB.Products.Where(p => p.CatID == SelectedProduct.CatID).Take(20));
            RecommendProducts = new ObservableCollection<Product>(DataProvider.Ins.DB.Products.Where(p => p.CatID == SelectedProduct.CatID).Take(4));


            IsAddedToWishList = false;
            DataProvider.Ins.DB.WishItems.ToList().ForEach(wishItem =>
            {
                if (wishItem.ProductID == SelectedProduct.ProductID && wishItem.UserID == CurrentUser.UserID)
                {
                    IsAddedToWishList = true;
                }
            });

            UpdateQuantityCommand = new RelayCommand<string>((p) =>
            {
                if (p != null) return true;
                return false;

            },
            (p) =>
            {
                int delta = 0;
                if(p == "plus")
                {
                    delta = 1;
                }
                else if(p == "minus")
                {
                    delta = -1;
                }
                Quantity += delta;
            });

            AddToWishlistCommand = new RelayCommand<object>(
              (p) => {
                  return true;
              },
              (p) => {
                  try
                  {
                      DataProvider.Ins.DB.WishItems.Add(new WishItem() { ProductID = SelectedProduct.ProductID, UserID = CurrentUser.UserID });
                      IsAddedToWishList = true;
                      DataProvider.Ins.DB.SaveChanges();
                      MessageBox.Show("Add to wishlist success");
                  }
                  catch (Exception ex)  
                  {
                                 // get only first line of error message
                      var err = ex.InnerException.Message.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                      MessageBox.Show(err.FirstOrDefault().ToString());
                  }
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
                  //Content = new CheckoutViewModel(CurrentUser, p, HideCheckoutRecommendCommand, );
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