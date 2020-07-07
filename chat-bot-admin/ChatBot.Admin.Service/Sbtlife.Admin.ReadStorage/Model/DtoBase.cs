using System;

namespace ChatBot.Admin.ReadStorage.Model
{
    public class DtoBase
    {
        public Guid Id { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is DtoBase item))
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
