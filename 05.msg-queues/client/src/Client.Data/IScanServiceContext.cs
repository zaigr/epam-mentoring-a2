using Client.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Client.Data
{
    public interface IScanServiceContext
    {
        DbSet<Resource> Resources { get; set; }

        void SaveChanges();
    }
}
