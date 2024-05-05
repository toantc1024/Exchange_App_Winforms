using Exchange_App.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Exchange_App.Tools
{
    internal class ImageConverter
    {
        public static BitmapImage ConvertByteArrayToBitmapImage(Byte[] bytes)
        {
            var stream = new MemoryStream(bytes);
            stream.Seek(0, SeekOrigin.Begin);
            var image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = stream;
            image.EndInit();
            return image;
        }
    }

    public class ProductPreviewImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // cast value to Product
            // value is virtual ICollection<Image> Images, how to get preview image
            var images = value as ICollection<Image>;
            images = images.ToList();
            if (images == null || images.Count == 0)
            {
                // return placeholder url
                return "https://via.placeholder.com/150";
            }
            return images.FirstOrDefault().ImageURL;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
