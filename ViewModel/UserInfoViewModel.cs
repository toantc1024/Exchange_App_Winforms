using Exchange_App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Exchange_App.ViewModel
{
    public class UserInfoViewModel : BaseViewModel
    {
        #region Variables

        private string _isEditEnable = "False";

        private User _currentUser;

        private string _isShowCancel = "Hidden";

        #endregion

        #region Properties
        public User CurrentUser { get => _currentUser; set
            {
                _currentUser=value;
                OnPropertyChanged();
            }
        }
        public string IsEditEnable { get => _isEditEnable; set
            {
                _isEditEnable=value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands
        public ICommand DisableEditCommand { get; set; }
        public ICommand EnableEditCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public string IsShowCancel { get => _isShowCancel; set

            {
                _isShowCancel=value;
                OnPropertyChanged();
            }
        }

        #endregion

        public UserInfoViewModel(User user)
        {
            CurrentUser = user;

            EnableEditCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                IsShowCancel = "Visible";
                IsEditEnable = "True";
            });

            DisableEditCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                IsShowCancel = "Hidden";
                IsEditEnable = "False";
            });

            SaveCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                DataProvider.Ins.DB.SaveChanges();
                MessageBox.Show("Save successfully");
                DisableEditCommand.Execute(null);
            });


        }

    }
}
