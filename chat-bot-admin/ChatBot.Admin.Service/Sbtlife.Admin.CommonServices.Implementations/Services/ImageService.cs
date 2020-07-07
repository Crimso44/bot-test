using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using ChatBot.Admin.CommonServices.Services.Abstractions;

namespace ChatBot.Admin.CommonServices.Services
{
    class ImageService : IImageService
    {
        public bool CheckPngImageFormat(byte[] bytes, int width, int height)
        {
            using (var stm = new MemoryStream(bytes, false))
            {
                using (var img = Image.FromStream(stm, true, true))
                {
                    return img.Width == width
                           && img.Height == height
                           && ImageFormat.Png.Equals(img.RawFormat);
                }
            }
        }
    }
}
