using System;
using Um.Connect.Abstractions;

namespace ChatBot.Admin.WebApi.UserManagementMock
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
}
