using System.Collections.Generic;

namespace SBoT.Code.Uavp.Classes
{
    public static class Const
    {
        public static class OrgData
        {
            public static readonly List<string> Keys = new List<string>() { 
                OrgKeys.FIO,
                OrgKeys.Position,
                OrgKeys.City
            };
        }

        public static class OrgKeys
        {
            public const string FIO = "фио";
            public const string Position = "позиция";
            public const string City = "город";
        }
    }
}
