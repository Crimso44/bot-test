
namespace ChatBot.Admin.CommonServices.Services.Abstractions
{
    public interface IRoleService
    {
        bool IsAdministrator { get; }
        bool IsChatBotEditor { get; }
        bool IsChatBotReport { get; }
    }
}
