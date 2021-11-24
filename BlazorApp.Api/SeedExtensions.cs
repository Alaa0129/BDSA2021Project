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

                SeedUsers(context);
            }
            return host;
        }

        private static void SeedUsers(PBankContext context)
        {
            context.Database.Migrate();

            if (!context.Users.Any())
            {
                context.Users.AddRange(
                    new User { Firstname = "John", Lastname = "Smith", },
                    new User { Firstname = "Lars", Lastname = "Larsen", },
                    new User { Firstname = "Lenny", Lastname = "Erwin", }
                );

                context.SaveChanges();
            }

            if(!context.Projects.Any())
            {
                context.Projects.AddRange(
                    new Project{Title = "Project One", Description = "Project One Description", SupervisorId = 1, MaxApplications = 4, AppliedStudents = new User[] {context.Users.Find(1)}},
                    new Project{Title = "Project Two", Description = "Project Two Description", SupervisorId = 1, MaxApplications = 100},
                    new Project { Title = "Project Three", Description = "Project Three Description", SupervisorId = 1, MaxApplications = 100 },
                    new Project { Title = "Project Four", Description = "Project Four Description", SupervisorId = 1, MaxApplications = 100 },
                    new Project { Title = "Project Five", Description = "Project Five Description", SupervisorId = 1, MaxApplications = 100 }

                );

                context.SaveChanges();
            }
        }
    }
}