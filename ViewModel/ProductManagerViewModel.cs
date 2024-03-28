using Exchange_App.Model;
using Exchange_App.View;
using Microsoft.Win32;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Exchange_App.ViewModel
{
    internal class ProductManagerViewModel : BaseViewModel
    {
        #region Variables
        private List<Category> _categories;

        private Category _selectedCategory;

        private string _showAddProduct;
        private User _currentUser;

        private string _isShowEditProduct;
        private string _isShowAddProduct;

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
        private ObservableCollection<string> _pathes;


        private List<Product> _products;

        #endregion

        #region Commands

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

        public ICommand ShowAddProductCommand
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

        #endregion

        #region Properties

        #region Products

        public ObservableCollection<string> Pathes
        {
            get
            {
                return _pathes;
            }
            set
            {
                _pathes = value;
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

        public List<Product> Products { get => _products; set { _products=value;  OnPropertyChanged(); } }

        #endregion

        public ProductManagerViewModel(User user)
        {
            #region Initialize
            CurrentUser = user;
            IsShowAddProduct = "Hidden";
            IsShowEditProduct = "Hidden";
            ShowAddProduct = "Hidden";
            Categories = DataProvider.Ins.DB.Categories.ToList();
            #endregion

            ShowAddCategory = new RelayCommand<object>(

              (p) => {
                  return true;
              },
              (p) => {
                  AddCategoryView addCategoryView = new AddCategoryView();
                  addCategoryView.ShowDialog();
                  Categories = DataProvider.Ins.DB.Categories.ToList();
              });

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
                      Pathes = new ObservableCollection<string>();

                      foreach (string file in ofd.FileNames)
                      {
                          Pathes.Add(file);
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

                      // add product to database and get productID
                      Product product = DataProvider.Ins.DB.Products.Add(new Product
                      {
                          ProductName = ProductName,
                          Sell_price = Sell_price,
                          Original_price = Original_price,
                          UploadedDate = DateTime.Now,
                          Info_des = Info_des,
                          Quantity = Quantity,
                          Status_des = Status_des,
                          UserID = CurrentUser.UserID,
                          CatID = SelectedCategory.CatID
                      });

                      // add product 
                      foreach (var path in Pathes)
                      {
                          DataProvider.Ins.DB.Images.Add(new Image
                          {
                              ProductID =
                        product.ProductID,
                              ImageURL = path,
                          });
                      }

                      DataProvider.Ins.DB.SaveChanges();
                      MessageBox.Show("Add product successfully!");

                  } catch (Exception ex) { 
                       MessageBox.Show(ex.Message);
                  }
              }
            );

            ShowAddProductCommand = new RelayCommand<object>(
              (p) => {
                  return true;
              },
              (p) => {
                  ShowView("Add");
              }
            );

            ShowEditProductCommand = new RelayCommand<object>(
              (p) => {
                  return true;
              },
              (p) => {
                  ShowView("Edit");
              }
            );
        }

        #region Methods

        public void GetProducts()
        {
            Products = DataProvider.Ins.DB.Products.ToList();
        }
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

        #endregion
    }
}