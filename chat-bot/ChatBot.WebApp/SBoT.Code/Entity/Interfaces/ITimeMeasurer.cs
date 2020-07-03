using System;
using System.Collections.Generic;
using System.Text;

namespace SBoT.Code.Entity.Interfaces
{
    public interface ITimeMeasurer
    {
        void SetTimer(string name);
        TimeSpan? AddTimer(string name, TimeSpan time);
        TimeSpan? GetTimer(string name);
    }
}
