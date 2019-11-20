using System.Reflection;
using Client.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Client.Data.Configuration
{
    public class ScanServiceContext : DbContext, IScanServiceContext
    {
        private readonly string _connectionString;

        public ScanServiceContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public ScanServiceContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Resource> Resources { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=scan-service.db");

            //if (!optionsBuilder.IsConfigured && !string.IsNullOrEmpty(_connectionString))
            //{
            //    optionsBuilder.UseSqlite(_connectionString);
            //}
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var assembly = Assembly.GetExecutingAssembly();
            modelBuilder.ApplyConfigurationsFromAssembly(assembly);
        }
    }
}
