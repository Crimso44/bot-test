using System;
using ChatBot.Admin.CommonServices.Services.Abstractions;

namespace ChatBot.Admin.CommonServices.Services
{
    internal class DateTimeService : IDateTimeService
    {
        private DateTime _utcNow;

        public DateTimeService()
        {
            _utcNow = DateTime.UtcNow;
        }

        public DateTime SessionUtcNow => _utcNow;

        public DateTime SessionUtcToday => _utcNow.Date;

        public void SetSessionUtcNow(DateTime utcNow) => _utcNow = utcNow;
    }
}
