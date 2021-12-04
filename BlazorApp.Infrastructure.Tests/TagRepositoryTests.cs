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

            var student1 = new Student { Id = "StudentId1", Name = "Student One Name" };
            var student2 = new Student { Id = "StudentId2", Name = "Student Two Name" };
            var student3 = new Student { Id = "StudentId3", Name = "Student Three Name" };

            var supervisor1 = new Supervisor { Id = "SupervisorId1", Name = "Supervisor One Name" };
            var supervisor2 = new Supervisor { Id = "SupervisorId2", Name = "Supervisor Two Name" };

            var tag1 = new Tag("First Tag");
            var tag2 = new Tag("Second Tag");
            var tag3 = new Tag("Third Tag");

            _context.Projects.AddRange(
                new Project { Id = 1, Title = "Project One", Description = "This is the first project", Supervisor = supervisor1, AppliedStudents = new List<Student>() { student1, student2 }, Tags = new[] { tag1, tag2 } },
                new Project { Id = 2, Title = "Project Two", Description = "This is the second project", Supervisor = supervisor2, AppliedStudents = new[] { student3 }, Tags = new[] { tag3 } }
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
            Assert.Equal("First Tag", arrayOfTags[0].Name);
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