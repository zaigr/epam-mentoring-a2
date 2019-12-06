using System;
using Client.Data.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Client.Core.Tests.Unit
{
    public static class DbContextFactory
    {
        public static ScanServiceContext CreateImMemory()
        {
            var options = new DbContextOptionsBuilder<ScanServiceContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new ScanServiceContext(options);
        }
    }
}
