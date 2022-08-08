using System;
using System.IO;
using MicroserviceApp.PlatformService.AsyncDataServices;
using MicroserviceApp.PlatformService.Data;
using MicroserviceApp.PlatformService.SyncDataServices.Grpc;
using MicroserviceApp.PlatformService.SyncDataServices.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace MicroserviceApp.PlatformService
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                if (_env.IsProduction())
                {
                    Console.WriteLine($"--> Using SQL DB");
                    options.UseSqlServer(_configuration.GetConnectionString("PlatformsConn"));
                }
                else
                {
                    Console.WriteLine($"--> Using InMem DB");
                    options.UseInMemoryDatabase("InMem");
                }
            });

            services.AddGrpc();
            services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();
            services.AddSingleton<IMessageBusClient, MessageBusClient>();

            services.AddScoped<IPlatformRepo, PlatformRepo>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "PlatformService", Version = "1"});
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            if (_env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PlatformService"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGrpcService<GrpcPlatformService>();

                endpoints.MapGet("/protos/platforms.proto", async context =>
                {
                    await context.Response.WriteAsync((File.ReadAllText("Protos/platforms.proto")));
                });
            });

            Console.WriteLine($"--> Environment is {_env.EnvironmentName}");
            Console.WriteLine($"--> CommandService url is {_configuration["CommandService"]}");
            
            PrepDb.PrepPopulation(app, _env);
        }
    }
}