using ChatBot.Admin.Common.Model;

namespace ChatBot.Admin.Common.Helpers
{
    public static class CommonErrorHelper
    {
        public static CommonError CreateError(string message)
        {
            return new CommonError
            {
                Severity = "Error",
                Message = message
            };
        }

        public static CommonError CreateWarning(string message)
        {
            return new CommonError
            {
                Severity = "Warning",
                Message = message
            };
        }
    }
}
