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
    public class SupervisorRepositoryTests : IDisposable
    {
        private readonly PBankContext _context;
        private readonly SupervisorRepository _repository;

        public SupervisorRepositoryTests()
        {
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();
            var builder = new DbContextOptionsBuilder<PBankContext>().UseSqlite(connection);

            _context = new PBankContext(builder.Options);
            _context.Database.EnsureCreated();

            _context.Supervisors.AddRange(
                new Supervisor { Id = "SupervisorId1", Name = "Supervisor One Name" },
                new Supervisor { Id = "SupervisorId2", Name = "Supervisor Two Name" },
                new Supervisor { Id = "SupervisorId3", Name = "Supervisor Three Name" }
            );

            _context.SaveChanges();

            _repository = new SupervisorRepository(_context);
        }


        [Fact]
        public async Task Create_create_Supervisor_with_valid_data()
        {
            //Given
            var supervisor = new SupervisorCreateDTO
            {
                Id = "SupervisorId4",
                Name = "Supervisor Fourth Name"
            };

            //When
            var createdId = await _repository.CreateAsync(supervisor);

            var actualSupervisor = await _context.Supervisors.Where(p => p.Id == createdId).FirstOrDefaultAsync();

            //Then
            Assert.Equal(actualSupervisor.Id, createdId);
            Assert.Equal("Supervisor Fourth Name", actualSupervisor.Name);
            Assert.Empty(actualSupervisor.Projects);
            Assert.Empty(actualSupervisor.Requests);
        }


        [Fact]
        public async Task Read_given_valid_id_returns_correct_Supervisor()
        {
            //Given
            var supervisor = await _repository.ReadAsync("SupervisorId1");

            //Then
            Assert.Equal("SupervisorId1", supervisor.Id);
            Assert.Equal("Supervisor One Name", supervisor.Name);
            Assert.Empty(supervisor.Projects);
            Assert.Empty(supervisor.Requests);
        }

        [Fact]
        public async Task Read_given_non_valid_id_returns_null()
        {
            //Given
            var supervisor = await _repository.ReadAsync("133333");

            //Then
            Assert.Null(supervisor);
        }

        [Fact]
        public async Task Read_returns_all_Supervisors()
        {
            //Given
            var supervisors = await _repository.ReadAsync();

            //Then
            Assert.Collection(supervisors,
                Supervisor => Assert.Equal(new SupervisorDTO("SupervisorId1", "Supervisor One Name"), Supervisor),
                Supervisor => Assert.Equal(new SupervisorDTO("SupervisorId2", "Supervisor Two Name"), Supervisor),
                Supervisor => Assert.Equal(new SupervisorDTO("SupervisorId3", "Supervisor Three Name"), Supervisor)
                );

        }

        [Fact]
        public async Task Update_given_non_valid_id_returns_NotFound()
        {
            //Given
            var Supervisor = new SupervisorUpdateDTO
            {
                Id = "SupervisorId10000",
                Name = "Supervisor Update Name"
            };

            //When
            var response = await _repository.UpdateAsync(Supervisor);

            //Then
            Assert.Equal(NotFound, response);
        }

        [Fact]
        public async Task Update_given_valid_id_updates_Supervisor_and_returns_OK()
        {
            //Given
            var supervisor = new SupervisorUpdateDTO
            {
                Id = "SupervisorId1",
                Name = "Supervisor Update Name"
            };

            //When
            var response = await _repository.UpdateAsync(supervisor);

            var updatedSupervisor = await _repository.ReadAsync("SupervisorId1");

            //Then
            Assert.Equal(OK, response);
            Assert.Equal("Supervisor Update Name", updatedSupervisor.Name);
        }


        public void Dispose()
        {
            _context.Dispose();
        }
    }
}