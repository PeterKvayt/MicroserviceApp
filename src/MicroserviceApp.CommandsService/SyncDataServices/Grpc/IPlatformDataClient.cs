using System.Collections;
using System.Collections.Generic;
using MicroserviceApp.CommandsService.Models;

namespace MicroserviceApp.CommandsService.SyncDataServices.Grpc;

public interface IPlatformDataClient
{
    IEnumerable<Platform> ReturnAllPlatforms();
}