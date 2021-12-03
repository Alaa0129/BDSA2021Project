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

            _context.Students.AddRange(
                new Student { Id = "1", Name = "Student One Name" },
                new Student { Id = "2", Name = "Student Two Name" },
                new Student { Id = "3", Name = "Student Three Name" }
            );

            _context.SaveChanges();

            _repository = new StudentRepository(_context);
        }


        [Fact]
        public async Task Create_create_student_with_valid_data()
        {
            //Given
            var student = new StudentCreateDTO
            {
                Id = "Student Id 1",
                Name = "Test Student"
            };

            //When
            var createdId = await _repository.CreateAsync(student);

            var actualStudent = await _context.Students.Where(p => p.Id == createdId).FirstOrDefaultAsync();

            //Then
            Assert.Equal(actualStudent.Id, createdId);
            Assert.Equal("Test Student", actualStudent.Name);
            Assert.Null(actualStudent.project);
        }


        [Fact]
        public async Task Read_given_valid_id_returns_correct_student()
        {
            //Given
            var student = await _repository.ReadAsync("1");

            //Then
            Assert.Equal("1", student.Id);
            Assert.Equal("Student One Name", student.Name);
        }

        [Fact]
        public async Task Read_given_non_valid_id_returns_null()
        {
            //Given
            var student = await _repository.ReadAsync("133333");

            //Then
            Assert.Null(student);
        }

        [Fact]
        public async Task Read_returns_all_students()
        {
            //Given
            var students = await _repository.ReadAsync();

            //Then
            Assert.Collection(students,
                student => Assert.Equal(new StudentDTO("1", "Student One Name"), student),
                student => Assert.Equal(new StudentDTO("2", "Student Two Name"), student),
                student => Assert.Equal(new StudentDTO("3", "Student Three Name"), student)
                );

        }

        [Fact]
        public async Task Update_given_non_valid_id_returns_NotFound()
        {
            //Given
            var student = new StudentUpdateDTO
            {
                Id = "122",
                Name = "Student Update Name"
            };

            //When
            var response = await _repository.UpdateAsync(student);

            //Then
            Assert.Equal(NotFound, response);
        }

        [Fact]
        public async Task Update_given_valid_id_updates_student_and_returns_OK()
        {
            //Given
            var student = new StudentUpdateDTO
            {
                Id = "1",
                Name = "Student Update Name"
            };

            //When
            var response = await _repository.UpdateAsync(student);

            var updatedStudent = await _repository.ReadAsync("1");

            //Then
            Assert.Equal(OK, response);
            Assert.Equal("Student Update Name", updatedStudent.Name);
        }


        public void Dispose()
        {
            _context.Dispose();
        }
    }
}