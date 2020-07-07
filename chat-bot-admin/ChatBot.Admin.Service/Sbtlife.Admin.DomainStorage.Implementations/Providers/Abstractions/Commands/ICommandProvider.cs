using System;
using System.Threading.Tasks;
using ChatBot.Admin.DomainStorage.Model.Command;

namespace ChatBot.Admin.DomainStorage.Providers.Abstractions.Commands
{
    public interface ICommandProvider
    {
        bool IsDocumentAlreadyProcessed(Guid documentId);
        void AddCommand(CommandDto commandDto);
    }
}
