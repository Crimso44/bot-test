using System;

namespace Sbtlife.Admin.ReadStorage.Common.Model
{
    public class DtoIntBase
    {
        public int Id { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is DtoIntBase item))
                return false;

            return Id.Equals(item.Id);
        }

        public override int GetHashCode()
        {
            // ReSharper disable once NonReadonlyMemberInGetHashCode
            return Id.GetHashCode();
        }
    }
}
