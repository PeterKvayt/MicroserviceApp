namespace MicroserviceApp.CommandsService.EventProcessing;

public interface IEventProcessor
{
    void ProcessEvent(string message);
}