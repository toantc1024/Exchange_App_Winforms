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

namespace Exchange_App.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        public bool IsLogin { get; set; }
        private User _currentUser;


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

        public ICommand LoginCommand { get; set; }
        public ICommand PasswordBoxChanged { get; set; }
        public User CurrentUser { get => _currentUser; set => _currentUser=value; }

        public LoginViewModel()
        {
            PasswordBoxChanged = new RelayCommand<PasswordBox>(
                (p) =>
                {
                    return true;
                },
                (p) =>
                {
                    Password = p.Password;
                }
            );

            LoginCommand = new RelayCommand<Window>((p) => {
                if (p != null)
                {
                    return true;
                }
                return false;
            }, (p) => { Login(); });
        }

        public void Login()
        {

            string passEncode = PasswordEncryption.MD5Hash(PasswordEncryption.Base64Encode(Password));


            var user = DataProvider.Ins.DB.Users.Where(x => x.Username == Username && x.Password == passEncode).SingleOrDefault();

            if (user != null)
            {
                CurrentUser = user;
                IsLogin = true;
                MainWindow mainWindow = new MainWindow(user);
                mainWindow.Show();
                App.Current.MainWindow = mainWindow;
                App.Current.Windows[0].Close();

            }
            else
            {
                IsLogin = false;
                MessageBox.Show("Wrong Username or Password");
            }
        }
    }
}
