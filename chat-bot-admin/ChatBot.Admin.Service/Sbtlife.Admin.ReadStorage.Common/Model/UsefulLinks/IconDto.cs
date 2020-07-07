
using System;

namespace Sbtlife.Admin.ReadStorage.Common.Model.UsefulLinks
{
    public class IconDto : DtoBase
    {
        // ReSharper disable once InconsistentNaming
        public string MD5 { get; set; }
        public string Name { get; set; }
        public string ContentType { get; set; }
        public byte[] Data { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime ModifiedOnUtc { get; set; }
        public Guid ModifiedBy { get; set; }
        public int LinkCount { get; set; }
    }
}
