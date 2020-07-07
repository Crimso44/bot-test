
namespace ChatBot.Admin.Common.Extensions
{
    public static class StringExtensions
    {
        public static bool Empty(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        public static bool NotEmpty(this string value)
        {
            return !value.Empty();
        }
    }
}
