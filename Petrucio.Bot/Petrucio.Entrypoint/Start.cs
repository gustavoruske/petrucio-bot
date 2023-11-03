﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Petrucio.Domain.Services;
using Petrucio.Domain.Extensions;
using Microsoft.Extensions.Configuration;

class Start
{
    // Program entry point
    static Task Main(string[] args)
    {
        IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddJsonFile("appsettings.Development.json")
            .AddEnvironmentVariables()
            .Build();

        var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices(services => {
                services.AddSingleton(config);
                services.AddDependencyInjection();
            })
            .Build();

        var entryPointService = host.Services.GetRequiredService<EntrypointService>();

        return entryPointService.Init();
    }
}