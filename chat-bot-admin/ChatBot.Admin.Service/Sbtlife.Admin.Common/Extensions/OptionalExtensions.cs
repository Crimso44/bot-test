using ChatBot.Admin.Common.Optional;

namespace ChatBot.Admin.Common.Extensions
{
    public static class OptionalExtensions
    {
        public static bool HasValue<T>(this Optional<T> opt)
        {
            return opt != null && opt.HasValue;
        }
    }
}
