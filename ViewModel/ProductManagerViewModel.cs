using Exchange_App.Model;
using Exchange_App.Repositories.Implementations;
using Exchange_App.View;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Input;

namespace Exchange_App.ViewModel
{
    public class ProductManagerViewModel : BaseViewModel
    {
        #region Variables

        private int _selectedImages = 0;

        private Boolean _isShowDeleteModal = false;
        private Boolean _isShowDeleteAllImage = false;

        private string _currentImagePath;
        private int _currentImageIndex;
        private List<Category> _categories;

        private BaseViewModel _currentContentPreview;

        private Category _selectedCategory;

        private string _showAddProduct;
        private User _currentUser;

        private string _isShowEditProduct;
        private string _isShowAddProduct;
        private Boolean _isSelectAllImages;

        private int _productID;
        private string _productName;
        private int _quantity;
        private double _original_price;
        private double _sell_price;
        private DateTime _uploadedDate;
        private int _catID;
        private int _userID;
        private string _status_des;
        private string _info_des;
        private ObservableCollection<ImagePath> _pathes;

        private bool _isShowPreviewModal;

        private List<Product> _products;

        #endregion

        #region Commands

        public ICommand SelectImageUpdateCommand { get; set; }
        public ICommand RemoveAllImageCommand { get; set; }
        public ICommand RemoveImageFromPathCommand
        {
            get; set;
        }
        public ICommand ShowCategoryManagerCommand
        {
            get; set;
        }
        public ICommand SortByAlphabetAscCommand
        {
            get; set;
        }
        public ICommand SortByAlphabetDescCommand
        {
            get; set;
        }
        public ICommand SortProductByPriceCommand
        {
            get; set;
        }

        public ICommand SortProductByDateCommand
        {
            get; set;
        }

        public ICommand SortAlphabetCommand
        {
            get; set;
        }

        public ICommand HideProductCommand
        {
            get;
            set;
        }

        public ICommand UpdateProductCommand
        {
            get;
            set;
        }

        public ICommand DeleteProductCommand
        {
            get;
            set;

        }

        public ICommand ShowAddCategory
        {
            set;
            get;
        }

        public ICommand AddProductCommand
        {
            get;
            set;
        }

        public ICommand ShowDeleteImageCommand { get; set; }

        public ICommand ShowAddProductCommand
        {
            get;
            set;
        }

        public ICommand ShowProductPreview
        {
            get;
            set;
        }

        public ICommand OpenFileDialog
        {
            get;
            set;
        }

        public ICommand ShowEditProductCommand
        {
            get;
            set;
        }

        public ICommand GetProductListCommand
        {
            get;
            set;
        }


        public ICommand ShowDeleteAllImageCommand
        {
            get; set;
        }

        public ICommand SelectAllCommand
        {
            get; set;
        }


        #endregion

        #region Properties

        #region Products

        public class ImagePath
        {
            private string _path;
            private Boolean _isChecked;

            public ImagePath()
            {

            }

            public ImagePath(string path, bool isChecked)
            {
                Path=path;
                IsChecked=isChecked;
            }

            public string Path { get => _path; set => _path = value; }
            public bool IsChecked { get => _isChecked; set => _isChecked=value; }
        }

        public ObservableCollection<ImagePath> Pathes
        {
            get
            {
                return _pathes;
            }
            set
            {

                _pathes = value;
                SelectedImages = value.Aggregate(0, (acc, item) => acc + (item.IsChecked ? 1 : 0));
                OnPropertyChanged();

            }
        }
        public string ProductName
        {
            get
            {
                return _productName;
            }
            set
            {
                _productName = value;
                OnPropertyChanged();
            }
        }

        public int ProductID
        {
            get
            {
                return _productID;
            }
            set
            {
                _productID = value;
                OnPropertyChanged();
            }
        }

        public int Quantity
        {
            get
            {
                return _quantity;
            }

            set
            {
                _quantity = value;
                OnPropertyChanged();
            }
        }

        public double Original_price
        {
            get
            {
                return _original_price;
            }
            set
            {
                _original_price = value;
                OnPropertyChanged();
            }
        }

        public double Sell_price
        {
            get
            {
                return _sell_price;
            }

            set
            {
                _sell_price = value;
                OnPropertyChanged();
            }
        }

        public DateTime UploadedDate
        {
            get
            {
                return _uploadedDate;
            }

            set
            {
                _uploadedDate = value;
                OnPropertyChanged();
            }
        }

        public int CatID
        {
            get
            {
                return _catID;
            }
            set
            {
                _catID = value;
                OnPropertyChanged();
            }
        }


        public int UserID
        {
            get
            {
                return _userID;
            }

            set
            {
                _userID = value;
                OnPropertyChanged();
            }
        }

        public string Status_des
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

        public string Info_des
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

        #endregion

        public string IsShowAddProduct
        {
            get
            {
                return _isShowAddProduct;
            }

            set
            {
                _isShowAddProduct = value;
                OnPropertyChanged();
            }
        }

        public string IsShowEditProduct
        {
            get
            {
                return _isShowEditProduct;
            }
            set
            {
                _isShowEditProduct = value;
                OnPropertyChanged();
            }
        }

        public string ShowAddProduct
        {
            get
            {
                return _showAddProduct;
            }
            set
            {
                _showAddProduct = value;
                OnPropertyChanged();
            }
        }

        public User CurrentUser
        {
            get
            {
                return _currentUser;
            }

            set
            {
                _currentUser = value;
                OnPropertyChanged();
            }
        }

        public List<Category> Categories
        {
            get => _categories;
            set
            {
                _categories = value;
                OnPropertyChanged();
            }
        }
        public Category SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                OnPropertyChanged();
            }
        }

        public List<Product> Products
        {
            get => _products;
            set
            {
                _products = value;
                OnPropertyChanged();
            }
        }

        private List<Product> _allProducts;

        public List<Product> AllProducts
        {
            get => _allProducts;
            set
            {
                _allProducts = value;
                OnPropertyChanged();
            }
        }

        public bool IsShowPreviewModal
        {
            get => _isShowPreviewModal;
            set
            {
                _isShowPreviewModal = value;
                OnPropertyChanged();
            }
        }

        public BaseViewModel CurrentContentPreview
        {
            get => _currentContentPreview;
            set
            {
                _currentContentPreview = value;
                OnPropertyChanged();
            }
        }

        public bool IsShowDeleteModal
        {
            get => _isShowDeleteModal; set
            {

                _isShowDeleteModal=value;
                OnPropertyChanged();
            }
        }

        public string CurrentImagePath
        {
            get => _currentImagePath; set
            {
                _currentImagePath = value;
                OnPropertyChanged();
            }
        }
        public int CurrentImageIndex
        {
            get => _currentImageIndex; set
            {
                _currentImageIndex=value;
                OnPropertyChanged();
            }
        }

        public bool IsShowDeleteAllImage
        {
            get => _isShowDeleteAllImage; set
            {
                _isShowDeleteAllImage=value;
                OnPropertyChanged();
            }
        }

        public int SelectedImages
        {
            get => _selectedImages;
            set
            {
                _selectedImages=value;
                OnPropertyChanged();
            }
        }

        public bool IsSelectAllImages
        {
            get => _isSelectAllImages;
            set
            {
                _isSelectAllImages=value;
                OnPropertyChanged();
            }
        }

        #endregion

        public ProductManagerViewModel(User user)
        {
            #region Initialize
            CurrentUser = user;
            IsShowAddProduct = "Hidden";
            IsShowEditProduct = "Hidden";
            ShowAddProduct = "Hidden";
            GetProductsByUserID(user.UserID);
            Categories = DataProvider.Ins.DB.Categories.ToList();
            #endregion

            SelectAllCommand = new RelayCommand<CheckBox>((p) =>
            {
                return true;
            }, (p) =>
            {
                var newPathes = new ObservableCollection<ImagePath>();
                foreach (var item in Pathes)
                {
                    newPathes.Add(new ImagePath { Path = item.Path, IsChecked = (bool)p.IsChecked });
                }
                Pathes = newPathes;
            });

            ShowCategoryManagerCommand = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                CategoriesManagerView categoriesManagerView = new CategoriesManagerView();
                categoriesManagerView.ShowDialog();
                Categories = DataProvider.Ins.DB.Categories.ToList();
            });

            RemoveAllImageCommand = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                Pathes = new ObservableCollection<ImagePath>();
                IsShowDeleteAllImage = false;
            });

            ShowDeleteAllImageCommand = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                IsShowDeleteAllImage = true;
            });


            RemoveImageFromPathCommand = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                int count = 0;
                var newPathes = new ObservableCollection<ImagePath>();
                foreach (var item in Pathes)
                {
                    if (!item.IsChecked)
                    {
                        newPathes.Add(item);
                    }
                    else
                    {

                        count = count + 1;
                    }
                }
                SelectedImages = count;
                Pathes = newPathes;
                IsShowDeleteAllImage = false;
            });

            SortAlphabetCommand = new RelayCommand<ListBox>((p) =>
            {
                return true;
            }, (p) =>
            {
                int k = p.SelectedIndex;
                // sort product by name
                if (k == 0)
                {
                    SortByAlphabetAscCommand.Execute(null);
                }
                else
                {
                    SortByAlphabetDescCommand.Execute(null);
                }
            });


            SortProductByDateCommand = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                // sort product by date
                Products = Products.OrderByDescending(x => x.UploadedDate).ToList();
            });

            SortByAlphabetDescCommand = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                // sort product by name desc
                Products = Products.OrderByDescending(x => x.ProductName).ToList();
            });

            SortByAlphabetAscCommand = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                // sort product by name asc
                Products = Products.OrderBy(x => x.ProductName).ToList();
            });

            SortProductByPriceCommand = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                // sort product by price
                Products = Products.OrderByDescending(x => x.Sell_price).ToList();
            });



            ShowAddCategory = new RelayCommand<object>(

              (p) => {
                  return true;
              },
              (p) => {
                  AddCategoryView addCategoryView = new AddCategoryView();
                  addCategoryView.ShowDialog();
                  Categories = DataProvider.Ins.DB.Categories.ToList();
              });

            ShowProductPreview = new RelayCommand<object>(
              (p) => {
                  return true;
              },
              (p) => {

                  //IsShowPreviewModal = true;
                  //Product product = new Product
                  //{
                  //    ProductName = ProductName,
                  //    Sell_price = Sell_price,
                  //    Original_price = Original_price,
                  //    UploadedDate = DateTime.Now,
                  //    Info_des = Info_des,
                  //    Quantity = Quantity,
                  //    Status_des = Status_des,
                  //    UserID = CurrentUser.UserID,
                  //    CatID = SelectedCategory.CatID,
                  //    ProductID = ProductID
                  //};
                  //CurrentContentPreview = new ProductDetailsViewModel(product, CurrentUser, HideProductCommand, );

              });

            ShowDeleteImageCommand = new RelayCommand<int>(
                (p) =>
                {
                    return true;
                }, (p) =>
                {
                    CurrentImageIndex = p;
                    CurrentImagePath = Pathes[p].Path;
                    IsShowDeleteModal = true;
                }
                );

            HideProductCommand = new RelayCommand<object>(
              (p) => {
                  return true;
              },
              (p) => {
                  ShowAddProduct = "Hidden";
              }
            );

            OpenFileDialog = new RelayCommand<object>(
              (p) => {
                  return true;
              },
              (p) => {
                  OpenFileDialog ofd = new OpenFileDialog();
                  ofd.Multiselect = true;
                  ofd.Title = "Choose your product photos";
                  ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.apng;*.avif;*.gif;*.jfif;*.pjpeg";
                  ofd.ShowDialog();
                  if (ofd.FileNames.Count() > 0 || ofd.FileName.Count() > 0)
                  {
                      // clear array string[]
                      //Pathes = new ObservableCollection<ImagePath>();

                      foreach (string file in ofd.FileNames)
                      {
                          Pathes.Add(new ImagePath { Path = file, IsChecked = false });
                      }
                  }
              });

            AddProductCommand = new RelayCommand<object>(
              (p) => {
                  return true;
              },
              (p) => {
                  try
                  {
                      var obj = DataProvider.Ins.DB.Products.Add(new Product
                      {
                          ProductName = ProductName,
                          Sell_price = Sell_price,
                          Original_price = Original_price,
                          UploadedDate = DateTime.Now,
                          Info_des = Info_des,
                          Quantity = Quantity,
                          Status_des = Status_des,
                          UserID = CurrentUser.UserID,
                          CatID = SelectedCategory.CatID,
                          View_count = 0
                      });

                      foreach (var path in Pathes)
                      {
                          DataProvider.Ins.DB.Images.Add(new Model.Image { ProductID = obj.ProductID, ImageURL = path.Path });
                      }
                      DataProvider.Ins.DB.SaveChanges();
                      GetProductsByUserID(CurrentUser.UserID);
                      HideProductCommand.Execute(true);
                  }
                  catch (Exception ex)
                  {
                      var err = ex.InnerException.Message.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                      MessageBox.Show(err.FirstOrDefault().ToString());
                  }
              }
            );


            SelectImageUpdateCommand = new RelayCommand<object>(
                (p) =>
                {
                    return true;
                }, (p) =>
                {
                    SelectedImages = Pathes.Aggregate(0, (acc, item) => acc + (item.IsChecked ? 1 : 0));

                    if (SelectedImages == Pathes.Count)
                    {
                        IsSelectAllImages = true;
                    }
                    else
                    {
                        IsSelectAllImages = false;
                    }


                });

            void GetProductsByUserID(int UserID)
            {
                Products = AllProducts = DataProvider.Ins.DB.Products.Where(x => x.UserID == UserID).ToList();
            }

            DeleteProductCommand = new RelayCommand<Product>((p) => {
                return true;
            }, (p) => {
                try
                {
                    MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this product?", "Delete Product", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        // delete product

                        // remove all images by product id
                        DataProvider.Ins.DB.Images.RemoveRange(DataProvider.Ins.DB.Images.Where(x => x.ProductID == p.ProductID));


                        var wishItems = DataProvider.Ins.DB.WishItems.Where(x => x.ProductID == p.ProductID).ToList();
                        if (wishItems.Count > 0)
                        {
                            throw new Exception("Product is in wishlist, cannot delete!");
                        }

                        var User_Orders = DataProvider.Ins.DB.User_Order.Where(x => x.ProductID == p.ProductID).ToList();

                        if (User_Orders.Count > 0)
                        {
                            throw new Exception("Product is in order, cannot delete!");
                        }

                        DataProvider.Ins.DB.Products.Remove(p);
                        DataProvider.Ins.DB.SaveChanges();
                        GetProductsByUserID(CurrentUser.UserID);
                        MessageBox.Show("Delete product successfully!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });

            ShowAddProductCommand = new RelayCommand<object>(
              (p) => {
                  return true;
              },
              (p) => {

                  ResetProperty();
                  ShowView("Add");
              }
            );



            UpdateProductCommand = new RelayCommand<object>(
              (p) => {
                  return true;
              },
              (p) => {
                  try
                  {
                      /// 
                      var product = DataProvider.Ins.DB.Products.Where(x => x.ProductID == ProductID).FirstOrDefault();
                      product.ProductName = ProductName;
                      product.Sell_price = Sell_price;
                      product.Original_price = Original_price;
                      product.Quantity = Quantity;
                      product.Status_des = Status_des;
                      product.Info_des = Info_des;
                      product.CatID = SelectedCategory.CatID;
                      DataProvider.Ins.DB.Images.RemoveRange(DataProvider.Ins.DB.Images.Where(x => x.ProductID == ProductID));
                      foreach (var path in Pathes)
                      {
                          DataProvider.Ins.DB.Images.Add(new Model.Image { ProductID = ProductID, ImageURL = path.Path });
                      }
                      DataProvider.Ins.DB.SaveChanges();
                      GetProductsByUserID(CurrentUser.UserID);
                      MessageBox.Show("Edit successfully!");
                  }
                  catch (Exception ex)
                  {
                      // get only first line of error message
                      var err = ex.InnerException.Message.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                      MessageBox.Show(err.FirstOrDefault().ToString());
                  }

              }
            );


            GetProductListCommand = new RelayCommand<TextBox>((p) => {
                if (p != null)
                {
                    return true;
                }
                return false;
            }, (p) => {
                Products = AllProducts.Where(x => x.ProductName.Contains(p.Text)).ToList();
            });

            ShowEditProductCommand = new RelayCommand<int>(
              (p) => {
                  return true;
              },
              (p) => {

                  Product product = DataProvider.Ins.DB.Products.Where(x => x.ProductID == p).FirstOrDefault();
                  ProductName = product.ProductName;
                  Sell_price = product.Sell_price;
                  Original_price = product.Original_price;

                  var newPathes = new ObservableCollection<ImagePath>();
                  var images = product.Images.ToList();
                  foreach (var image in images)
                  {
                      newPathes.Add(new ImagePath { IsChecked = false, Path = image.ImageURL });
                  }

                  Pathes = newPathes;

                  Quantity = product.Quantity;
                  Status_des = product.Status_des;
                  Info_des = product.Info_des;
                  SelectedCategory = Categories.Where(x => x.CatID == product.CatID).FirstOrDefault();
                  ProductID = product.ProductID;
                  ShowView("Edit");
              }
            );

        }

        #region Methods

        public void ShowView(string view)
        {
            if (view == "Add")
            {
                ShowAddProduct = "Visible";
                IsShowAddProduct = "Visible";
                IsShowEditProduct = "Hidden";
            }
            else if (view == "Edit")
            {
                ShowAddProduct = "Visible";
                IsShowAddProduct = "Hidden";
                IsShowEditProduct = "Visible";
            }
        }

        public void ResetProperty()
        {
            ProductName = "";
            Sell_price = 0;
            Original_price = 0;
            Quantity = 0;
            SelectedCategory = null;
            Status_des = "";
            Info_des = "";
            Pathes = new ObservableCollection<ImagePath>();

        }

        #endregion
    }
}