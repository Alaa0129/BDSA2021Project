using System.Linq;
using BlazorApp.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BlazorApp.Api
{
    public static class SeedExtensions
    {
        public static IHost Seed(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<PBankContext>();

                SeedContext(context);
            }
            return host;
        }

        private static void SeedContext(PBankContext context)
        {
            context.Database.Migrate();





            // if (!context.Projects.Any())
            // {
            //     var supervisor1 = new Supervisor { Id = "SupervisorId1", Name = "Supervisor One Name" };
            //     var supervisor2 = new Supervisor { Id = "SupervisorId2", Name = "Supervisor Two Name" };

            //     var student1 = new Student { Id = "StudentId1", Name = "Student One Name" };
            //     var student2 = new Student { Id = "StudentId2", Name = "Student Two Name" };
            //     var student3 = new Student { Id = "StudentId3", Name = "Student Three Name" };

            //     var tag1 = new Tag("First Tag");
            //     var tag2 = new Tag("Second Tag");
            //     var tag3 = new Tag("Third Tag");

            //     context.Projects.AddRange(
            //         new Project { Id = 1, Title = "Project One", Description = "This is the first project", Supervisor = supervisor1, AppliedStudents = new[] { student1, student2 }, Tags = new[] { tag1, tag2 } },
            //         new Project { Id = 2, Title = "Project Two", Description = "This is the second project", Supervisor = supervisor2, AppliedStudents = new[] { student3 }, Tags = new[] { tag3 } }
            //     );

            //     context.Requests.AddRange(
            //         new Request { Id = 1, Title = "Request One", Description = "Description One", Student = student1, Supervisor = supervisor1 },
            //         new Request { Id = 2, Title = "Request Two", Description = "Description Two", Student = student2, Supervisor = supervisor2 }
            //     );

            //     context.SaveChangesAsync();
            // }

            if (!context.Students.Any())
            {
                context.Students.AddRange(
                    new Student { Id = "StudentId1", Name = "John" },
                    new Student { Id = "StudentId2", Name = "Lars" },
                    new Student { Id = "StudentId3", Name = "Lenny" }
                );

                context.SaveChanges();
            }

            if (!context.Supervisors.Any())
            {
                context.Supervisors.AddRange(
                    new Supervisor { Id = "SupervisorId1", Name = "Supervisor John" },
                    new Supervisor { Id = "SupervisorId2", Name = "Supervisor Lars" },
                    new Supervisor { Id = "SupervisorId3", Name = "Supervisor Lenny" }
                );

                context.SaveChanges();
            }

            if (!context.Projects.Any())
            {

                var tag1 = new Tag("First Tag");
                var tag2 = new Tag("Second Tag");
                var tag3 = new Tag("Third Tag");

                context.Projects.AddRange(
                    new Project { Title = "Project One", Description = "Project One Description", Supervisor = context.Supervisors.Find("SupervisorId1"), AppliedStudents = new Student[] { context.Students.Find("StudentId1"), context.Students.Find("StudentId2") }, Tags = new[] { tag1, tag2 } },
                    new Project { Title = "Project Two", Description = "Project Two Description", Supervisor = context.Supervisors.Find("SupervisorId2"), AppliedStudents = new Student[] { context.Students.Find("StudentId3") }, Tags = new[] { tag3 } }
                );

                context.SaveChanges();
            }

            if (!context.Requests.Any())
            {
                context.Requests.AddRange(
                    new Request { Title = "Request One", Description = "Request One Description", Student = context.Students.Find("StudentId1"), Supervisor = context.Supervisors.Find("SupervisorId1")},
                    new Request { Title = "Request Two", Description = "Request Two Description", Student = context.Students.Find("StudentId1"), Supervisor = context.Supervisors.Find("SupervisorId2")}
                );

                context.SaveChanges();
            }
        }
    }
}