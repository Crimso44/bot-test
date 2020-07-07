using System.Collections.Generic;

namespace ChatBot.Admin.ReadStorage.Model
{
    public class CollectionDto<TItem>
    {
        public IEnumerable<TItem> Items { get; set; }

        public long? Count { get; set; }
    }
}
