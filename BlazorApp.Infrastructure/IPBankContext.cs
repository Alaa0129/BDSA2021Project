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
        DbSet<Tag> Tags { get; }
        DbSet<Request> Requests {get; }
        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        
    }
}