using System;

namespace Sbtlife.Admin.ReadStorage.Common.Model.UsefulLinks
{
    public class LinkDto : DtoBase
    {
        public Guid? LinkGroupId { get; set; }
        public string LinkGroupCaption { get; set; }
        public int? LinkGroupOrdinalPosition { get; set; }
        public Guid? LinkSubgroupId { get; set; }
        public Guid? LinkSubgroupParentGroupId { get; set; }
        public string LinkSubgroupCaption { get; set; }
        public int? LinkSubgroupOrdinalPosition { get; set; }
        public Guid? TargetSystemId { get; set; }
        public string TargetSystemName { get; set; }
        public string LinkUrl { get; set; }
        public string LinkText { get; set; }
        public string LinkTags { get; set; }
        public string LinkDescription { get; set; }
        public Guid? LinkIconId { get; set; }
        public string LinkIconMd5 { get; set; }
        public string LinkIconText { get; set; }
        public int? LinkOrdinalPosition { get; set; }
        public Guid? LinkPermittedForScope { get; set; }
        public bool LinkShowOnPortal { get; set; }
        public DateTime LinkCreatedOnUtc { get; set; }
        public Guid LinkCreatedBy { get; set; }
        public DateTime LinkModifiedOnUtc { get; set; }
        public Guid LinkModifiedBy { get; set; }
    }
}
