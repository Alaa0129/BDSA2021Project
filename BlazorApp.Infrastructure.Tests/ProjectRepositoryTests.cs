using System;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using System.Threading.Tasks;
using System.Linq;
using BlazorApp.Core;
using static System.Net.HttpStatusCode;
using System.Collections.Generic;

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
                SupervisorId = "SupervisorId1",
                Tags = new[] { "First Tag" }
            };

            //When
            var createdId = await _repository.CreateAsync(project);
            var actualProject = await _context.Projects.Where(p => p.Id == createdId).FirstOrDefaultAsync();

            //Then
            Assert.Equal(actualProject.Id, createdId);
            Assert.Equal("Title", actualProject.Title);
            Assert.Equal("Description", actualProject.Description);
            Assert.Equal(_context.Supervisors.Find("SupervisorId1"), actualProject.Supervisor);
            Assert.Equal(_context.Tags.Find(1), actualProject.Tags.First());
        }

        [Fact]
        public async Task Create_with_new_tags_add_to_tags_table()
        {
            //Given
            var project = new ProjectCreateDTO
            {
                Title = "Title",
                Description = "Description",
                SupervisorId = "SupervisorId1",
                Tags = new[] { "Forth Tag", "Third Tag" }
            };

            //When
            var createdId = await _repository.CreateAsync(project);
            var actualProject = await _context.Projects.Where(p => p.Id == createdId).FirstOrDefaultAsync();

            //Then
            Assert.Equal(actualProject.Id, createdId);
            Assert.Equal("Title", actualProject.Title);
            Assert.Equal("Description", actualProject.Description);
            Assert.Equal(_context.Supervisors.Find("SupervisorId1"), actualProject.Supervisor);
            Assert.Equal(_context.Tags.Find(4), actualProject.Tags.First());
            Assert.Equal(_context.Tags.Find(3), actualProject.Tags.Last());
        }


        [Fact]
        public async Task ReadAsync_returns_all_projects()
        {
            //When
            var projects = await _repository.ReadAsync();

            //Then

            var project1 = projects.First();

            Assert.Collection(projects,
                project => Assert.Equal(new ProjectDTO(1, "Project One", "This is the first project", "SupervisorId1"), project),
                project => Assert.Equal(new ProjectDTO(2, "Project Two", "This is the second project", "SupervisorId2"), project)
            );
        }

        [Fact]
        public async Task Read_given_non_valid_id_returns_null()
        {
            //When
            var project = await _repository.ReadAsync(232);

            //Then
            Assert.Null(project);
        }

        [Fact]
        public async Task Read_given_valid_id_return_correct_project()
        {
            //When
            var projectDetails = await _repository.ReadAsync(1);

            //Then
            Assert.Equal(1, projectDetails.Id);
            Assert.Equal("Project One", projectDetails.Title);
            Assert.Equal("This is the first project", projectDetails.Description);
            Assert.Equal(_context.Supervisors.Find("SupervisorId1").Name, projectDetails.Supervisor.Name);
            Assert.Equal(2, projectDetails.AppliedStudents.Count);
            Assert.Equal(2, projectDetails.Tags.Count);
        }

        [Fact]
        public async Task Update_given_non_valid_project_id_returns_NotFound()
        {
            //Given
            var project = new ProjectUpdateDTO
            {
                Id = 122,
                Title = "Title",
                Description = "Description"
            };

            //When
            var response = await _repository.UpdateAsync(project);

            //Then
            Assert.Equal(NotFound, response);
        }


        [Fact]
        public async Task Update_given_valid_project_id_updates_project()
        {
            //Given
            var project = new ProjectUpdateDTO
            {
                Id = 1,
                Title = "Title",
                Description = "Description",
            };

            //When
            var response = await _repository.UpdateAsync(project);

            var updatedProject = await _repository.ReadAsync(1);

            //Then
            Assert.Equal(OK, response);
            Assert.Equal("Title", updatedProject.Title);
            Assert.Equal("Description", updatedProject.Description);
            Assert.Equal(2, updatedProject.AppliedStudents.Count);
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