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
    public class StudentRepositoryTests : IDisposable
    {
        private readonly PBankContext _context;
        private readonly StudentRepository _repository;

        public StudentRepositoryTests()
        {
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();
            var builder = new DbContextOptionsBuilder<PBankContext>().UseSqlite(connection);

            _context = new PBankContext(builder.Options);
            _context.Database.EnsureCreated();

            var supervisor1 = new Supervisor { Id = "SupervisorId1", Name = "Supervisor One Name" };

            var project1 = new Project { Id = 1, Title = "Project One", Description = "This is the first project", Supervisor = supervisor1 };


            _context.Students.AddRange(
                new Student { Id = "StudentId1", Name = "Student One Name" },
                new Student { Id = "StudentId2", Name = "Student Two Name" },
                new Student { Id = "StudentId3", Name = "Student Three Name", Project = project1 }
            );

            _context.Projects.Add(new Project { Id = 2, Title = "Project Two", Description = "This is the second project", Supervisor = supervisor1 });

            _context.SaveChanges();

            _repository = new StudentRepository(_context);
        }


        [Fact]
        public async Task Create_creates_student_with_correct_data()
        {
            //Given
            var student = new StudentCreateDTO
            {
                Id = "StudentId4",
                Name = "Student Fourth Name"
            };

            //When
            var createdId = await _repository.CreateAsync(student);

            var actualStudent = await _context.Students.FindAsync(student.Id);

            //Then
            Assert.Equal("StudentId4", createdId);
            Assert.Equal("Student Fourth Name", actualStudent.Name);
            Assert.Null(actualStudent.Project);
            Assert.Empty(actualStudent.Requests);
        }

        [Fact]
        public async Task Create_with_already_used_id_throws_error()
        {
            //Given
            var student = new StudentCreateDTO
            {
                Id = "StudentId3",
                Name = "Student Fourth Name"
            };

            //Then
            await Assert.ThrowsAsync<Exception>(async () => await _repository.CreateAsync(student));
        }


        [Fact]
        public async Task Read_given_valid_id_returns_correct_student()
        {
            // Given
            var student = await _repository.ReadAsync("StudentId1");

            // Then
            Assert.Equal("StudentId1", student.Id);
            Assert.Equal("Student One Name", student.Name);
            Assert.Null(student.Project);
            Assert.Empty(student.Requests);
        }

        [Fact]
        public async Task Read_given_non_valid_id_returns_null()
        {
            // Given
            var student = await _repository.ReadAsync("StudentId10000");

            // Then
            Assert.Null(student);
        }

        [Fact]
        public async Task Read_returns_all_students()
        {
            // Given
            var students = await _repository.ReadAsync();

            // Then
            Assert.Collection(students,
                student => Assert.Equal(new StudentDTO("StudentId1", "Student One Name"), student),
                student => Assert.Equal(new StudentDTO("StudentId2", "Student Two Name"), student),
                student => Assert.Equal(new StudentDTO("StudentId3", "Student Three Name"), student)
                );
        }



        [Fact]
        public async Task Update_given_non_valid_id_returns_NotFound()
        {
            // Given
            var student = new StudentUpdateDTO
            {
                Id = "StudentId10000",
                Name = "Student Update Name"
            };

            // When
            var response = await _repository.UpdateAsync(student);

            // Then
            Assert.Equal(NotFound, response);
        }

        [Fact]
        public async Task Update_given_valid_id_updates_student_and_returns_OK()
        {
            // Given
            var student = new StudentUpdateDTO
            {
                Id = "StudentId3",
                Name = "Student Update Name"
            };

            // When
            var response = await _repository.UpdateAsync(student);

            var updatedStudent = await _repository.ReadAsync("StudentId3");

            // Then
            Assert.Equal(OK, response);
            Assert.Equal("Student Update Name", updatedStudent.Name);
            Assert.Equal(_context.Projects.Find(1).Title, updatedStudent.Project.Title);
        }

        [Fact]
        public async Task UpdateProject_given_valid_ids_replaces_project_on_student_and_returns_OK()
        {
            // Given
            var studentId = "StudentId3";
            var projectId = 2;

            // When
            var response = await _repository.UpdateProjectAsync(studentId, projectId);

            var updatedStudent = await _repository.ReadAsync(studentId);

            // Then
            Assert.Equal(OK, response);
            Assert.Equal(_context.Projects.Find(2).Title, updatedStudent.Project.Title);
        }

        [Fact]
        public async Task UpdateProject_given_non_valid_project_id_returns_BadRequest()
        {
            // Given
            var studentId = "StudentId3";
            var projectId = 10000000;

            // When
            var response = await _repository.UpdateProjectAsync(studentId, projectId);

            var updatedStudent = await _repository.ReadAsync(studentId);

            // Then
            Assert.Equal(BadRequest, response);
        }

        [Fact]
        public async Task UpdateProject_given_non_valid_student_id_returns_BadRequest()
        {
            // Given
            var studentId = "StudentId10000";
            var projectId = 2;

            // When
            var response = await _repository.UpdateProjectAsync(studentId, projectId);

            var updatedStudent = await _repository.ReadAsync(studentId);

            // Then
            Assert.Equal(BadRequest, response);
        }


        public void Dispose()
        {
            _context.Dispose();
        }
    }
}