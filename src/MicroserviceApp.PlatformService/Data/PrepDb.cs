using System;
using System.Linq;
using MicroserviceApp.PlatformService.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace MicroserviceApp.PlatformService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>());
            }
        }

        private static void SeedData(AppDbContext contex)
        {
            if (contex.Platforms.Any())
            {
                Console.WriteLine("--> We already have data");
                return;
            }
            
            Console.WriteLine("--> Seeding data ...");
            
            contex.Platforms.AddRange(
                new Platform(){Name = "Dot Net", Publisher = "Microsoft", Cost = "Free"},
                new Platform(){Name = "SQL Server Express", Publisher = "Microsoft", Cost = "Free"},
                new Platform(){Name = "Kubernetes", Publisher = "Cloud Native Computing Foundation", Cost = "Free"}
                );

            contex.SaveChanges();
        }
    }
}