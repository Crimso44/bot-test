
namespace ChatBot.Admin.CommonServices.Services.Abstractions
{
    public interface IImageService
    {
        bool CheckPngImageFormat(byte[] bytes, int width, int height);
    }
}
