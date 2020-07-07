using System;
using ChatBot.Admin.Common.Helpers;
using ChatBot.Admin.Common.Model;

namespace ChatBot.Admin.Common.Exceptions
{
    public class BusinessLogicException : Exception
    {
        public BusinessLogicException(string message) : base(message)
        {
            Errors = new[] { CommonErrorHelper.CreateError(message) };
        }

        public BusinessLogicException(string message, CommonError[] errors) : base(message)
        {
            Errors = errors;
        }

        public CommonError[] Errors { get; }
    }
}
