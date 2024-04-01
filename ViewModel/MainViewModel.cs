//using Exchange_App.Commands;
using Exchange_App.Model;
using Exchange_App.View;

//using Exchange_App.View;
using Exchange_App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Exchange_App.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private BaseViewModel _selectedViewModel;

        public BaseViewModel SelectedViewModel
        {
            get { return _selectedViewModel; }
            set { _selectedViewModel = value; OnPropertyChanged(nameof(SelectedViewModel)); }
        }

    
        public int count = 0;


        public User CurrentUser { get; set; }
        public ICommand UpdateViewCommand { get; set; }
        public ICommand LoadedWindowCommand { get; set; }
        public ICommand SetCount { get; set; }
        public ICommand LogoutCommand { get; set; }
        public MainViewModel(User user)
        {
            CurrentUser = user;
            SelectedViewModel = new HomeViewModel(CurrentUser);

            LogoutCommand = new RelayCommand<object>(o =>
            {
                return true;
            }, o =>
            {
                CurrentUser = null;
                var authWindow = new AuthWindow();
                authWindow.Show();
                Application.Current.MainWindow.Close();

            });

            LoadedWindowCommand = new RelayCommand<Window>(
                (p) => { return true; },
                (p) =>
                {
                }
            );

            SetCount = new RelayCommand<object>(o =>
            {
                return true;
            }, o =>
            {
                count = 25;
                MessageBox.Show(count.ToString());
            });


            UpdateViewCommand = new RelayCommand<object>(o =>
            {
                return true;
            },
                o =>
                {
                    if ((string)o == "Home")
                    {
                        SelectedViewModel = new HomeViewModel(CurrentUser);
                    } else if ((string)o == "ProductManager")
                    {
                        SelectedViewModel = new ProductManagerViewModel(CurrentUser);
                    }
                }

            );
        }
    }
}
