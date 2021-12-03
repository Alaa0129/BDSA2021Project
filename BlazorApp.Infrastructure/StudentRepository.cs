using System;
using BlazorApp.Core;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using System.Net;

namespace BlazorApp.Infrastructure
{
    public class StudentRepository : IStudentRepository
    {

        private readonly IPBankContext _context;

        public StudentRepository(IPBankContext context)
        {
            _context = context;
        }

        public async Task<string> CreateAsync(StudentCreateDTO student)
        {
            var entity = new Student
            {
                Id = student.Id,
                Name = student.Name
            };

            _context.Students.Add(entity);

            await _context.SaveChangesAsync();

            return entity.Id;
        }

        public async Task<StudentDetailsDTO> ReadAsync(string studentId)
        {
            var students = from s in _context.Students
                           where s.Id == studentId
                           select new StudentDetailsDTO
                           (
                            s.Id,
                            s.Name,
                            s.project == null ? null : new ProjectDetailsDTO(s.project.Id, s.project.Title, s.project.Description, s.project.SupervisorId, s.project.MaxApplications)
                           );

            var studente = await students.FirstOrDefaultAsync();

            return studente;
        }

        public async Task<IReadOnlyCollection<StudentDTO>> ReadAsync() =>
            (await _context.Students
                            .Select(u => new StudentDTO(u.Id, u.Name))
                            .ToListAsync())
                            .AsReadOnly();

        public async Task<HttpStatusCode> UpdateAsync(StudentUpdateDTO student)
        {
            var entity = await _context.Students.Where(u => u.Id == student.Id).FirstOrDefaultAsync();

            if (entity == null) return HttpStatusCode.NotFound;

            entity.Name = student.Name;

            await _context.SaveChangesAsync();

            return HttpStatusCode.OK;
        }
    }
}