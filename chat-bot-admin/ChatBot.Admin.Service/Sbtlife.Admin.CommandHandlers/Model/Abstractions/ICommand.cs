using System;

namespace ChatBot.Admin.CommandHandlers.Model.Abstractions
{
    public interface ICommand
    {
        void SetId(Guid id);
        Guid GetId();
    }
}
