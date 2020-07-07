using System;

namespace ChatBot.Admin.CommonServices.Services.Abstractions
{
    public interface IDateTimeService
    {
        DateTime SessionUtcNow { get; }
        DateTime SessionUtcToday { get; }

        void SetSessionUtcNow(DateTime utcNow);
    }
}
