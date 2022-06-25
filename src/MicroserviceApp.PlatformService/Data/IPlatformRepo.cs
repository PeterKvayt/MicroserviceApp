using System.Collections.Generic;
using MicroserviceApp.PlatformService.Models;

namespace MicroserviceApp.PlatformService.Data
{
    public interface IPlatformRepo
    {
        bool SaveChanges();

        IEnumerable<Platform> GetAll();
        Platform GetById(int id);
        void Create(Platform platform);
    }
}