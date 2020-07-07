using FluentValidation;
using ChatBot.Admin.CommandHandlers.Commands.ChatBot;

namespace ChatBot.Admin.CommandHandlers.CommandValidators.Abstractions
{
    internal interface ICreateChatBotCategoryCommandValidator : IValidator<CreateChatBotCategoryCommand>
    {
    }
}
