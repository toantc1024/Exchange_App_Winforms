using Exchange_App.Model;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Exchange_App.ViewModel
{
    public class ReviewViewModel : BaseViewModel
    {
        private Model.User _currentUser;
        private Model.Product _currentProduct;
        private string _reviewContent;
        private double _sellerRating;
        private double _qualityRating;

        public Model.User CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                OnPropertyChanged();
            }
        }

        public Model.Product CurrentProduct
        {
            get => _currentProduct;
            set
            {
                _currentProduct = value;
                OnPropertyChanged();
            }
        }


        public string ReviewContent
        {
            get => _reviewContent;
            set
            {
                _reviewContent = value;
                OnPropertyChanged();
            }
        }


        public double SellerRating
        {
            get => _sellerRating;
            set
            {
                _sellerRating = value;
                OnPropertyChanged();
            }
        }



        public double QualityRating
        {
            get => _qualityRating;
            set
            {
                _qualityRating = value;
                OnPropertyChanged();
            }
        }

        public ICommand UpdateSellerRating { get; set; }

        public ICommand SubmitReviewCommand { get; set; }

        public ReviewViewModel(Model.User user, Model.Product product)
        {
            CurrentUser = user;
            CurrentProduct = product;

            UpdateSellerRating = new RelayCommand<RatingBar>(
                (p) =>
                {

                    return true;  },
                (p) =>
                {
                    SellerRating =  p.Value;
                }
            );

            SubmitReviewCommand = new RelayCommand<Window>(
                (p) =>
                {
                    return true;
                }, (p) =>
                {

                    Model.Review review = new Model.Review()
                    {
                        UserID = CurrentUser.UserID,
                        Comment = ReviewContent,
                        Rating = SellerRating,
                        TargetUserID = CurrentProduct.UserID,
                        Quality = QualityRating,
                        ReviewDate = DateTime.Now
                    };

                    DataProvider.Ins.DB.Reviews.Add(review);
                    DataProvider.Ins.DB.SaveChanges();

                    p.Close();

                });
        }

    }
}
