using Microsoft.EntityFrameworkCore;
using Service.Data.Entities;

namespace Service.Data
{
    public interface IAvailabilityServiceContext
    {
        DbSet<ClientAvailability> Availabilities { get; set; }

        int SaveChanges();
    }
}
