using System;

namespace ChatBot.Admin.Common.Exceptions
{
    public class OptionalException : Exception
    {
        public OptionalException(string message)
            : base(message)
        {
        }
    }
}
