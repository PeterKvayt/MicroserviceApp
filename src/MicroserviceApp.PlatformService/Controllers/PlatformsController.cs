using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MicroserviceApp.PlatformService.AsyncDataServices;
using MicroserviceApp.PlatformService.Data;
using MicroserviceApp.PlatformService.Dtos;
using MicroserviceApp.PlatformService.Models;
using MicroserviceApp.PlatformService.SyncDataServices.Http;
using Microsoft.AspNetCore.Mvc;

namespace MicroserviceApp.PlatformService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IPlatformRepo _platformRepo;

        public PlatformsController(IPlatformRepo platformRepo, IMapper mapper)
        {
            _platformRepo = platformRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            Console.WriteLine("Getting platforms ...");

            var platformItem = _platformRepo.GetAll();
            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platformItem));
        }

        [HttpGet("{id}", Name = "GetPlatformById")]
        public ActionResult<PlatformReadDto> GetPlatformById(int id)
        {
            Console.WriteLine("Getting platform ...");

            var platformItem = _platformRepo.GetById(id);
            if (platformItem is null) return NotFound();
            return Ok(_mapper.Map<PlatformReadDto>(platformItem));
        }

        [HttpPost]
        public async Task<ActionResult<PlatformReadDto>> CreatePlatform(PlatformCreateDto platform, [FromServices] ICommandDataClient commandDataClient, [FromServices] IMessageBusClient messageBusClient)
        {
            var model = _mapper.Map<Platform>(platform);
            _platformRepo.Create(model);
            _platformRepo.SaveChanges();

            var platformResponse = _mapper.Map<PlatformReadDto>(model);

            // Send sync message
            try
            {
                await commandDataClient.SendPlatformToCommand(platformResponse);
            }
            catch (Exception e)
            {
                Console.WriteLine($"--> Could not send sync: {e.Message}");
            }
            
            //Send async message
            try
            {
                var platformPublishedDto = _mapper.Map<PlatformPublishedDto>(platformResponse);
                platformPublishedDto.Event = "Platform_Published";
                messageBusClient.PublishNewPlatform(platformPublishedDto);
            }
            catch (Exception e)
            {
                Console.WriteLine($"--> Could not send async: {e.Message}");
            }
            
            return CreatedAtRoute(nameof(GetPlatformById), new {Id = platformResponse.Id}, platformResponse);
        }
    }
}