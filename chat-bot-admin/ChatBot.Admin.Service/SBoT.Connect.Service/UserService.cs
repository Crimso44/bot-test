using Microsoft.AspNetCore.Http;
using SBoT.Connect.Abstractions.Interfaces;
using System;
using System.Collections.Generic;
using RoleConst = SBoT.Connect.Abstractions.RoleConst;

namespace SBoT.Connect.Service
{
    public class UserService : IUser
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string SigmaLogin { get; set; }
        public string SigmaEmail { get; set; }
        public ICollection<IRole> Roles { get; set; }

        public UserService(IHttpContextAccessor context)
        {
            Name = context.HttpContext.User?.Identity?.Name ?? "Unknown user";
            SigmaLogin = Name;
            Roles = new List<IRole>() { new Role(RoleConst.ChatBotAdministrator, Guid.Empty, Guid.Empty) };
        }
    }
}
