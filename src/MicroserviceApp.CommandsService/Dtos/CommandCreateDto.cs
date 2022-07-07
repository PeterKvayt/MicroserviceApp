using System.ComponentModel.DataAnnotations;

namespace MicroserviceApp.CommandsService.Dtos;

public class CommandCreateDto
{
    [Required]
    public string HowTo { get; set; }
    [Required]
    public string CommandLine { get; set; }
}