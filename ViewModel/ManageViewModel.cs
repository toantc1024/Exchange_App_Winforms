using Exchange_App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exchange_App.ViewModel
{
    public class ManageViewModel : BaseViewModel
    {
        private User _currentUser;


        private ProductManagerViewModel _productManagerViewModel;

        public User CurrentUser { get => _currentUser; set {
                _currentUser=value;
                OnPropertyChanged();
            }
        }

        public ProductManagerViewModel ProductManagerContent
        { get => _productManagerViewModel; set {
                _productManagerViewModel=value;
                OnPropertyChanged();
            } }

        public ManageViewModel(User user)
        {
            CurrentUser = user;
            ProductManagerContent = new ProductManagerViewModel(CurrentUser);
        }
    }
}
