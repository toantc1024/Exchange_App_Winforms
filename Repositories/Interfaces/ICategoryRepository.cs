using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exchange_App.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        void CreateCategory(string name);
        void DeleteCategory(int id);
        void UpdateCategory(int id, string name);

        List<Model.Category> GetAllCategories();

        Model.Category GetCategoryById(int id);
    }
}
