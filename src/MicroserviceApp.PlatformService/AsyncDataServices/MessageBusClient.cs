using System;
using System.Text;
using System.Text.Json;
using MicroserviceApp.PlatformService.Dtos;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace MicroserviceApp.PlatformService.AsyncDataServices;

class MessageBusClient : IMessageBusClient
{
    private readonly IConnection _connection;
    private readonly IModel _channel;

    private const string ExchangeName = "trigger";
    
    public MessageBusClient(IConfiguration configuration)
    {
        var factory = new ConnectionFactory
        {
            HostName = configuration["RabbitMQHost"],
            Port = int.Parse(configuration["RabbitMQPort"])
        };

        try
        {
            _connection = factory.CreateConnection();
            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;

            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(ExchangeName, ExchangeType.Fanout);
            
            Console.WriteLine("--> Connected to the Message bus");
        }
        catch (Exception e)
        {
            Console.WriteLine($"--> Could not connect to message bus: {e.Message}");
        }
    }

    private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
    {
        Console.WriteLine("--> RabbitMQ connection shutdown");
    }

    public void PublishNewPlatform(PlatformPublishedDto platformPublishedDto)
    {
        var message = JsonSerializer.Serialize(platformPublishedDto);

        if (_channel.IsClosed)
        {
            Console.WriteLine("--> Rabbitmq connection closed, not sending message");
            return;
        }

        Console.WriteLine("--> Rabbitmq connection open, sending message ...");
        SendMessage(message);
    }

    private void SendMessage(string message)
    {
        var body = Encoding.UTF8.GetBytes(message);
        _channel.BasicPublish(ExchangeName, string.Empty, null, body);
        Console.WriteLine($"We have sent {message}");
    }

    public void Dispose()
    {
        Console.WriteLine("--> Message bus disposed");
        
        if (_channel.IsClosed) return;
        
        _channel.Close();
        _connection.Close();
    }
}