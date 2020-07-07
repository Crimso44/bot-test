using System;

namespace ChatBot.Admin.Common.Const
{
    public static class CommandTypeConst
    {
        public static class ChatBotCategory
        {
            public static readonly Guid Create = new Guid("3058E48B-2A6D-4458-A219-322A44FD9C10");
            public static readonly Guid Edit = new Guid("885AD75C-C4ED-4AA0-883D-95C056AF86E1");
            public static readonly Guid Delete = new Guid("AB7E3F4C-BA19-4E4D-82F6-A1B83D2DAC5C");
            public static readonly Guid Publish = new Guid("692DFACD-BC21-4F3E-B6EC-59BCB09658FC");
            public static readonly Guid Unpublish = new Guid("94A3EB48-0D04-4ADB-8BAD-C3A6CFC0B911");
            public static readonly Guid CopyPatternToLearn = new Guid("AA327203-5E0D-459B-B6C1-CCA1592E3323");
            public static readonly Guid StorePattern = new Guid("BE0390E8-8597-4495-9BC4-9549ACBBFD81");
            public static readonly Guid DeletePattern = new Guid("A51736D7-4182-4A34-8B0C-197CEFC09775");
        }

        public static class ChatBotPartition
        {
            public static readonly Guid Create = new Guid("46B57465-4AFF-4E79-ACC6-3234878AAEF0");
            public static readonly Guid Edit = new Guid("4E3C55A6-8154-47AB-B2F4-F555543B7CF8");
            public static readonly Guid Delete = new Guid("11A700BE-4079-4511-BD7C-8D6B0C7DAFE6");
        }

        public static class ChatBotSubpartition
        {
            public static readonly Guid Create = new Guid("C43F4AE0-17E7-4E9B-BA1E-85998ED53565");
            public static readonly Guid Edit = new Guid("FA6DE7E7-304E-4EA6-AF6C-1875224D0181");
            public static readonly Guid Delete = new Guid("16AA7A84-06B9-44A8-865B-A146D9BDE9FD");
        }

        public static class ChatBotSetting
        {
            public static readonly Guid Save = new Guid("F82CED97-3AFB-4D0F-A66E-5C4526EB99CF");
        }

        public static class ChatBotLearning
        {
            public static readonly Guid Store = new Guid("B8229D43-C11B-43E0-A033-8F29650AD867");
            public static readonly Guid StoreReport = new Guid("68BAAF63-3B39-4E7C-985E-FAB675CB374C");
            public static readonly Guid Delete = new Guid("8C3E1B40-197D-4EF2-B82A-7A2936D15876");
        }

    }
}
