using Exchange_App.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exchange_App.Repositories.Implementations
{
    public class CategoryRepository : BaseRepository, ICategoryRepository
    {

        public void CreateCategory(string name)
        {
            DbContext.PROC_AddCat(name);
        }

        public void DeleteCategory(int id)
        {
            DbContext.PROC_DeleteCat(id);
        }

        public List<Model.Category> GetAllCategories()
        {
            var categories = DbContext.Categories.ToList();
            return categories;
        }

        public Model.Category GetCategoryById(int id)
        {
            throw new NotImplementedException();
        }

        public void UpdateCategory(int id, string name)
        {
            DbContext.PROC_UpdateCat(id, name);
        }   
    }
}
