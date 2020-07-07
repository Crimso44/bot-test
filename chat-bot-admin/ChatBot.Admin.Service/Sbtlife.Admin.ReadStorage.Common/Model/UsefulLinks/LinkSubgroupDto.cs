
using System;

namespace Sbtlife.Admin.ReadStorage.Common.Model.UsefulLinks
{
    public class LinkSubgroupDto : DtoBase
    {
        public Guid ParentGroupId { get; set; }
        public string ParentGroupCaption { get; set; }
        public string Caption { get; set; }
        public int? OrdinalPosition { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime ModifiedOnUtc { get; set; }
        public Guid ModifiedBy { get; set; }
        public int? LinkCount { get; set; }
    }
}
