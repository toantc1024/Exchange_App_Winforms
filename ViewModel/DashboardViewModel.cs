using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exchange_App.ViewModel
{
    public class DashboardViewModel : BaseViewModel
    {
        private Model.User _currentUser;
        private ProductManagerViewModel _productManagerContent;
        public DashboardViewModel(Model.User user)
        {
            CurrentUser = user;
            ProductManagerContent = new ProductManagerViewModel(CurrentUser);
        }

        public Model.User CurrentUser { get => _currentUser; set {
                _currentUser=value;
                OnPropertyChanged();
            } }

        public ProductManagerViewModel ProductManagerContent
        { get => _productManagerContent; set {

                _productManagerContent=value;
                OnPropertyChanged();
            }
        }
    }
}
