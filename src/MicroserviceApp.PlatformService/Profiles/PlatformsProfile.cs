using AutoMapper;
using MicroserviceApp.PlatformService.Dtos;
using MicroserviceApp.PlatformService.Models;

namespace MicroserviceApp.PlatformService.Profiles
{
    public class PlatformsProfile : Profile
    {
        public PlatformsProfile()
        {
            CreateMap<Platform, PlatformReadDto>();
            CreateMap<PlatformCreateDto, Platform>();
        }
    }
}