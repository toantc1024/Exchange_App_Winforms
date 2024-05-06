using Exchange_App.CustomUserControls;
using Exchange_App.Model;
using Exchange_App.Repositories.Implementations;
using Exchange_App.Tools;
using Exchange_App.View;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    public class MultipleParams
    {
        public Product currentProduct;
        public int quantity;
    }
    public class HomeViewModel : BaseViewModel
    {
        #region Variables

        private int _selectedOrderType;

        private Product _selectedProduct;
        private Boolean _isAllChecked;
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
        
        public ICommand SelectAllCategory
        {
            get;set;
        }

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
                if(value.Count == 0)
                {
                    MaximumPrice = 0;
                } else
                {
                    MaximumPrice = (int)value.Max(x => x.Sell_price);
                }
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
        ProductRepository productRepository = new ProductRepository();

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

        public bool IsAllChecked { get => _isAllChecked; set
            {
                _isAllChecked =value;
             
                OnPropertyChanged();
            } }


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

                var catItem = new CategoryFilterClass(item, true);
                CategoriesFilter.Add(catItem);
            }
            IsAllChecked = true;
            SelectedCount = CategoriesFilter.Count;
            // define Aggregates to find maximum price

            HideProductDetailCommand = new RelayCommand<object>(
                               (p) =>
                               {
                                   return true;
                               },
                                              (p) =>
                                              {
                                                  IsShowContent = "Hidden";
                                              }
                                                             );

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
                        Products = Products.OrderByDescending(x => x.UploadedDate ).OrderByDescending(x => x.View_count).ToList();
                    } else
                    {
                        Products = Products.OrderBy(x => x.UploadedDate).OrderByDescending(x => x.View_count).ToList();
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
               //if (SelectedOrderType == 0)
               //{
               //    Products = Products.OrderByDescending(x => x.OrderDetails.Count()).OrderByDescending(x => x.View_count).ToList();
               //}
               //else
               //{
               //    Products = Products.OrderBy(x => x.OrderDetails.Count()).OrderByDescending(x => x.View_count).ToList();
               //}
               // Sort products 
           });
            ProductQuery = new RelayCommand<string>(

                (p) =>
                {
                    return true;
                },
                (p) =>
                {
                    List<Product> products = new List<Product>();
                    // loop through CategoriesFiler
                    foreach(var item in CategoriesFilter)
                    {
                        if(item.IsChecked)
                        {
                            var productsByCatID = item.Category.Products.ToList();

                            products.AddRange(productsByCatID);
                        }
                    }
                    // filter  product  x => x.Sell_price >= MinimumPrice
                    products = products.Where((x) => x.Sell_price >= MinimumPrice && x.Sell_price <= MaximumPrice).ToList();
                    Products = products;
                });

            SelectAllCategory = new RelayCommand<object>(
                (p) =>
                {
                    return true;
                },
                (p) =>
                {
                    List<CategoryFilterClass> categoryFilterClasses = new List<CategoryFilterClass>();
                    foreach (var item in CategoriesFilter)
                    {
                        categoryFilterClasses.Add(new CategoryFilterClass(item.Category, IsAllChecked));
                    }
                    CategoriesFilter = categoryFilterClasses;
                    if(IsAllChecked)
                    {
                        SelectedCount = CategoriesFilter.Count;
                    } else
                    {
                        SelectedCount = 0;
                    }
                }
            );

            UpdateSelectCountCommand = new RelayCommand<string>(

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
                    if(count == CategoriesFilter.Count)
                    {
                        IsAllChecked = true;
                    } else
                    {
                        IsAllChecked = false;
                    }
                    SelectedCount = count;
                });





        ShowCheckoutCommand = new RelayCommand<MultipleParams>(
                
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
                    Content = new CheckoutViewModel(currentUser, p.currentProduct, HideCheckoutCommand, p.quantity);
                }
                       );

            HideCheckoutCommand = new RelayCommand<int>(
                (p) =>
                {
                    return true;
                },

                (p) =>
                {
                    IsShowCheckout = "Hidden";
                    IsShowContent = "Visible";

                    SelectedProduct = DataProvider.Ins.DB.Products.SingleOrDefault(x => x.ProductID == p);
                    Content = new ProductDetailsViewModel(SelectedProduct, CurrentUser, ShowCheckoutCommand, SelectProductCommand);
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
                  if(CurrentUser.UserID != p.UserID)
                  {
                      var result = DataProvider.Ins.DB.Products.SingleOrDefault(b => b.ProductID == p.ProductID);
                      // increase view_coutn
                      result.View_count += 1;
                      DataProvider.Ins.DB.SaveChanges();
                      SelectedProduct =  result;

                  }

                  Products = DataProvider.Ins.DB.Products.ToList();

                  IsShowContent = "Visible";
                  Content = new ProductDetailsViewModel(p, CurrentUser, ShowCheckoutCommand, SelectProductCommand);
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
             Products=  DataProvider.Ins.DB.Products.Where(x => x.ProductName.Contains(keyword)).ToList();
         });

            // run a async task 
            async Task getDataAsync()
            {
                Products  = await Task.Run(() => DataProvider.Ins.DB.Products.ToList());
            }

            async void getData()
            {
                // convert code to async
                await getDataAsync();
            }
            getData();

        }
    }
}