using System.Linq;
using ChatBot.Admin.CommonServices.Services.Abstractions;
using SBoT.Connect.Abstractions.Interfaces;
using RoleConst = SBoT.Connect.Abstractions.RoleConst;

namespace ChatBot.Admin.CommonServices.Services
{
    class RoleService : LazyInitSupport, IRoleService
    {
        private readonly IUser _user;
        private bool? _isAdministrator;
        private bool? _isChatBotEditor;
        private bool? _isChatBotReport;

        public RoleService(IUser user)
        {
            _user = user;
        }

        public bool IsAdministrator
        {
            get
            {
                LazyInit(ref _isAdministrator, () => _user.Roles.Any(r =>
                        r.Id == RoleConst.ChatBotAdministrator));

                // ReSharper disable once PossibleInvalidOperationException
                return _isAdministrator.Value;
            }
        }

        public bool IsChatBotEditor
        {
            get
            {
                LazyInit(ref _isChatBotEditor, () => _user.Roles.Any(r =>
                    r.Id == RoleConst.ChatBotAdministrator));

                // ReSharper disable once PossibleInvalidOperationException
                return _isChatBotEditor.Value;
            }
        }

        public bool IsChatBotReport
        {
            get
            {
                LazyInit(ref _isChatBotReport, () => _user.Roles.Any(r =>
                    r.Id == RoleConst.ChatBotReports));

                // ReSharper disable once PossibleInvalidOperationException
                return _isChatBotReport.Value;
            }
        }

    }
}
