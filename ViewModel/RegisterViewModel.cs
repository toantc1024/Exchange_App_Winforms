using Exchange_App.Model;
using Exchange_App.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Exchange_App.ViewModel
{
    public class RegisterViewModel : BaseViewModel
    {

        #region Variables

        private string _username;
        private string _password;
        private string _confirmPassword;
        private string _phone;
        private DateTime _birthDate;
        private string _fullname;

        #endregion

        #region Commands
        public ICommand ShowLoginCommand
        {
            get;
            set;
        }

        public ICommand PasswordBoxChanged
        {
            get;
            set;
        }
        public ICommand ConfirmPasswordBoxChanged
        {
            get;
            set;
        }

        public ICommand SignUpCommand
        {
            get;
            set;
        }

        public ICommand BirthDateChanged
        {
            get;
            set;
        }

        #endregion

        #region Properties
        public string Username
        {
            get
            {
                return _username;
            }
            set
            {
                _username = value;
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

        public string Phone
        {
            get
            {
                return _phone;
            }
            set
            {
                _phone = value;
                OnPropertyChanged();
            }
        }

        public string Fullname
        {
            get
            {
                return _fullname;
            }
            set
            {
                _fullname = value;
                OnPropertyChanged();
            }
        }

        public DateTime BirthDate
        {
            get
            {
                return _birthDate;
            }
            set
            {
                _birthDate = value;
                OnPropertyChanged();
            }
        }

        #endregion

        public RegisterViewModel()
        {
            ShowLoginCommand = new RelayCommand<Window>(
              (p) => {
                  return true;
              },
              (p) => {
                  AuthWindow aw = new AuthWindow();
                  aw.Show();
                  p.Close();
              });

            PasswordBoxChanged = new RelayCommand<PasswordBox>(
              (p) => {
                  return true;
              },
              (p) => {
                  Password = p.Password;
              });

            ConfirmPasswordBoxChanged = new RelayCommand<PasswordBox>(

              (p) => {
                  return true;
              },
              (p) => {
                  ConfirmPassword = p.Password;
              });

            BirthDateChanged = new RelayCommand<DatePicker>(
              (p) => {
                  return true;
              },
              (p) => {
                  BirthDate = p.SelectedDate.Value;
              });

            SignUpCommand = new RelayCommand<Window>(

              (p) => {
                  if(p != null)
                  {
                      return true;
                  }
                  return false;
              }, (p) => {
                  try
                  {
                      if (Password != ConfirmPassword)
                      {
                          throw new Exception("Password and Confirm Password must be the same!");
                      };

                      User user = new User
                      {
                          Name = Fullname,
                          Username = Username,
                          Password = PasswordEncryption.MD5Hash(PasswordEncryption.Base64Encode(Password)),
                          Phone = Phone,
                          Address = "",
                          Birthdate = BirthDate,
                          RoleID = 2,
                          IsActive = true,
                      };

                      // Write validation function later!

                      user.Validate();

                      DataProvider.Ins.DB.Users.Add(user);

                      DataProvider.Ins.DB.SaveChanges();

                      MessageBox.Show("Register successfully! Please login to continue.");

                      AuthWindow aw = new AuthWindow();
                      aw.Show();
                      p.Close();

                  }
                  catch (Exception ex)
                  {
                      MessageBox.Show(ex.Message);
                  }

              });
        }
    }
}