using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace PortProgramm.ClassFolder
{
    internal class VariableClass
    {
        public static string AdminLogin;
    }
    class ImageClass
    {
        public static BitmapImage ConvertByteArrayToImage(byte[] array)
        {
            if (array != null)
            {
                using (var ms = new MemoryStream(array, 0, array.Length))
                {
                    var image = new BitmapImage();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.StreamSource = ms;
                    image.EndInit();
                    return image;
                }
            }
            return null;
        }
        public static byte[] ConvertImageToByteArray(string fileName)
        {
            Bitmap bitmap = new Bitmap(fileName);
            ImageFormat bmpFormat = bitmap.RawFormat;
            var imageToConvert = Image.FromFile(fileName);
            using (var ms = new MemoryStream())
            {
                imageToConvert.Save(ms, bmpFormat);
                return ms.ToArray();
            }
        }
    }
}
