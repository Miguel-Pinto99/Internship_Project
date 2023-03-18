using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Project1.Data;
using Project1.Persistance;
using Xunit;
using Xunit.Abstractions;

namespace PVSDashboard.Tests.Persistance
{
    public class RepositoryTestsBase : IDisposable
    {
        
        protected AppDbContext context;

        public RepositoryTestsBase()
        {           
            var dbOptBuilder = GetDbOptionsBuilder();
            context = new AppDbContext(dbOptBuilder.Options);
            context.Database.EnsureCreated();
        }

        public void Dispose()
        {           
            context.Dispose();
        }

        private static DbContextOptionsBuilder<AppDbContext> GetDbOptionsBuilder()
        {
            // The key to keeping the databases unique and not shared is 
            // generating a unique db name for each.
            string dbName = Guid.NewGuid().ToString();

            // Create a fresh service provider, and therefore a fresh 
            // InMemory database instance.
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            // Create a new options instance telling the context to use an
            // InMemory database and the new service provider.
            var builder = new DbContextOptionsBuilder<AppDbContext>();
            builder.UseInMemoryDatabase(dbName)
                   .UseInternalServiceProvider(serviceProvider);

            return builder;
        }
    }
}
