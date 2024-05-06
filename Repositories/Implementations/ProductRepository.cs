using Exchange_App.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exchange_App.Repositories.Implementations
{
    public class ProductRepository : BaseRepository
    {
        public void AddNewProduct(Model.Product product)
        {
            DbContext.Set<Model.Product>().Add(product);
            DbContext.SaveChanges();
        }


        public List<Model.Product> GetAllProductsByUserId(int UserID) { 
            var products = DbContext.Products.Where(p => p.UserID == UserID).ToList();
            return products;
        }

        public List<Model.Product> GetAllProducts()
        {
            return DbContext.Products.ToList();
        }

        public Model.Product GetProductById(int ProductId)
        {
            var product = DbContext.Products.Where(p=>p.ProductID == ProductId).SingleOrDefault();
            return product;
        }

        public Model.Product GetProductByName(string productName)
        {
            NotImplementedException ex = new NotImplementedException();
            throw ex;
        }

        public List<Model.Product> GetProductsByCategoryID(int CatId)
        {
            var products = new List<Model.Product>();
            return products;
        }
        public List<Model.Product> GetProductsByKeyword(string keyword)
        {
            var products = DbContext.Products.Where(p => p.ProductName.Contains(keyword)).ToList();
            return products;
        }


    }
}
