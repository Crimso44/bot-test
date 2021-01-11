using SBoT.Code.Uavp.Services;
using SBoT.Code.Uavp.Services.Abstractions;
using System;
using System.Collections.Generic;

namespace SBoT.Code.Uavp.Helpers
{
    public static class DependencyHelper
    {
        public static Dictionary<Type, Type> GetDependencies()
        {
            return new Dictionary<Type, Type>
            {
                { typeof(IRosterService), typeof(RosterService)},
                { typeof(IUserInfoService), typeof(UserInfoService)},
            };
        }

    }
}

