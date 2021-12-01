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
        private readonly RequestRepository _repo;

        public RequestRepositoryTests()
        {
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();
            var builder = new DbContextOptionsBuilder<PBankContext>().UseSqlite(connection);

            _context = new PBankContext(builder.Options);
            _context.Database.EnsureCreated();

            // var user1 = new User { Id = 1, Firstname = "User One Firstname", Lastname = "User One Lastname" };
            // var user2 = new User { Id = 2, Firstname = "User Two Firstname", Lastname = "User Two Lastname" };
            // var user3 = new User { Id = 3, Firstname = "User Three Firstname", Lastname = "User Three Lastname" };

            _context.Requests.AddRange(
                new Request { Id = 1, Title = "Request One", Description = "Description one", StudentId = 4},
                new Request { Id = 2, Title = "Request Two", Description = "Description two", StudentId = 5}
            );

            _context.SaveChanges();

            _repo = new RequestRepository(_context);
        }

        [Fact]
        public async Task CreateAsync_create_a_request_and_return_its_id()
        {
            //Given
            var request = new RequestCreateDTO
            {
                Title = "Title",
                Description = "Description",
                StudentId = 3
            };

            //When
            var createdId = await _repo.CreateAsync(request);
            var actualRequest = await _context.Requests.Where(r => r.Id == createdId).FirstOrDefaultAsync();

            //Then
            Assert.Equal(actualRequest.Id, createdId);
            Assert.Equal("Title", actualRequest.Title);
            Assert.Equal("Description", actualRequest.Description);
            Assert.Equal(3, actualRequest.StudentId);
        }

        [Fact]
        public async Task ReadAsync_returns_all_requests()
        {
        //when
            var requests = await _repo.ReadAsync();

        //then
            Assert.Collection(requests, 
                request => Assert.Equal(new RequestDetailsDTO(1, "Request One", "Description one", 4), request),
                request => Assert.Equal(new RequestDetailsDTO(2, "Request Two", "Description two", 5), request)
            );
        }


        [Fact]
        public async void Given_a_non_existing_requestid_returns_null()
        {
        //Given
            var request = await _repo.ReadAsync(888);
                
        //Then
            Assert.Null(request);
        }

        [Fact]
        public async Task Given_an_existing_requestid_returns_the_request()
        {        
        //When
            var request = await _repo.ReadAsync(1);

        //Then
            Assert.Equal(1, request.Id);
            Assert.Equal("Request One", request.Title);
            Assert.Equal("Description one", request.Description);
            Assert.Equal(4, request.StudentId);
        }

        [Fact]
        public async void Given_a_non_existing_requestid_returns_notfound_when_trying_to_delete_it()
        {        
        //When
            var response = await _repo.DeleteAsync(888);
        
        //Then
            Assert.Equal(NotFound, response);
        }

        [Fact]
        public async void Given_an_existing_requestid_deletes_request()
        {
        //When
            var response = await _repo.DeleteAsync(1);
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