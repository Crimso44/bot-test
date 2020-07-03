using System;
using System.Collections.Generic;
using System.Text;

namespace SBoT.Code.Entity.Interfaces
{
    public interface IRoleChecker
    {
        bool IsAdmin();
        bool IsReports();
        void CheckIsAdmin();
        void CheckIsReports();
        string GetUserName();
    }
}
