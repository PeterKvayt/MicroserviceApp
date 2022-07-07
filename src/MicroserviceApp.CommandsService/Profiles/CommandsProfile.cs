using AutoMapper;
using MicroserviceApp.CommandsService.Dtos;
using MicroserviceApp.CommandsService.Models;

namespace MicroserviceApp.CommandsService.Profiles;

public class CommandsProfile : Profile
{
    public CommandsProfile()
    {
        CreateMap<Platform, PlatformReadDto>();
        CreateMap<CommandCreateDto, Command>();
        CreateMap<Command, CommandReadDto>();
        CreateMap<PlatformPublishedDto, Platform>()
            .ForMember(target => target.ExternalId, opt => opt.MapFrom(src => src.Id))
            ;
    }
}