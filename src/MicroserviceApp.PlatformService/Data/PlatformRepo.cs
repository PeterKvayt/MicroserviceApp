using System;
using System.Collections.Generic;
using System.Linq;
using MicroserviceApp.PlatformService.Models;

namespace MicroserviceApp.PlatformService.Data
{
    public class PlatformRepo : IPlatformRepo
    {
        private readonly AppDbContext _context;

        public PlatformRepo(AppDbContext context)
        {
            _context = context;
        }
        
        public bool SaveChanges()
        {
            return _context.SaveChanges() >= 0;
        }

        public IEnumerable<Platform> GetAll()
        {
            return _context.Platforms.ToList();
        }

        public Platform GetById(int id)
        {
            return _context.Platforms.FirstOrDefault(p => p.Id == id);
        }

        public void Create(Platform platform)
        {
            if (platform is null)
            {
                throw new ArgumentNullException(nameof(platform));
            }

            _context.Platforms.Add(platform);
        }
    }
}