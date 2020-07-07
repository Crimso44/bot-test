using System;

namespace ChatBot.Admin.Common.Extensions
{
    public static class OptionalHelper
    {
        public static void SafeUpdate<T>(Action<T> update, Optional.Optional<T> optionalSource)
        {
            if (!optionalSource.HasValue())
                return;

            update(optionalSource.Value);
        }

        public static void SafeUpdateOpt<T>(Action<Optional.Optional<T>> update, Optional.Optional<T> optionalSource)
        {
            if (!optionalSource.HasValue())
                return;

            update(new Optional.Optional<T>(optionalSource.Value));
        }
    }
}
