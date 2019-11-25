using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Service.Data.Configuration
{
    public class AvailabilityServiceContextFactory : IDesignTimeDbContextFactory<AvailabilityServiceContext>
    {
        public AvailabilityServiceContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AvailabilityServiceContext>();
            optionsBuilder.UseSqlServer(
                "Server=tcp:etl-monitoring-poc.database.windows.net,1433;Initial Catalog=monitoring;Persist Security Info=False;User ID=dbaadmin;Password=SQLServer2008;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

            return new AvailabilityServiceContext(optionsBuilder.Options);
        }
    }
}
