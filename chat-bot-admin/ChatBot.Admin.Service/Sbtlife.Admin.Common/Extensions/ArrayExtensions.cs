
namespace ChatBot.Admin.Common.Extensions
{
    public static class ArrayExtensions
    {
        public static T[] ToValueArray<T>(this T value)
        {
            return new[] { value };
        }

        public static bool NotEmpty<T>(this T[] value)
        {
            return value != null && value.Length > 0;
        }
    }
}
