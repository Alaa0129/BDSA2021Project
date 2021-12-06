using BlazorApp.Core;
using Microsoft.EntityFrameworkCore;


namespace BlazorApp.Infrastructure
{
    public class PBankContext : DbContext, IPBankContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Supervisor> Supervisors { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Tag> Tags { get; set; }

        public PBankContext(DbContextOptions<PBankContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Specific constraints if needed
        }
    }
}