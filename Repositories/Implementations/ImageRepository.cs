using Exchange_App.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exchange_App.Repositories.Implementations
{
    internal class ImageRepository: BaseRepository, IImageRepository
    {


        public void AddImage(Model.Image image)
        {
            DbContext.PROC_AddImageByProductID(image.ImageURL, image.ProductID);
            DbContext.SaveChanges();
        }

        public void DeleteImage(int ImageId)
        {
            DbContext.Images.Remove(DbContext.Images.Find(ImageId));
            DbContext.SaveChanges();
        }

        public Model.Image GetImageById(int ImageId)
        {
            var image = DbContext.Images.Find(ImageId);
            return image;
        }

        public List<Model.Image> GetImagesByProductId(int ProductId)
        {
            List<Model.Image> images = DbContext.FUNC_GetImagesByProductID(ProductId).ToList();
            return images;  
        }

        public void DeleteImagesByProductId(int ProductId)
        {
            DbContext.PROC_RemoveImagesByProductID(ProductId);
        }

    }
}
