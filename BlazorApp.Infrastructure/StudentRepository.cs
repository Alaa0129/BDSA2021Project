using System;
using BlazorApp.Core;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using static System.Net.HttpStatusCode;

namespace BlazorApp.Infrastructure
{
    public class StudentRepository : IStudentRepository
    {

        private readonly IPBankContext _context;

        public StudentRepository(IPBankContext context)
        {
            _context = context;
        }

        //Creates a student and adds it to the database if it does not exist
        public async Task<string> CreateAsync(StudentCreateDTO student)
        {

            if (_context.Students.Find(student.Id) != null) throw new Exception("This Student already exists in the database");

            var entity = new Student
            {
                Id = student.Id,
                Name = student.Name
            };

            _context.Students.Add(entity);

            await _context.SaveChangesAsync();

            return entity.Id;
        }

        //Gets student details for the specified studentId

        public async Task<StudentDetailsDTO> ReadAsync(string studentId)
        {
            var students = from s in _context.Students
                           where s.Id == studentId
                           select new StudentDetailsDTO
                           (
                            s.Id,
                            s.Name,
                            s.Project == null ? null : new ProjectDTO(s.Project.Id, s.Project.Title, s.Project.Description, s.Project.Supervisor.Id),
                            s.Requests == null ? null : s.Requests.Select(s => new RequestDTO(s.Id, s.Title, s.Description, s.Student.Id, s.Supervisor.Id)).ToList()
                           );

            return await students.FirstOrDefaultAsync();
        }

        //Gets all students from the database
        public async Task<IReadOnlyCollection<StudentDTO>> ReadAsync() =>
            (await _context.Students
                            .Select(u => new StudentDTO(u.Id, u.Name))
                            .ToListAsync())
                            .AsReadOnly();

        //Updates a student if it exists in the database
        public async Task<HttpStatusCode> UpdateAsync(StudentUpdateDTO student)
        {
            var entity = await _context.Students.Where(u => u.Id == student.Id).FirstOrDefaultAsync();

            if (entity == null) return NotFound;

            entity.Name = student.Name;

            await _context.SaveChangesAsync();

            return HttpStatusCode.OK;
        }


        
        public async Task<HttpStatusCode> UpdateProjectAsync(string studentId, int projectId)
        {
            var studentEntity = await _context.Students.FindAsync(studentId);
            var projectEntity = await _context.Projects.FindAsync(projectId);

            if (projectEntity == null || studentEntity == null) return BadRequest;

            studentEntity.Project = projectEntity;

            await _context.SaveChangesAsync();
            return OK;
        }
    }
}