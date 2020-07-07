using System;
using System.Collections.Generic;
using Um.Abstractions.Core.Const;
using Um.Connect.Abstractions;
using Um.Abstractions.ChatBot;
using ApplicationConst = Um.Abstractions.ChatBot;

namespace ChatBot.Admin.Worker.Code.UserManagementMock
{
    public class MockRole : IRole
    {
        public MockRole(Guid id, Guid scopeId, Guid applicationId)
        {
            Id = id;
            ScopeId = scopeId;
            ApplicationId = applicationId;
        }

        public Guid Id { get; }
        public Guid ScopeId { get; }
        public Guid ApplicationId { get; }
    }

    public class UserConst
    {
        /// <summary>
        /// Системная учетная запись
        /// </summary>
        public static readonly Guid SystemUserId = new Guid("74003CD7-69B3-4B17-9C76-A59FBFA17D77");
    }

    internal class SystemUser : IUser
    {
        public Guid Id => UserConst.SystemUserId;

        public string Name => "Системная учетная запись";

        public string SigmaLogin => string.Empty;

        public string SigmaEmail => string.Empty;

        public ICollection<IRole> Roles => new List<IRole> { new MockRole(ApplicationConst.RoleConst.ChatBotAdministrator, ScopeConst.SberbankTechnology, ApplicationConst.ApplicationConst.ChatBot) };
    }

}
