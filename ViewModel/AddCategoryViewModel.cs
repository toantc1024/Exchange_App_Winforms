﻿using Exchange_App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Proxies;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Exchange_App.ViewModel
{
    public class AddCategoryViewModel : BaseViewModel
    {

        #region Variables

        public string dotNet{
            get
            {
                return "AS";
            }
            set {; }
            }
        private string _categoryName;

        #endregion

        #region Properties
        public string CategoryName
        {
            get => _categoryName;
            set
            {
                _categoryName = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        public ICommand AddCategoryCommand
        {
            get;
            set;
        }

        #endregion

        public AddCategoryViewModel()
        {
            AddCategoryCommand = new RelayCommand<Window>(
              (p) => {
                  return true;
              }, (p) => {

                  try
                  {
                       DataProvider.Ins.DB.PROC_AddCat(CategoryName);
                      MessageBox.Show("Add Category Success");
                  }
                  catch (Exception ex)
                  {
                      MessageBox.Show(ex.Message);
                  }
                  finally
                  {
                      p.Close();
                  }

              });
        }

    }
}