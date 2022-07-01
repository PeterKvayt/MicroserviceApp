using System;
using System.Collections.Generic;
using AutoMapper;
using MicroserviceApp.PlatformService.Data;
using MicroserviceApp.PlatformService.Dtos;
using MicroserviceApp.PlatformService.Models;
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
        public ActionResult<PlatformReadDto> CreatePlatform(PlatformCreateDto platform)
        {
            var model = _mapper.Map<Platform>(platform);
            _platformRepo.Create(model);
            _platformRepo.SaveChanges();

            var platformResponse = _mapper.Map<PlatformReadDto>(model);

            return CreatedAtRoute(nameof(GetPlatformById), new {Id = platformResponse.Id}, platformResponse);
        }
    }
}