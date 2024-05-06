using Exchange_App.Model;
using Exchange_App.Repositories.Implementations;
using Exchange_App.View;
using MaterialDesignThemes.Wpf;
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
    public class CategoryManagerViewModel : BaseViewModel
    {
        private Boolean _isShowDialog = false;

        public Boolean IsShowDialog { get => _isShowDialog; set {
                _isShowDialog=value; OnPropertyChanged();
            } }


        private List<Category> _categories;

        private Category _selectedCategory;
        private Boolean _isDeleteCategoryVisible = false;
        private Boolean _isAddCategoryVisible = false;
        private Boolean _isEditCategoryVisible = false;

  


        public ICommand ShowAddCategoryCommand { get; set; }
        public ICommand ShowDelCategoryCommand { get; set; }
        public ICommand ShowEditCategoryCommand { get; set; } 

        public CategoryManagerViewModel()
        {
            CategoryRepository categoryRepository = new CategoryRepository();
            Categories = categoryRepository.GetAllCategories();

            ShowDelCategoryCommand = new RelayCommand<Category>((p) =>
            {
                return true;
            }, (p) =>
            {
                SelectedCategory = p;
                IsDeleteCategoryVisible=true;
            });

            ShowAddCategoryCommand = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                IsAddCategoryVisible = true;

            });

            ShowEditCategoryCommand = new RelayCommand<Category>((p) =>
            {
                
                return true;
            }, (p) =>
            {
                SelectedCategory = p;
                IsEditCategoryVisible = true;
            });

            AddCategoryCommand = new RelayCommand<TextBox>((p) =>
            {
                return true;
            }, async (p) =>
            {
                try
                {
                    var catName = p.Text;
                    await Task.Run(() =>
                    {
                        //categoryRepository.CreateCategory(catName);
                        Categories = categoryRepository.GetAllCategories();
                    });
                    p.Text = "";
                    IsAddCategoryVisible=false;
                } catch (Exception ex)
                {
                    MessageBox.Show(ex.InnerException.Message);
                }
            });

            EditCategoryCommand = new RelayCommand<TextBox>((p) =>
            {
                return true;
            },
            async (p) =>
            {
                try {
                    var newCatName = p.Text;
                    await Task.Run(() =>
                    {
                        categoryRepository.UpdateCategory(SelectedCategory.CatID, SelectedCategory.CatName);
                    });

                    Categories = categoryRepository.GetAllCategories();

                    IsEditCategoryVisible = false;
                    SelectedCategory = null;
                } catch (Exception ex)
                {
                    MessageBox.Show(ex.InnerException.Message);
                }
              
            });


            DeleteCategoryCommand = new RelayCommand<object>((p) =>
            {
                return true;
            },

            (p) =>
            {
                try
                {
                    categoryRepository.DeleteCategory(SelectedCategory.CatID);
                    Categories = categoryRepository.GetAllCategories();
                    IsDeleteCategoryVisible = false;
                } catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            });
        }

        public List<Category> Categories { get => _categories; set {
                _categories=value;

                OnPropertyChanged();
            } }

        //Commands

        // Edit Category

        public ICommand AddCategoryCommand { get; set; }

        public ICommand EditCategoryCommand { get; set; }
        public ICommand DeleteCategoryCommand { get; set; }
        public Category SelectedCategory { get => _selectedCategory; set {
                _selectedCategory=value;
                OnPropertyChanged();
            } }
        public bool IsAddCategoryVisible { get => _isAddCategoryVisible; set {
                _isAddCategoryVisible=value; OnPropertyChanged();

            } }

        public bool IsEditCategoryVisible { get => _isEditCategoryVisible; set {
                _isEditCategoryVisible=value;
                OnPropertyChanged();

             } }

        public bool IsDeleteCategoryVisible { get => _isDeleteCategoryVisible; set {
                _isDeleteCategoryVisible=value;
                OnPropertyChanged();
            } }
    }
}
