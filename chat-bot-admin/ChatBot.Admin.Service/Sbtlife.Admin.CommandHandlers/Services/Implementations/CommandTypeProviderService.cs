using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ChatBot.Admin.CommandHandlers.Handlers.Abstractions;
using ChatBot.Admin.CommandHandlers.Model;
using ChatBot.Admin.CommandHandlers.Services.Abstractions;

namespace ChatBot.Admin.CommandHandlers.Services.Implementations
{
    internal class CommandTypeProviderService : ICommandTypeProviderService
    {
        private Dictionary<string, CommandHandlerInfo> _commandsTypes;

        public CommandHandlerInfo GetCommandInfoById(string commandId)
        {
            var commandsTypes = GetCommandTypesCollection();

            if (!commandsTypes.ContainsKey(commandId))
                throw new ArgumentException(nameof(commandId));

            return commandsTypes[commandId];
        }

        private Dictionary<string, CommandHandlerInfo> GetCommandTypesCollection()
        {
            if (_commandsTypes == null)
            {
                _commandsTypes = new Dictionary<string, CommandHandlerInfo>();
                var assembly = typeof(CommandTypeProviderService).GetTypeInfo().Assembly;
                var commandHandlerInterface = typeof(ICommandHandler<>);

                var commandHandlerTypeInfos = assembly.GetTypes()
                    .Select(t => t.GetTypeInfo())
                    .Where(ti => !ti.IsAbstract && ti.ImplementedInterfaces.Any(ii => ii.GetTypeInfo().IsGenericType && ii.GetGenericTypeDefinition() == commandHandlerInterface));

                foreach (var commandHandlerTypeInfo in commandHandlerTypeInfos)
                {
                    var implementedInterfaces = commandHandlerTypeInfo.ImplementedInterfaces.ToArray();

                    var iCmdHandlerInterfaceType = implementedInterfaces.Single(ii => ii.GetTypeInfo().ImplementedInterfaces
                        .Any(iii => iii.GetTypeInfo().IsGenericType && iii.GetGenericTypeDefinition() == commandHandlerInterface));

                    var iCmdHandlerGenericInterfaceType = implementedInterfaces.Single(ii => ii.GetTypeInfo().IsGenericType && ii.GetGenericTypeDefinition() == commandHandlerInterface);
                    var commandType = iCmdHandlerGenericInterfaceType.GenericTypeArguments.Single();

                    _commandsTypes.Add(iCmdHandlerInterfaceType.Name, new CommandHandlerInfo { Interface = iCmdHandlerInterfaceType, Command = commandType });
                }
            }

            return _commandsTypes;
        }

    }
}
