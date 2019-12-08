using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Client.Data.Configuration
{
    public class BloggingContextFactory : IDesignTimeDbContextFactory<ScanServiceContext>
    {
        public ScanServiceContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ScanServiceContext>();
            optionsBuilder.UseSqlite("Data Source=blog.db");

            return new ScanServiceContext(optionsBuilder.Options);
        }
    }
}
