using System;
using System.Text.Json;
using AutoMapper;
using MicroserviceApp.CommandsService.Data;
using MicroserviceApp.CommandsService.Dtos;
using MicroserviceApp.CommandsService.Models;
using Microsoft.Extensions.DependencyInjection;

namespace MicroserviceApp.CommandsService.EventProcessing;

class EventProcessor : IEventProcessor
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IMapper _mapper;

    public EventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper)
    {
        _scopeFactory = scopeFactory;
        _mapper = mapper;
    }
    
    public void ProcessEvent(string message)
    {
        var eventType = DetermineEvent(message);

        switch (eventType)
        {
            case EventType.PlatformPublished:
                AddPlatform(message);
                break;
        }
    }

    private EventType DetermineEvent(string notificationMessage)
    {
        Console.WriteLine($"--> Determining event");

        var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);

        switch (eventType.Event)
        {
            case "Platform_Published":
                Console.WriteLine("--> Platform published event detected");
                return EventType.PlatformPublished;
            default:
                Console.WriteLine("--> Could not determine event type");
                return EventType.Undetermined;
        }
    }

    private void AddPlatform(string platformPublishedMessage)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var repo = scope.ServiceProvider.GetRequiredService<ICommandRepo>();

            var platformPublishedDto = JsonSerializer.Deserialize<PlatformPublishedDto>(platformPublishedMessage);

            try
            {
                var plat = _mapper.Map<Platform>(platformPublishedDto);

                if (repo.ExternalPlatformExists(plat.ExternalId))
                {
                    Console.WriteLine("--> Platform already exists");
                }
                else
                {
                    repo.CreatePlatform(plat);
                    repo.SaveChanges();
                    Console.WriteLine("--> Platform added");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"--> Could not add platform to db: {e.Message}");
            }
        }
    }
}