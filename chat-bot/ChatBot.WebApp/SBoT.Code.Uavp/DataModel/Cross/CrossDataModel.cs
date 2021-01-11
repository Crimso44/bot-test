using Microsoft.EntityFrameworkCore;
using SBoT.Code.Uavp.DataModel.Cross.Interfaces;

namespace SBoT.Code.Uavp.DataModel.Cross
{
    public class CrossDataModel : DbContext, ICrossDataModel
    {
        public CrossDataModel(DbContextOptions<CrossDataModel> options) : base(options)
        {
        }

        public virtual DbSet<Staff> Staff { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Staff>().HasKey(x => x.Id);
        }

    }
}
