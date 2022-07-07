using System.Collections.Generic;
using MicroserviceApp.CommandsService.Models;

namespace MicroserviceApp.CommandsService.Data;

public interface ICommandRepo
{
    bool SaveChanges();

    #region Platforms

    IEnumerable<Platform> GetAllPlatforms();

    void CreatePlatform(Platform platform);

    bool PlatformExists(int platformId);
    
    bool ExternalPlatformExists(int externalPlatformId);

    #endregion
    
    #region Commands

    IEnumerable<Command> GetCommandsForPlatform(int platformId);

    Command GetCommand(int platformId, int commandId);

    void CreateCommand(int platformId, Command command);

    #endregion
    
}