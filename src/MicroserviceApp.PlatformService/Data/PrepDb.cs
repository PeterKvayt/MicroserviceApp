using System;
using System.Linq;
using MicroserviceApp.PlatformService.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MicroserviceApp.PlatformService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder applicationBuilder, IWebHostEnvironment environment)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), environment);
            }
        }

        private static void SeedData(AppDbContext context, IWebHostEnvironment environment)
        {
            if (environment.IsProduction())
            {
                Console.WriteLine("--> We attempting to apply migrations ...");
                try
                {
                    context.Database.Migrate();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"--> We could not apply migrations: {e.Message}");
                }
            }
            
            if (context.Platforms.Any())
            {
                Console.WriteLine("--> We already have data");
                return;
            }
            
            Console.WriteLine("--> Seeding data ...");
            
            context.Platforms.AddRange(
                new Platform(){Name = "Dot Net", Publisher = "Microsoft", Cost = "Free"},
                new Platform(){Name = "SQL Server Express", Publisher = "Microsoft", Cost = "Free"},
                new Platform(){Name = "Kubernetes", Publisher = "Cloud Native Computing Foundation", Cost = "Free"}
                );

            context.SaveChanges();
        }
    }
}