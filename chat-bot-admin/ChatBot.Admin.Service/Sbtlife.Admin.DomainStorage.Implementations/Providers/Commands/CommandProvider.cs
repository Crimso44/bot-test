using System;
using System.Threading.Tasks;
using ChatBot.Admin.DomainStorage.Model.Command;
using ChatBot.Admin.DomainStorage.Providers.Abstractions.Commands;
using Microsoft.EntityFrameworkCore;
using Nest;

namespace ChatBot.Admin.DomainStorage.Providers.Commands
{
    internal class CommandProvider : ICommandProvider
    {
        public CommandProvider() 
        {
        }

        public  bool IsDocumentAlreadyProcessed(Guid documentId)
        {
            return false;
        }

        public  void AddCommand(CommandDto commandDto)
        {
            // не сохраняем команды никуда
        }
    }
}
