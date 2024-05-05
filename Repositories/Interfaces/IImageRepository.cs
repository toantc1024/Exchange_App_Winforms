using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exchange_App.Repositories.Interfaces
{
    public interface IImageRepository 
    {
        void AddImage(Model.Image image);
        void DeleteImage(int ImageId);
        
        void DeleteImagesByProductId(int ProductId);

        Model.Image GetImageById(int ImageId);
        List<Model.Image> GetImagesByProductId(int ProductId);

    }
}
