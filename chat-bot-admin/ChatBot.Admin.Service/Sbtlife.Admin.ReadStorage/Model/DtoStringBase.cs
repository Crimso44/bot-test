using System;

namespace ChatBot.Admin.ReadStorage.Model
{
    public class DtoStringBase
    {
        public string Id { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is DtoStringBase item))
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
