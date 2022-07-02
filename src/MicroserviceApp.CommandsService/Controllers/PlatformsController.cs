using Microsoft.AspNetCore.Mvc;

namespace MicroserviceApp.CommandsService.Controllers;

[Route("api/c/[controller]")]
[ApiController]
public class PlatformsController : ControllerBase
{
    public PlatformsController()
    {

    }

    [HttpPost]
    public ActionResult TestInboundConnection()
    {
        Console.WriteLine("--> Inbound POST # Command service");
        return Ok("Inbounds test of from Platforms Controller");
    }
}