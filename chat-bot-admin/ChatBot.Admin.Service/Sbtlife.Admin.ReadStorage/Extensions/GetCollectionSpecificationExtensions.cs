using Platform.Core.Common.Utilities;
using ChatBot.Admin.ReadStorage.Specifications;

namespace ChatBot.Admin.ReadStorage.Extensions
{
    internal static class GetCollectionSpecificationExtensions
    {
        public static void AdjustSkipTake(this GetCollectionSpecification specification)
        {
            if (specification.Skip == null || specification.Skip < 0)
                specification.Skip = 0;

            if (specification.Take == null || specification.Take <= 0)
                specification.Take = 20;
            else if (specification.Take > 100)
                specification.Take = 100;
        }

        public static void AdjustSearch(this GetCollectionSpecification specification)
        {
            specification.Search = specification.Search.Optimize();
        }

        public static bool HasSearch(this GetCollectionSpecification specification)
        {
            return !string.IsNullOrWhiteSpace(specification.Search);
        }

        public static bool HasArchived(this GetCollectionSpecification specification)
        {
            return specification.IsArchived != null;
        }
    }
}
