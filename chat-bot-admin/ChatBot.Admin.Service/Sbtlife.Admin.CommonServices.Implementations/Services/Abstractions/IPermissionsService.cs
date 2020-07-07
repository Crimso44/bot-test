
namespace ChatBot.Admin.CommonServices.Services.Abstractions
{
    public interface IPermissionsService
    {
        bool CanReadChatBot { get; }
        bool CanEditChatBot { get; }
    }
}
