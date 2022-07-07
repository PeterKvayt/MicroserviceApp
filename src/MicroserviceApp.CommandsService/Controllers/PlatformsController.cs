using System;
using System.Collections.Generic;
using AutoMapper;
using MicroserviceApp.CommandsService.Data;
using MicroserviceApp.CommandsService.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace MicroserviceApp.CommandsService.Controllers;

[Route("api/c/[controller]")]
[ApiController]
public class PlatformsController : ControllerBase
{
    private readonly ICommandRepo _commandRepo;
    private readonly IMapper _mapper;

    public PlatformsController(ICommandRepo commandRepo, IMapper mapper)
    {
        _commandRepo = commandRepo;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
    {
        Console.WriteLine("--> Getting all platforms from Command Service");
        var platforms = _commandRepo.GetAllPlatforms();
        return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platforms));
    }
    
    [HttpPost]
    public ActionResult TestInboundConnection()
    {
        Console.WriteLine("--> Inbound POST # Command service");
        return Ok("Inbounds test of from Platforms Controller");
    }
}