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
                new Supervisor { Id = "1", Name = "Supervisor One Name" },
                new Supervisor { Id = "2", Name = "Supervisor Two Name" },
                new Supervisor { Id = "3", Name = "Supervisor Three Name" }
            );

            _context.SaveChanges();

            _repository = new SupervisorRepository(_context);
        }


        [Fact]
        public async Task Create_create_Supervisor_with_valid_data()
        {
            //Given
            var Supervisor = new SupervisorCreateDTO
            {
                Id = "Supervisor Id 1",
                Name = "Test Supervisor"
            };

            //When
            var createdId = await _repository.CreateAsync(Supervisor);

            var actualSupervisor = await _context.Supervisors.Where(p => p.Id == createdId).FirstOrDefaultAsync();

            //Then
            Assert.Equal(actualSupervisor.Id, createdId);
            Assert.Equal("Test Supervisor", actualSupervisor.Name);
            Assert.Null(actualSupervisor.projects);
        }


        [Fact]
        public async Task Read_given_valid_id_returns_correct_Supervisor()
        {
            //Given
            var Supervisor = await _repository.ReadAsync("1");

            //Then
            Assert.Equal("1", Supervisor.Id);
            Assert.Equal("Supervisor One Name", Supervisor.Name);
        }

        [Fact]
        public async Task Read_given_non_valid_id_returns_null()
        {
            //Given
            var Supervisor = await _repository.ReadAsync("133333");

            //Then
            Assert.Null(Supervisor);
        }

        [Fact]
        public async Task Read_returns_all_Supervisors()
        {
            //Given
            var Supervisors = await _repository.ReadAsync();

            //Then
            Assert.Collection(Supervisors,
                Supervisor => Assert.Equal(new SupervisorDTO("1", "Supervisor One Name"), Supervisor),
                Supervisor => Assert.Equal(new SupervisorDTO("2", "Supervisor Two Name"), Supervisor),
                Supervisor => Assert.Equal(new SupervisorDTO("3", "Supervisor Three Name"), Supervisor)
                );

        }

        [Fact]
        public async Task Update_given_non_valid_id_returns_NotFound()
        {
            //Given
            var Supervisor = new SupervisorUpdateDTO
            {
                Id = "122",
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
            var Supervisor = new SupervisorUpdateDTO
            {
                Id = "1",
                Name = "Supervisor Update Name"
            };

            //When
            var response = await _repository.UpdateAsync(Supervisor);

            var updatedProject = await _repository.ReadAsync("1");

            //Then
            Assert.Equal(OK, response);
            Assert.Equal("Supervisor Update Name", updatedProject.Name);
        }


        public void Dispose()
        {
            _context.Dispose();
        }
    }
}