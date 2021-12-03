using System;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using System.Threading.Tasks;
using System.Linq;
using BlazorApp.Core;
using static System.Net.HttpStatusCode;

namespace BlazorApp.Infrastructure.Tests
{
    public class ProjectRepositoryTests : IDisposable
    {

        private readonly PBankContext _context;
        private readonly ProjectRepository _repository;

        public ProjectRepositoryTests()
        {
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();
            var builder = new DbContextOptionsBuilder<PBankContext>().UseSqlite(connection);

            _context = new PBankContext(builder.Options);
            _context.Database.EnsureCreated();

            var student1 = new Student { Id = "1", Name = "Student One Name"};
            var student2 = new Student { Id = "2", Name = "Student Two Name"};
            var student3 = new Student { Id = "3", Name = "Student Three Name"};

            _context.Projects.AddRange(
                new Project { Id = 1, Title = "Project One", Description = "This is the first project", SupervisorId = 1, MaxApplications = 4, AppliedStudents = new[] { student1, student2 } },
                new Project { Id = 2, Title = "Project Two", Description = "This is the second project", SupervisorId = 2, MaxApplications = 1, AppliedStudents = new[] { student3 } }
            );

            _context.SaveChanges();

            _repository = new ProjectRepository(_context);
        }


        [Fact]
        public async Task CreateAsync_create_correct_project_and_return_Id()
        {
            //Given
            var project = new ProjectCreateDTO
            {
                Title = "Title",
                Description = "Description",
                MaxApplications = 4,
                SupervisorId = 1,
            };

            //When
            var createdId = await _repository.CreateAsync(project);
            var actualProject = await _context.Projects.Where(p => p.Id == createdId).FirstOrDefaultAsync();

            //Then
            Assert.Equal(actualProject.Id, createdId);
            Assert.Equal("Title", actualProject.Title);
            Assert.Equal("Description", actualProject.Description);
            Assert.Equal(4, actualProject.MaxApplications);
            Assert.Equal(1, actualProject.SupervisorId);
        }

        [Fact]
        public async Task ReadAsync_returns_all_projects()
        {
            //When
            var projects = await _repository.ReadAsync();

            //Then
            Assert.Collection(projects,
                project => Assert.Equal(new ProjectDetailsDTO(1, "Project One", "This is the first project", 1, 4), project),
                project => Assert.Equal(new ProjectDetailsDTO(2, "Project Two", "This is the second project", 2, 1), project)
            );
        }

        [Fact]
        public async Task Read_given_non_valid_id_returns_null()
        {
            //When
            var project = await _repository.ReadAsync(232);

            //Then
            Assert.Equal(null, project);
        }

        [Fact]
        public async Task Read_given_valid_id_return_correct_project()
        {
            //When
            var project = await _repository.ReadAsync(1);

            //Then
            Assert.Equal(1, project.Id);
            Assert.Equal("Project One", project.Title);
            Assert.Equal("This is the first project", project.Description);
            Assert.Equal(1, project.SupervisorId);
            Assert.Equal(4, project.MaxApplications);
        }

        [Fact]
        public async Task Update_given_non_valid_id_returns_NotFound()
        {
            //Given
            var project = new ProjectUpdateDTO
            {
                Id = 122,
                Title = "Title",
                Description = "Description",
                MaxApplications = 100,
                SupervisorId = 123
            };

            //When
            var response = await _repository.UpdateAsync(project);

            //Then
            Assert.Equal(NotFound, response);
        }

        [Fact]
        public async Task Update_given_valid_id_updates_project_and_returns_OK()
        {
            //Given
            var project = new ProjectUpdateDTO
            {
                Id = 1,
                Title = "Title",
                Description = "Description",
                MaxApplications = 100,
                SupervisorId = 123
            };

            //When
            var response = await _repository.UpdateAsync(project);

            var updatedProject = await _repository.ReadAsync(1);

            //Then
            Assert.Equal(OK, response);
            Assert.Equal("Title", updatedProject.Title);
            Assert.Equal("Description", updatedProject.Description);
        }

        [Fact]
        public async Task Delete_given_non_valid_id_returns_NotFound()
        {       
        //When
        var reponse = await _repository.DeleteAsync(100);

        //Then
        Assert.Equal(NotFound, reponse);
        }

        [Fact]
        public async Task Delete_given_valid_id_returns_OK_and_removes_project()
        {
        //When
        var response = await _repository.DeleteAsync(1);

        //Then
        Assert.Equal(OK, response);
        Assert.Null(await _context.Projects.FindAsync(1));
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}