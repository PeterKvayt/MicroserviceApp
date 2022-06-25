using MicroserviceApp.PlatformService.Models;
using Microsoft.EntityFrameworkCore;

namespace MicroserviceApp.PlatformService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        public DbSet<Platform> Platforms { get; set; }
    }
}