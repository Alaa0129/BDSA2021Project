using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace BlazorApp.Infrastructure
{
    public interface IPBankContext : IDisposable
    {
        DbSet<User> Users { get; }
        DbSet<Project> Projects { get; }
        DbSet<Tag> Tags { get; set; }
        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        
    }
}