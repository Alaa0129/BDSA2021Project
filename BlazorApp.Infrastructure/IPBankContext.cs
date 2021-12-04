using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace BlazorApp.Infrastructure
{
    public interface IPBankContext : IDisposable
    {
        DbSet<Student> Students { get; }
        DbSet<Supervisor> Supervisors { get; }
        DbSet<Project> Projects { get; }
        DbSet<Request> Requests { get; }
        DbSet<Tag> Tags { get; }
        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    }
}