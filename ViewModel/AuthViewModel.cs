using Exchange_App.Model;
using Exchange_App.Tools;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Exchange_App.ViewModel
{
    public class AuthViewModel : BaseViewModel
    {
        #region Properties
        public bool IsLogin { get; set; }
        private User _currentUser;


        private Boolean _isLoading = false;

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
        public Boolean IsLoading
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
                   IsLoading = true;
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

        // create async method to login

        async Task LoginAsync()
        {
            try
            {
                if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
                {
                    throw new Exception("Username or Password is empty");
                }

                string passEncode = await Task.Run(() => PasswordEncryption.MD5Hash(PasswordEncryption.Base64Encode(Password)));
                // thay connstring ->> 


                User user = await DataProvider.Ins.DB.Users.Where(x => x.Username == Username && x.Password == passEncode).FirstOrDefaultAsync();
                if (user != null)
                {
                    Password = "";
                    Username = "";
                    CurrentUser = user;
                    MainWindow mainWindow = new MainWindow(user);
                    mainWindow.Show();
                    IsLoading = false;
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
                IsLoading = false;
                MessageBox.Show(ex.Message);
            }
            finally
            {
            }
        }

            public async void Login()
            {
                // convert code to async
                await LoginAsync();
            }



        #endregion
        public AuthViewModel()
        {
        }

    }
}
