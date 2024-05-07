using Exchange_App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exchange_App.ViewModel
{
    internal class DashboardViewModel : BaseViewModel
    {
        private User _currentUser;
        public User CurrentUser
        {
            get { return _currentUser; }
            set { _currentUser = value; OnPropertyChanged(nameof(CurrentUser)); }
        }

        private ProductManagerViewModel _productManagerContent;

        public ProductManagerViewModel ProductManagerContent
        {
            get { return _productManagerContent; }
            set { _productManagerContent = value; OnPropertyChanged(nameof(ProductManagerContent)); }
        }
        public DashboardViewModel(User currentUser)
        {

            CurrentUser=currentUser;
            ProductManagerContent = new ProductManagerViewModel(CurrentUser);
        }
    }
}
