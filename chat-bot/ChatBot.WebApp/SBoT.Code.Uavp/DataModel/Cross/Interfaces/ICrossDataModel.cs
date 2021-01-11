using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace SBoT.Code.Uavp.DataModel.Cross.Interfaces
{
    public interface ICrossDataModel
    {
        DbSet<Staff> Staff { get; set; }

        int SaveChanges();
        DatabaseFacade Database { get; }
    }

}
