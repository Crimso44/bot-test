using System;
using System.Collections.Generic;
using ChatBot.Admin.Common.Optional;

namespace ChatBot.Admin.DomainStorage.Model.Abstractions.ChatBot
{
    public class PartitionOptionalDto : DtoBase
    {
        public Optional<Guid?> ParentId { get; set; }

        public Optional<string> Title { get; set; }
    }
}
