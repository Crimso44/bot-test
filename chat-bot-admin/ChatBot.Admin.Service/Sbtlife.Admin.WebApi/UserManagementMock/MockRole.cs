using SBoT.Connect.Abstractions.Interfaces;
using System;

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

        public Guid Id { get; set;  }
        public Guid ScopeId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}
