using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exchange_App.Repositories.Interfaces
{
    public interface IProductsRepository
    {
        List<Model.Product> GetAllProducts();
        List<Model.Product> GetProductsByCategoryID(int CatId);
        List<Model.Product> GetProductsByKeyword(string keyword);
        Model.Product GetProductById(int ProductId);
        Model.Product GetProductByName(string ProductName);

        void AddNewProduct(Model.Product product);
    }
}
