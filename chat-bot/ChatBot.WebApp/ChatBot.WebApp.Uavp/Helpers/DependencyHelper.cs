using SBoT.Code.Uavp.Services;
using SBoT.Code.Uavp.Services.Abstractions;
using System;
using System.Collections.Generic;

namespace ChatBot.WebApp.Uavp.Helpers
{
    public static class DependencyHelper
    {
        public static Dictionary<Type, Type> GetDependencies()
        {
            var funcDependencies = SBoT.Code.Uavp.Helpers.DependencyHelper.GetDependencies();

            foreach (var dependency in RegisterDependencies())
            {
                if (!funcDependencies.ContainsKey(dependency.Key))
                {
                    funcDependencies.Add(dependency.Key, dependency.Value);
                }
            }

            return funcDependencies;
        }

        private static Dictionary<Type, Type> RegisterDependencies()
        {
            return new Dictionary<Type, Type>
            {
            };
        }
    }
}
