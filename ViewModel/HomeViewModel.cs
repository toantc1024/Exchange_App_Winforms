﻿using Exchange_App.Model;
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

        private int _selectedOrderType;

        private Product _selectedProduct;
        private string _isLoading = "Hidden";
        private User _currentUser;
        private List<Product> _products;
        private List<CategoryFilterClass> _categoriesFilter;
        private BaseViewModel _content;
        private string _isShowCheckout = "Hidden";

        private int minimumPrice = 0;
        private int _maximumPrice = 0;
        
        private string _isShowContent = "Hidden";
        #endregion

        #region Commands
        
        public ICommand HideCheckoutCommand
        {
            get;set;
        }

        public ICommand ShowCheckoutCommand
        {
            get;set;
        }
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

        public string LoadingText
        {
            get
            {
                return "Bee are collecting honey for " + CurrentUser.Name.Split(' ')[0];
            }
            set
            {
                ;
            }
        }

        public ICommand SortByDate { get; set; }
        public ICommand SortByOrders { get; set; }


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

        public class CategoryFilterClass
        {
            private Category _category;
            private bool _isChecked;
            public CategoryFilterClass(Category category, bool check)
            {
                _category = category;
                _isChecked = check;
            }

            public Category Category { get => _category; set => _category = value; }
            public bool IsChecked { get => _isChecked; set => _isChecked=value; }
        }
        public List<CategoryFilterClass> CategoriesFilter
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

        public string IsShowCheckout { get => _isShowCheckout; set
            {
                _isShowCheckout=value;
                OnPropertyChanged();
            }

        }

        public string IsLoading { get => _isLoading; set
            {
                _isLoading=value;
                OnPropertyChanged();
            }
        }
        private int _selectedCount = 0;

        public int SelectedCount { get => _selectedCount; set

            {
                _selectedCount=value;
                OnPropertyChanged();
            }
        }


        public ICommand ProductQuery
        {
            get;set;
        }
        public ICommand UpdateSelectCountCommand { get; set; }
        public int MinimumPrice { get => minimumPrice; set {

                minimumPrice = value;
                OnPropertyChanged();
            }
        }
        public int MaximumPrice { get => _maximumPrice; set {
                _maximumPrice=value;
                OnPropertyChanged();
            } }

        public int SelectedOrderType { get => _selectedOrderType; set
            {
                _selectedOrderType=value;
                OrderByPrice(value);
                OnPropertyChanged();
            }
            }


        #endregion
        public void OrderByPrice(int type)
        {
            if(type==0)
            {
                Products = Products.OrderByDescending(x => x.Sell_price).ToList();
            }
            else
            {
                Products = Products.OrderBy(x => x.Sell_price).ToList();

            }
        }


        public HomeViewModel(User currentUser)
        {
            CurrentUser = currentUser;
            CategoriesFilter = new List<CategoryFilterClass>();

            foreach (var item in DataProvider.Ins.DB.Categories)
            {

                var catItem = new CategoryFilterClass(item, false);
                CategoriesFilter.Add(catItem);
            }

            SortByDate = new RelayCommand<object>(
                (p) =>
                {
                    return true;
                }
                ,
                (p) =>
                {
                    // Sort products 
                    if(SelectedOrderType == 0)
                    {
                        Products = Products.OrderByDescending(x => x.UploadedDate).ToList();
                    } else
                    {
                        Products = Products.OrderBy(x => x.UploadedDate).ToList();
                    }
                });

            SortByOrders = new RelayCommand<object>(
           (p) =>
           {
               return true;
           }
           ,
           (p) =>
           {
               // Sort products 
               if (SelectedOrderType == 0)
               {
                   Products = Products.OrderByDescending(x => x.OrderDetails.Count()).ToList();
               }
               else
               {
                   Products = Products.OrderBy(x => x.OrderDetails.Count()).ToList();
               }
               // Sort products 
           });
            GetProductByName(String.Empty);

            ProductQuery = new RelayCommand<object>(

                (p) =>
                {
                    return true;
                },
                (p) =>
                {
                    // Define empty list 
                    List<Product> products = new List<Product>();
                    // loop through CategoriesFiler
                    foreach(var item in CategoriesFilter)
                    {
                        if(item.IsChecked)
                        {
                            var productsByCatID = DataProvider.Ins.DB.GetProductByCategory(item.Category.CatID).Where(x=>x.Sell_price > MinimumPrice && x.Sell_price < MaximumPrice).ToList();
                            // add all item in productsByCatID  to products 
                            products.AddRange(productsByCatID);
                        }
                    }
                    Products = products;
                });

            UpdateSelectCountCommand = new RelayCommand<object>(

                (p) =>
                {
                    return true;
                },
                (p) =>
                {
                    int count = CategoriesFilter.Aggregate(0, (a, b) => { 
                        if(b.IsChecked)
                        {
                            return a + 1;
                        }
                        return a;
                    });
                    SelectedCount = count;
                });

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
                
            ShowCheckoutCommand = new RelayCommand<Product>(
                
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
                    Content = new CheckoutViewModel(currentUser, p, HideCheckoutCommand);
                }
                                  
                       
                       
                       );

            HideCheckoutCommand = new RelayCommand<object>(
                (p) =>
                {
                    return true;
                },

                (p) =>
                {
                    IsShowCheckout = "Hidden";
                    IsShowContent = "Visible";
                    Content = new ProductDetailsViewModel(SelectedProduct, CurrentUser, ShowCheckoutCommand);

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
                  // Increaase product view count
                  if(CurrentUser.UserID != p.ProductID)
                  {
                      // increase view_coutn
                      p.View_count += 1;
                      DataProvider.Ins.DB.SaveChanges();
                  }

                  SelectedProduct = p;
                  IsShowContent = "Visible";
                  Content = new ProductDetailsViewModel(p, CurrentUser, ShowCheckoutCommand);
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

            public  void GetProductByName(string keyword)
        {
            Products = DataProvider.Ins.DB.FindProductByKeyWord(keyword).ToList();
        }


    }
}