using System;
using System.Collections.Generic;

namespace SBoT.Connect.Abstractions.Interfaces
{
    public interface IUser
    {
        Guid Id { get; set; }

        string Name { get; set; }

        string SigmaLogin { get; set; }

        string SigmaEmail { get; set; }


        ICollection<IRole> Roles { get; set; }
    }

    public interface IRole
    {
        Guid Id { get; set; }

        Guid ScopeId { get; set; }

        Guid ApplicationId { get; set; }
    }
}
