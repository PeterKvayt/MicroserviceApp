using MicroserviceApp.PlatformService.Dtos;

namespace MicroserviceApp.PlatformService.AsyncDataServices;

public interface IMessageBusClient
{
    void PublishNewPlatform(PlatformPublishedDto platformPublishedDto);
}