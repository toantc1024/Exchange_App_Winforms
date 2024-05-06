using Exchange_App.Model;
using Exchange_App.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
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
        private bool[] _modeArray = new bool[] { true, false };
        public bool[] ModeArray
        {
            get { return _modeArray; }
        }
        public int SelectedMode
        {
            get { return Array.IndexOf(_modeArray, true); }
        }

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
                      // Validate password
                      // User at least 6 chars
                      // Must contain at least 1 special
                      if(!ValidatePassword(Password))
                      {
                          throw new Exception("Password at least 6 chars and 1 special char!");

                      }

                      if (Password != ConfirmPassword)
                      {
                          throw new Exception("Password and Confirm Password must be the same!");
                      };

                      var role = 2;
                      if (ModeArray[1])
                      {
                          role = 1;
                      }

                      User user = new User
                      {
                          Name = Fullname,
                          Username = Username,
                          Password = PasswordEncryption.MD5Hash(PasswordEncryption.Base64Encode(Password)),
                          Phone = Phone,
                          Address = "Chưa xác định",
                          Birthdate = BirthDate,
                          RoleID = role,
                          IsActive = true,
                      };

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