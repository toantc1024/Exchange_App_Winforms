using Exchange_App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.Painting.Effects;
using SkiaSharp;
namespace Exchange_App.ViewModel
{
    public class StatisticViewModel :BaseViewModel
    {
        public ISeries[] Series { get; set; }
        private double _totalIncome;

        public ISeries[] BudgetSeries { get; set; }


        private List<Review> _userReceivedReviews;

        public Axis[] XAxes { get; set; }
            
        public Axis[] YAxes { get; set; }
       

        private User _currentUser;

        public User CurrentUser { get => _currentUser; set {
                _currentUser=value;
                OnPropertyChanged();
            } }

        public List<Review> UserReceivedReviews { get => _userReceivedReviews; set {
                _userReceivedReviews=value;
                OnPropertyChanged();
            } }

        public double TotalIncome { get => _totalIncome; set {
                _totalIncome=value;
                OnPropertyChanged();
            } }

        public void Initlize()
        {
            TotalIncome = 0;
            
            UserReceivedReviews = CurrentUser.Reviews.ToList();
            // Get total view by date 
            var products = CurrentUser.Products;

            foreach (var item in products)
            {
                item.OrderDetails.ToList().ForEach(x => TotalIncome += x.Quantity * x.Product.Sell_price);
            }


            List<UserViewProduct> userViewProducts = new List<UserViewProduct>();
            foreach (var item in products)
            {
                userViewProducts.AddRange(item.UserViewProducts);
            }

            // how to distribute the view by date
            // how to distribute the view by user
            Dictionary<string, int> viewByDate = new Dictionary<string, int>();
            foreach(var item in userViewProducts)
            {
                var dateStr  = item.ViewDate.ToString("dd/MM/yyyy");    
                if (viewByDate.ContainsKey(dateStr))
                {
                    viewByDate[dateStr]++;
                }
                else
                {
                    viewByDate.Add(dateStr, 1);
                }
            }

            Series   = new ISeries[] { new ColumnSeries<int> { Values = new[] { 1, 2, 3, 4 } } };

            // convert values of dictionary to array
            var values = viewByDate.Values.ToArray();
            Series   = new ISeries[] { new ColumnSeries<int> { Values = values } };

            // convert key  of dictionary to label
            var labels = viewByDate.Keys.ToArray();

            XAxes   = new Axis[]
            {
                new Axis
                {
                    Name = "Date",
                    TextSize = 10,
                     Labels = labels
                }
            };

            YAxes      = new Axis[]
            {
                new Axis
                {
                    Name = "Views",
                    TextSize = 20,
                }
            };



            // group by date


            // Serires of product sold by category;
            var productsSold = CurrentUser.Products;
           
            var orderDetails = new List<OrderDetail>();
            foreach (var item in productsSold)
            {
                orderDetails.AddRange(item.OrderDetails);
            }

            Dictionary<string, int> productSoldByCategory = new Dictionary<string, int>();
            foreach (var item in orderDetails)
            {
                var category = item.Product.Category.CatName;
                if (productSoldByCategory.ContainsKey(category))
                {
                    productSoldByCategory[category]++;
                }
                else
                {
                    productSoldByCategory.Add(category, 1);
                }
            }


            // convert list to array
            BudgetSeries = new ISeries[productSoldByCategory.Keys.Count];
            int n = productSoldByCategory.Keys.Count;
            for (int i = 0; i < n; i++)
            {
                string key = productSoldByCategory.Keys.ElementAt(i);
                BudgetSeries[i] = new PieSeries<int> { Values = new[] { productSoldByCategory[key]}, Name=key };
            }

        }

        public StatisticViewModel(User user)
        {
            CurrentUser = user;


            // Get total view by date 
            Initlize();

        }
    }
}
