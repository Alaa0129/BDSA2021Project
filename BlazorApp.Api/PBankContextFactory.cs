using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace BlazorApp.Api 
{
    public class PBankContextFactory : IDesignTimeDbContextFactory<PBankContext>
    {
        public PBankContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddUserSecrets<Program>()
                .Build();
            
            var connectionString = configuration.GetConnectionString("PBank");

            var optionsBuilder = new DbContextOptionsBuilder<KanbanContext>().UseSqlServer(connectionString);

            return new PBankContext(optionsBuilder.Options);
        }
    }
}