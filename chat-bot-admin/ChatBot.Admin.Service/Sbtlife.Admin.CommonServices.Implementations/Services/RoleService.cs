using System.Linq;
using ChatBot.Admin.CommonServices.Services.Abstractions;
using Um.Abstractions.Core.Const;
using Um.Connect.Abstractions;
using SbtlifeConst = Um.Abstractions.SbtLife;
using ChatBotConst = Um.Abstractions.ChatBot;

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
                        r.Id == RoleConst.Administrator
                            && (r.ScopeId == ScopeConst.SberbankTechnology || r.ScopeId == SbtlifeConst.ApplicationConst.SbtLife) ));

                // ReSharper disable once PossibleInvalidOperationException
                return _isAdministrator.Value;
            }
        }

        public bool IsChatBotEditor
        {
            get
            {
                LazyInit(ref _isChatBotEditor, () => _user.Roles.Any(r =>
                    r.Id == ChatBotConst.RoleConst.ChatBotAdministrator));

                // ReSharper disable once PossibleInvalidOperationException
                return _isChatBotEditor.Value;
            }
        }

        public bool IsChatBotReport
        {
            get
            {
                LazyInit(ref _isChatBotReport, () => _user.Roles.Any(r =>
                    r.Id == ChatBotConst.RoleConst.ChatBotReports));

                // ReSharper disable once PossibleInvalidOperationException
                return _isChatBotReport.Value;
            }
        }

    }
}
