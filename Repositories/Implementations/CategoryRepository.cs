using Exchange_App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exchange_App.Repositories.Implementations
{
    public class CategoryRepository : BaseRepository
    {

        public void CreateCategory(Category category)
        {
            DbContext.Categories.Add(category);
            DbContext.SaveChanges();
        }

        public void DeleteCategory(int id)
        {
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
        }   
    }
}
