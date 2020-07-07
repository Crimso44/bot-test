using System;

namespace ChatBot.Admin.CommandHandlers.Model.Abstractions
{
    public abstract class CommandBase : ICommand
    {
        private Guid _id;

        public void SetId(Guid id) => _id = id;
        public Guid GetId() => _id;
        public string ErrorMessage;
    }
}
