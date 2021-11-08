using BlazorApp.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BlazorApp.Infrastructure
{
    public class PBankContext : DbContext, IPBankContext
    {
        public DbSet<User> Users {get; set;}

        public PBankContext(DbContextOptions<PBankContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Specific constraints if needed
        }
    }
}