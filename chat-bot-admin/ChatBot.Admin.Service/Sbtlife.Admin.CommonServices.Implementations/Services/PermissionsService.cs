using ChatBot.Admin.CommonServices.Services.Abstractions;

namespace ChatBot.Admin.CommonServices.Services
{
    class PermissionsService : LazyInitSupport, IPermissionsService
    {
        private readonly IRoleService _roleService;
        private bool? _canReadChatBot;
        private bool? _canEditChatBot;

        public PermissionsService(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public bool CanReadChatBot
        {
            get
            {
                LazyInit(ref _canReadChatBot, () => _roleService.IsAdministrator || _roleService.IsChatBotEditor || _roleService.IsChatBotReport);
                // ReSharper disable once PossibleInvalidOperationException
                return _canReadChatBot.Value;
            }
        }

        public bool CanEditChatBot
        {
            get
            {
                LazyInit(ref _canEditChatBot, () => _roleService.IsAdministrator || _roleService.IsChatBotEditor);
                // ReSharper disable once PossibleInvalidOperationException
                return _canEditChatBot.Value;
            }
        }
    }
}
