using System;
using System.Linq;
using BlazorApp.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

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

            if (!context.Students.Any())
            {
                context.Students.AddRange(
                    new Student { Id = "Id1", Name = "John" },
                    new Student { Id = "Id2", Name = "Lars" },
                    new Student { Id = "Id3", Name = "Lenny" }
                );

                context.SaveChanges();
            }

            if(!context.Projects.Any())
            {
                context.Projects.AddRange(
                    new Project{Title = "Project One", Description = "Project One Description", SupervisorId = 1, MaxApplications = 4, AppliedStudents = new Student[] {context.Students.Find("Id1")}},
                    new Project{Title = "Project Two", Description = "Project Two Description", SupervisorId = 1, MaxApplications = 100}
                );

                context.SaveChanges();
            }
        }
    }
}