using SBoT.Connect.Abstractions.Interfaces;
using System;

namespace SBoT.Connect.Service
{
    public class Role : IRole
    {
        public Role(Guid id, Guid scopeId, Guid applicationId)
        {
            Id = id;
            ScopeId = scopeId;
            ApplicationId = applicationId;
        }

        public Guid Id { get; set; }
        public Guid ScopeId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}
