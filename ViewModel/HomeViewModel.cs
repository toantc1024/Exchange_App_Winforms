using Exchange_App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exchange_App.ViewModel
{
    public class HomeViewModel: BaseViewModel
    {
        #region Variables
        private User _currentUser;


        #endregion


        #region Commands

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

        #endregion

        public HomeViewModel(User currentUser)
        {

            CurrentUser=currentUser;
        }
    }
}
