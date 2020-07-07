using FluentValidation;
using ChatBot.Admin.CommandHandlers.Const;

namespace ChatBot.Admin.CommandHandlers.CommandValidators
{
    internal abstract class CommandValidatorBase<TCommand> : AbstractValidator<TCommand>
    {
        protected string RequiredFieldMessage(string fieldName)
        {
            return $"{MessageConst.RequiredFieldNotFilled}: {fieldName}";
        }
    }
}
