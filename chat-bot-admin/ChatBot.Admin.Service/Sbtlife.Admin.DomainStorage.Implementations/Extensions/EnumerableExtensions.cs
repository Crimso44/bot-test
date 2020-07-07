using System.Collections.Generic;
using ChatBot.Admin.DomainStorage.Const;

namespace ChatBot.Admin.DomainStorage.Extensions
{
    internal static class EnumerableExtensions
    {
        public static string ToSeparatedString<T>(this IEnumerable<T> values)
        {
            return string.Join(CommonConst.DatabaseValuesSeparator, values);
        }
    }
}
