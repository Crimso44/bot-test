using System;
using System.Collections.Generic;
using System.Text;
using SBoT.Code.Entity.Interfaces;

namespace SBoT.Code.Entity
{
    public class TimeMeasurer : ITimeMeasurer
    {
        private Dictionary<string, TimeSpan> _timers = new Dictionary<string, TimeSpan>();

        public void SetTimer(string name)
        {
            _timers[name] = TimeSpan.Zero;
        }

        public TimeSpan? AddTimer(string name, TimeSpan time)
        {
            if (!_timers.ContainsKey(name)) return null;
            _timers[name] += time;
            return _timers[name];
        }

        public TimeSpan? GetTimer(string name)
        {
            if (!_timers.ContainsKey(name)) return null;
            return _timers[name];
        }
    }
}
