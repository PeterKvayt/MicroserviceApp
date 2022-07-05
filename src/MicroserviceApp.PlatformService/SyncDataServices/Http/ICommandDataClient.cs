using System.Threading.Tasks;
using MicroserviceApp.PlatformService.Dtos;

namespace MicroserviceApp.PlatformService.SyncDataServices.Http;

public interface ICommandDataClient
{
    Task SendPlatformToCommand(PlatformReadDto platform);
}