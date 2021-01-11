using SBoT.Connect.Abstractions.Interfaces;
using System;
using System.Collections.Generic;

namespace SBoT.Connect.Abstractions.Dto
{
    public class UserDto : IUser
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string SigmaLogin { get; set; }

        public string SigmaEmail { get; set; }


        public ICollection<IRole> Roles { get; set; }
    }

    public class UserDtoSerializable
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string SigmaLogin { get; set; }

        public string SigmaEmail { get; set; }


        public List<RoleDto> Roles { get; set; }
    }

    public class RoleDto : IRole
    {
        public Guid Id { get; set; }

        public Guid ScopeId { get; set; }

        public Guid ApplicationId { get; set; }
    }

}
