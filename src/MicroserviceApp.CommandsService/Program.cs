using System;
using MicroserviceApp.CommandsService.AsyncDataServices;
using MicroserviceApp.CommandsService.Data;
using MicroserviceApp.CommandsService.EventProcessing;
using MicroserviceApp.CommandsService.SyncDataServices.Grpc;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services
    .AddHostedService<MessageBusSubscriber>()
    .AddSingleton<IEventProcessor, EventProcessor>()
    .AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("InMem"))
    .AddScoped<ICommandRepo, CommandRepo>()
    .AddScoped<IPlatformDataClient, PlatformDataClient>()
    .AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies())
    .AddEndpointsApiExplorer()
    .AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

PrepDb.PrepPopulation(app);

app.Run();

