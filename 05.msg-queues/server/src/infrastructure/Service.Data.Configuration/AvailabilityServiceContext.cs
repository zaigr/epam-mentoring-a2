using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Service.Data.Entities;

namespace Service.Data.Configuration
{
    public class AvailabilityServiceContext : DbContext, IAvailabilityServiceContext
    {
        private readonly string _connectionString;

        public AvailabilityServiceContext(string connectionString)
        {
            _connectionString = connectionString;
        }


        public AvailabilityServiceContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<ClientAvailability> Availabilities { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured && !string.IsNullOrEmpty(_connectionString))
            {
                optionsBuilder.UseSqlServer(_connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var assembly = Assembly.GetExecutingAssembly();
            modelBuilder.ApplyConfigurationsFromAssembly(assembly);
        }
    }
}
