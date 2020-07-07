using System.Collections.Generic;

namespace Sbtlife.Admin.ReadStorage.Common.Model
{
    public class CollectionDto<TItem>
    {
        public IEnumerable<TItem> Items { get; set; }

        public long? Count { get; set; }
    }
}
