using System;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Linq;
using System.Collections.Generic;

namespace BlazorApp.Infrastructure.Tests
{
    public class TagRepositoryTests : IDisposable
    {
        private readonly PBankContext _context;
        private readonly TagRepository _repository;

        public TagRepositoryTests()
        {

            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();
            var builder = new DbContextOptionsBuilder<PBankContext>().UseSqlite(connection);

            _context = new PBankContext(builder.Options);
            _context.Database.EnsureCreated();

            var user1 = new User { Id = 1, Firstname = "User One Firstname", Lastname = "User One Lastname" };
            var user2 = new User { Id = 2, Firstname = "User Two Firstname", Lastname = "User Two Lastname" };
            var user3 = new User { Id = 3, Firstname = "User Three Firstname", Lastname = "User Three Lastname" };

            var tag1 = new Tag("First Tag");
            var tag2 = new Tag("Second Tag");
            var tag3 = new Tag("Third Tag");

            _context.Projects.AddRange(
                new Project { Id = 1, Title = "Project One", Description = "This is the first project", SupervisorId = 1, MaxApplications = 4, AppliedStudents = new[] { user1, user2 }, Tags = new List<Tag>() { tag1, tag2 } },
                new Project { Id = 2, Title = "Project Two", Description = "This is the second project", SupervisorId = 2, MaxApplications = 1, AppliedStudents = new[] { user3 }, Tags = new List<Tag> { tag3 } }
            );

            _context.SaveChanges();

            _repository = new TagRepository(_context);
        }

        [Fact]
        public async Task Read_async_returns_all_tags()
        {
    
            var tags = await _repository.ReadAsyncAll();

            var arrayOfTags = tags.ToArray();

            Assert.Equal(1, arrayOfTags[0].Id);
            Assert.Equal("First Tag",arrayOfTags[0].Name);
            Assert.Equal(2, arrayOfTags[1].Id);
            Assert.Equal("Second Tag", arrayOfTags[1].Name);
            Assert.Equal(3, arrayOfTags[2].Id);
            Assert.Equal("Third Tag", arrayOfTags[2].Name);

        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}