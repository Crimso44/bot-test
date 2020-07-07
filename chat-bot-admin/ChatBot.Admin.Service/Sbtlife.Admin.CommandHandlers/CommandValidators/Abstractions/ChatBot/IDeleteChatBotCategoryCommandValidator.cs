using FluentValidation;
using ChatBot.Admin.CommandHandlers.Commands.ChatBot;

namespace ChatBot.Admin.CommandHandlers.CommandValidators.Abstractions.ChatBot
{
    internal interface IDeleteChatBotCategoryCommandValidator : IValidator<DeleteChatBotCategoryCommand>
    {
    }
}
