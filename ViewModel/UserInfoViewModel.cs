using Exchange_App.Model;
using Exchange_App.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Media3D;

namespace Exchange_App.ViewModel
{
    public class UserInfoViewModel : BaseViewModel
    {
        #region Variables

        private string _isEditEnable = "False";
        private string _password;
        private string _confirmPassword;
        private List<WishItem> _wishItems;
        private WishItem _selectedWishItem;
        private BaseViewModel _content;
        private string _isShowContent = "Hidden";

        private User _currentUser;

        #endregion

        #region Properties
        public User CurrentUser
        {
            get => _currentUser; set
            {
                _currentUser = value;
                OnPropertyChanged();
            }
        }
        public string IsEditEnable
        {
            get => _isEditEnable; set
            {
                _isEditEnable = value;
                OnPropertyChanged();
            }
        }
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }
        public string ConfirmPassword
        {
            get
            {
                return _confirmPassword;
            }
            set
            {
                _confirmPassword = value;
                OnPropertyChanged();
            }
        }
        public List<WishItem> WishItems
        {
            get
            {
                return _wishItems;
            }
            set
            {
                _wishItems = value;
                OnPropertyChanged();
            }
        }
        public BaseViewModel Content
        {
            get
            {
                return _content;
            }
            set
            {
                _content = value;
                OnPropertyChanged();
            }
        }
        public string IsShowContent
        {
            get
            {
                return _isShowContent;
            }
            set
            {
                _isShowContent = value;
                OnPropertyChanged();
            }
        }
        public WishItem SelectedWishItem
        {
            get
            {
                return _selectedWishItem;
            }
            set
            {
                _selectedWishItem = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Commands
        public ICommand DisableEditCommand { get; set; }
        public ICommand EnableEditCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand PasswordBoxChanged { get; set; }
        public ICommand ConfirmPasswordBoxChanged { get; set; }
        public ICommand SelectWishItemCommand { get; set; }
        public ICommand ShowWishItemDetailCommand { get; set; }

        #endregion

        public UserInfoViewModel(User user)
        {
            WishItems = DataProvider.Ins.DB.WishItems.ToList();
            CurrentUser = user;

            EnableEditCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                IsEditEnable = "True";
            });

            DisableEditCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                IsEditEnable = "False";
            });
                         bool ValidatePassword(string password)
            {
                if (password == null || password.Length < 6)
                {
                    return false;
                }

                const string specialChars = "!@#$%^&*()-_=+[{]};':\",<.>/?|\\";
                bool hasSpecialChar = false;

                foreach (char c in password)
                {
                    if (specialChars.IndexOf(c) >= 0)
                    {
                        hasSpecialChar = true;
                        break;
                    }
                }

                return hasSpecialChar;
            }
            SaveCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                try
                {
                    if(ValidatePassword(Password))
                    {
                        throw new Exception("Password at least 6 characters and 1 special chars");
                    }
                    if (Password != ConfirmPassword)
                    {
                        throw new Exception("Password and Confirm Password must be the same!");
                    };
                    if (Password != null)
                    {
                        CurrentUser.Password = PasswordEncryption.MD5Hash(PasswordEncryption.Base64Encode(Password));
                    }
                    DataProvider.Ins.DB.PROC_UpdateUserInformation(CurrentUser.UserID, CurrentUser.Name, CurrentUser.Username, CurrentUser.Password, CurrentUser.Phone, CurrentUser.Address, CurrentUser.Birthdate);
                    MessageBox.Show("Save successfully");
                    DisableEditCommand.Execute(null);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });

            PasswordBoxChanged = new RelayCommand<PasswordBox>(
              (p) =>
              {
                  return true;
              },
              (p) =>
              {
                  Password = p.Password;
              });

            ConfirmPasswordBoxChanged = new RelayCommand<PasswordBox>(

              (p) =>
              {
                  return true;
              },
              (p) =>
              {
                  ConfirmPassword = p.Password;
              });

            SelectWishItemCommand = new RelayCommand<WishItem>(
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
                 SelectedWishItem = p;
                 IsShowContent = "Visible";
                 //Content = new ProductkDetailsViewModel(p, CurrentUser);
             }
           );
        }

    }
}
