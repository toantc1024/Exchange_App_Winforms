//using Exchange_App.Commands;
using Exchange_App.Model;
using Exchange_App.View;

//using Exchange_App.View;
using Exchange_App.ViewModel;
using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Exchange_App.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private BaseViewModel _selectedViewModel;

        private string _isAdminRole;
        public BaseViewModel SelectedViewModel
        {
            get { return _selectedViewModel; }
            set { _selectedViewModel = value; OnPropertyChanged(nameof(SelectedViewModel)); }
        }

    
        public ICommand ChangeToHomeView { get; set; }
        public int count = 0;


        public User CurrentUser { get; set; }
        public ICommand UpdateViewCommand { get; set; }

        public ICommand LoadedWindowCommand { get; set; }
        public ICommand SetCount { get; set; }
        public ICommand LogoutCommand { get; set; }
        public string IsAdminRole { get => _isAdminRole; set {
                _isAdminRole=value;
                OnPropertyChanged();
            }
        }

        public MainViewModel(User user)
        {
            if (user.RoleID == 2)
            {
                IsAdminRole = "Hidden";
            } else
            {
                IsAdminRole = "Visible";
            }
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


            ChangeToHomeView = new RelayCommand<object>(
                (p) =>
                {
                    return true;
                }, (p) =>
                {
                    SelectedViewModel = new HomeViewModel(CurrentUser);

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
                        SelectedViewModel = new ManageViewModel(CurrentUser);
                    } else if ((string)o == "Order")
                    {
                        SelectedViewModel = new OrderViewModel(CurrentUser);
                    } else if ((string)o == "User")
                    {
                        SelectedViewModel = new UserInfoViewModel(CurrentUser);
                    } else if ((string)o == "Cart")
                    {
                        SelectedViewModel = new CartViewModel(CurrentUser, ChangeToHomeView);
                    }
                }

            );

                    
        }
    }
}
