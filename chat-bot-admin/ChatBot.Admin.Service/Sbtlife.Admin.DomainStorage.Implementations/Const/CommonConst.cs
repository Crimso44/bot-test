
using System;

namespace ChatBot.Admin.DomainStorage.Const
{
    internal static class CommonConst
    {
        public const string DatabaseValuesSeparator = ";#";

        public static class DocumentStorage
        {
            public static readonly Guid TopCatalogId = new Guid("D2EEDD34-2D48-4616-B95E-32222EA82134");
            public static readonly Guid CatalogId = new Guid("374F01B5-E161-404F-B845-2DD8D8317769");

            public static readonly string TopCatalogName = "Портал Sbt-life";
            public static readonly string CatalogName = "Обученная модель для чат-бота";
        }
    }
}
