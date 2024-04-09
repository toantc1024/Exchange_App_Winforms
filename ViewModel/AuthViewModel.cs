using Exchange_App.Model;
using Exchange_App.Tools;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Exchange_App.ViewModel
{
    public class AuthViewModel : BaseViewModel
    {
        #region Properties
        public bool IsLogin { get; set; }
        private User _currentUser;

        private string _isLoading = "Hidden";

        private string _Username;

        public string Username
        {
            get { return _Username; }
            set { _Username = value; OnPropertyChanged(); }
        }

        private string _Password;

        public string Password
        {
            get { return _Password; }
            set { _Password = value; OnPropertyChanged(); }
        }


        public User CurrentUser { get => _currentUser; set => _currentUser=value; }
        public string IsLoading
        {
            get => _isLoading; set
            {
                _isLoading=value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        private readonly ClickCommand _loginCommand;
        public ClickCommand LoginCommand
        {
            get
            {
                return _loginCommand ?? (new ClickCommand(obj =>
                {
                    IsLoading = "Visible";
                    Login();
                }));
            }
        }

        private readonly ClickCommand _registerOpenComamnd;
        public ClickCommand RegisterOpenComamnd
        {
            get
            {
                return _registerOpenComamnd ?? (new ClickCommand(obj =>
                {
                    RegisterWindow registerWindow = new RegisterWindow();
                    registerWindow.Show();
                    (obj as Window).Close();
                }));
            }
        }


        private readonly ClickCommand _passwordBoxChanged;
        public ClickCommand PasswordBoxChanged
        {
            get
            {
                return _passwordBoxChanged ?? (new ClickCommand(obj =>
                {
                    PasswordBox p = obj as PasswordBox;
                    Password = p.Password;
                }));
            }
        }


        #endregion

        #region Methods

        public void Login()
        {
            try
            {
                IsLoading = "Visible";
                if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
                {
                    throw new Exception("Username or Password is empty");
                }

                string passEncode = PasswordEncryption.MD5Hash(PasswordEncryption.Base64Encode(Password));
                User user = DataProvider.Ins.DB.LoginAccount(Username, passEncode).SingleOrDefault();

                if (user != null)
                {
                    // clear password
                    Password = "";
                    // clear username
                    Username = "";
                    CurrentUser = user;
                    IsLogin = true;
                    MainWindow mainWindow = new MainWindow(user);
                    mainWindow.Show();
                    App.Current.MainWindow = mainWindow;
                    App.Current.Windows[0].Close();
                }
                else
                {
                    throw new Exception("Username or Password is incorrect");
                }
            }
            catch (Exception ex)
            {
                IsLogin = false;
                IsLoading = "Hidden";
                MessageBox.Show(ex.Message);
            }
            finally
            {

                IsLoading = "Hidden";

            }




        }


        #endregion
        public AuthViewModel()
        {
        }

    }
}
