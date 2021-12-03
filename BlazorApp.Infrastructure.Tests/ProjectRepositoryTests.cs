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

            var user1 = new User { Id = 1, Firstname = "User One Firstname", Lastname = "User One Lastname" };
            var user2 = new User { Id = 2, Firstname = "User Two Firstname", Lastname = "User Two Lastname" };
            var user3 = new User { Id = 3, Firstname = "User Three Firstname", Lastname = "User Three Lastname" };

            var tag1 = new Tag("First Tag");
            var tag2 = new Tag("Second Tag");
            var tag3 = new Tag("Third Tag");




            _context.Projects.AddRange(
                new Project { Id = 1, Title = "Project One", Description = "This is the first project", SupervisorId = 1, MaxApplications = 4, AppliedStudents = new[] { user1, user2 }, Tags = new List<Tag>(){tag1,tag2}},
                new Project { Id = 2, Title = "Project Two", Description = "This is the second project", SupervisorId = 2, MaxApplications = 1, AppliedStudents = new[] { user3 }, Tags = new List<Tag>{tag3}}
            );

            _context.SaveChanges();

            _repository = new ProjectRepository(_context);
        }


        [Fact]
        public async Task CreateAsync_create_correct_project_and_returns_Id()
        {
            //Given
            var project = new ProjectCreateDTO
            {
                Title = "Title",
                Description = "Description",
                MaxApplications = 4,
                SupervisorId = 1,
                Tags = new[]
                {
                    "tag1"
                }
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
            Assert.Equal(4, actualProject.Tags.First().Id);
            Assert.Equal("tag1", actualProject.Tags.First().Name);
            Assert.Equal(new[] { actualProject }, actualProject.Tags.First().Projects);
        }

        [Fact]
        public async Task ReadAsync_returns_all_projects()
        {
            //When
            var projects = await _repository.ReadAsyncAll();
            
            var arrayOfProjects = projects.ToArray();

            // Then

            Assert.Equal(1,arrayOfProjects[0].Id);
            Assert.Equal("Project One", arrayOfProjects[0].Title);
            Assert.Equal("This is the first project", arrayOfProjects[0].Description);
            Assert.Equal(1, arrayOfProjects[0].SupervisorId);
            Assert.Equal(4, arrayOfProjects[0].MaxApplications);
            Assert.Equal(new[] { "First Tag", "Second Tag" }, arrayOfProjects[0].Tags);

            Assert.Equal(2, arrayOfProjects[1].Id);
            Assert.Equal("Project Two", arrayOfProjects[1].Title);
            Assert.Equal("This is the second project", arrayOfProjects[1].Description);
            Assert.Equal(2, arrayOfProjects[1].SupervisorId);
            Assert.Equal(1, arrayOfProjects[1].MaxApplications);
            Assert.Equal(new [] { "Third Tag" }, arrayOfProjects[1].Tags);

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
            var project = await _repository.ReadAsync(1);

            //Then
            Assert.Equal(1, project.Id);
            Assert.Equal("Project One", project.Title);
            Assert.Equal("This is the first project", project.Description);
            Assert.Equal(1, project.SupervisorId);
            Assert.Equal(4, project.MaxApplications);
            Assert.Equal(new[] { "First Tag", "Second Tag" },project.Tags);
        }

        [Fact]
        public async Task Read_given_tag_name_returns_projects_with_that_tag_name()
        {
            var projects = await _repository.ReadAsyncAllByTagName("Third Tag");
            
            var actualProject = projects.First();

            Assert.Equal(2, actualProject.Id);
            Assert.Equal("Project Two", actualProject.Title);
            Assert.Equal("This is the second project", actualProject.Description);
            Assert.Equal(2, actualProject.SupervisorId);
            Assert.Equal(1, actualProject.MaxApplications);
            Assert.Equal(new[] { "Third Tag" }, actualProject.Tags);

        }

        [Fact]
        public async Task Read_given_invalid_tag_name_returns_empty_list()
        {
            var projects = await _repository.ReadAsyncAllByTagName("invalid name");

            Assert.Equal(new List<ProjectDetailsDTO>(), projects);
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
                SupervisorId = 123,
                Tags = new[] {"Updated Tag"}
            };

            //When
            var response = await _repository.UpdateAsync(project);

            var updatedProject = await _repository.ReadAsync(1);

            //Then
            Assert.Equal(OK, response);
            Assert.Equal("Title", updatedProject.Title);
            Assert.Equal("Description", updatedProject.Description);
            Assert.Equal(100, updatedProject.MaxApplications);
            Assert.Equal(123, updatedProject.SupervisorId);
            Assert.Equal(new[] { "Updated Tag" }, updatedProject.Tags);
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