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
    public class RequestRepositoryTests : IDisposable
    {

        private readonly PBankContext _context;
        private readonly RequestRepository _repository;

        public RequestRepositoryTests()
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


            _context.Requests.AddRange(
                new Request { Id = 1, Title = "Request One", Description = "Description One", Student = student1, Supervisor = supervisor1 },
                new Request { Id = 2, Title = "Request Two", Description = "Description Two", Student = student2, Supervisor = supervisor2 }
            );

            _context.SaveChanges();

            _repository = new RequestRepository(_context);
        }

        [Fact]
        public async Task CreateAsync_create_a_request_and_return_its_id()
        {
            //Given
            var request = new RequestCreateDTO
            {
                Title = "Title",
                Description = "Description",
                StudentId = "StudentId1",
                SupervisorId = "SupervisorId2"
            };

            // When
            var createdId = await _repository.CreateAsync(request);
            var actualRequest = await _context.Requests.Where(r => r.Id == createdId).FirstOrDefaultAsync();

            //Then
            Assert.Equal(actualRequest.Id, createdId);
            Assert.Equal("Title", actualRequest.Title);
            Assert.Equal("Description", actualRequest.Description);
            Assert.Equal(_context.Students.Find("StudentId1"), actualRequest.Student);
            Assert.Equal(_context.Supervisors.Find("SupervisorId2"), actualRequest.Supervisor);
        }


        [Fact]
        public async Task ReadAsync_returns_all_requests()
        {
            //when
            var requests = await _repository.ReadAsync();

            //then
            Assert.Collection(requests,
                request => Assert.Equal(new RequestDTO(1, "Request One", "Description One", "StudentId1", "SupervisorId1"), request),
                request => Assert.Equal(new RequestDTO(2, "Request Two", "Description Two", "StudentId2", "SupervisorId2"), request)
            );
        }


        [Fact]
        public async void Read_given_a_non_existing_requestid_returns_null()
        {
            //Given
            var request = await _repository.ReadAsync(888);

            //Then
            Assert.Null(request);
        }

        [Fact]
        public async Task Read_given_an_existing_requestid_returns_the_request()
        {
            //When
            var request = await _repository.ReadAsync(1);

            //Then
            Assert.Equal(1, request.Id);
            Assert.Equal("Request One", request.Title);
            Assert.Equal("Description One", request.Description);
            Assert.Equal("StudentId1", request.Student.Id);
            Assert.Equal("SupervisorId1", request.Supervisor.Id);
        }

        [Fact]
        public async void Delete_given_a_non_existing_requestid_returns_NotFound()
        {        
        //When
            var response = await _repository.DeleteAsync(888);

        //Then
            Assert.Equal(NotFound, response);
        }

        [Fact]
        public async void Delte_given_an_existing_requestid_deletes_request()
        {
        //When
            var response = await _repository.DeleteAsync(1);
            var deletedRequest = await _context.Requests.FindAsync(1);

        //Then
            Assert.Equal(OK, response);
            Assert.Null(deletedRequest);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}