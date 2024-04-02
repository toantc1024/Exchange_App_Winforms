using Exchange_App.Model;
using Exchange_App.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace Exchange_App.ViewModel
{
    public class HomeViewModel : BaseViewModel
    {
        #region Variables
        private Product _selectedProduct;
        private User _currentUser;
        private List<Product> _products;
        private List<object> _categoriesFilter;
        private BaseViewModel _content;
        private string _isShowContent = "Hidden";
        #endregion

        #region Commands
        public ICommand HideProductDetailCommand
        {
            get;
            set;
        }
        public ICommand ShowProductDetailCommand
        {
            get;
            set;
        }

        public ICommand SelectProductCommand
        {
            get;
            set;
        }

        public ICommand OnSearchCommand { get; set; }
        #endregion

        #region Properties

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

        public List<Product> Products
        {
            get => _products;
            set
            {
                _products = value;
                OnPropertyChanged();
            }
        }
        public List<object> CategoriesFilter
        {
            get => _categoriesFilter;
            set
            {
                _categoriesFilter = value;
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

        public Product SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                OnPropertyChanged();
            }
        }

        #endregion

        public HomeViewModel(User currentUser)
        {
            CurrentUser = currentUser;
            CategoriesFilter = new List<object>();

            foreach (var item in DataProvider.Ins.DB.Categories)
            {
                CategoriesFilter.Add(new
                {
                    CatName = item.CatName,
                    CatID = item.CatID,
                    IsChecked = false
                });
            }

            Products = DataProvider.Ins.DB.Products.ToList();


            HideProductDetailCommand = new RelayCommand<object>(
              (p) => {
                  return true;
              },
              (p) => {
                  Products = DataProvider.Ins.DB.Products.ToList(); 
                  IsShowContent = "Hidden";
                  SelectedProduct = null;
                  
              }
            );

            SelectProductCommand = new RelayCommand<Product>(
              (p) => {
                  if (p != null)
                  {
                      return true;
                  }
                  return false;
              },
              (p) => {
                  SelectedProduct = p;
                  IsShowContent = "Visible";
                  Content = new ProductDetailsViewModel(p, CurrentUser);
              }
            );


            OnSearchCommand = new RelayCommand<TextBox>(p =>
            {
                if (p == null)
                {
                    return false;
                }
                return true;
            },
         p =>
         {

             string keyword = p.Text;
             GetProductByName(keyword);
         });

        }

        public void GetProductByName(string name)
        {
            Products = DataProvider.Ins.DB.Products.Where(x => x.ProductName.Contains(name)).ToList();
        }


    }
}