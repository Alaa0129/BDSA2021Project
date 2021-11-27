using BlazorApp.Core;
using Microsoft.EntityFrameworkCore;


namespace BlazorApp.Infrastructure
{
    public class PBankContext : DbContext, IPBankContext
    {
        public DbSet<User> Users {get; set;}
        public DbSet<Project> Projects {get; set;}

        public PBankContext(DbContextOptions<PBankContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Specific constraints if needed
        }
    }
}