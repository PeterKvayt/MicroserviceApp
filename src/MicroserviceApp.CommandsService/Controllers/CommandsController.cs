using System;
using System.Collections.Generic;
using AutoMapper;
using MicroserviceApp.CommandsService.Data;
using MicroserviceApp.CommandsService.Dtos;
using MicroserviceApp.CommandsService.Models;
using Microsoft.AspNetCore.Mvc;

namespace MicroserviceApp.CommandsService.Controllers;

[Route("api/c/platforms/{platformId}/[controller]")]
[ApiController]
public class CommandsController : ControllerBase
{
    private readonly ICommandRepo _commandRepo;
    private readonly IMapper _mapper;

    public CommandsController(ICommandRepo commandRepo, IMapper mapper)
    {
        _commandRepo = commandRepo;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult<IEnumerable<CommandReadDto>> GetCommandsForPlatform(int platformId)
    {
        Console.WriteLine($"--> Hit GetCommandsForPlatform: {platformId}");
        
        if (_commandRepo.PlatformExists(platformId))
        {
            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(_commandRepo.GetCommandsForPlatform(platformId)));
        }

        return NotFound();
    }
    
    [HttpGet("{commandId}", Name = "GetCommandForPlatform")]
    public ActionResult<IEnumerable<CommandReadDto>> GetCommandForPlatform(int platformId, int commandId)
    {
        Console.WriteLine($"--> Hit GetCommandForPlatform: {platformId} / {commandId}");

        if (!_commandRepo.PlatformExists(platformId)) 
            return NotFound();

        var command = _commandRepo.GetCommand(platformId, commandId);
        
        if (command is null) 
            return NotFound();

        return Ok(_mapper.Map<CommandReadDto>(command));
    }
    
    [HttpPost]
    public ActionResult<CommandReadDto> CreateCommandForPlatform(int platformId, CommandCreateDto commandDto)
    {
        Console.WriteLine($"--> Hit CreateCommandForPlatform: {platformId}");

        if (!_commandRepo.PlatformExists(platformId))
            return NotFound();

        var command = _mapper.Map<Command>(commandDto);
        
        _commandRepo.CreateCommand(platformId, command);
        _commandRepo.SaveChanges();

        var commandReadDto = _mapper.Map<CommandReadDto>(command);

        return CreatedAtRoute(nameof(GetCommandForPlatform), new { platformId, commandId = commandReadDto.Id }, commandReadDto);

    }
}