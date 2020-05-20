using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CSC1451_TaskWorker.ASB;
using CSC1451_TaskWorker.Handlers;
using CSC1451_TaskWorker.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CSC1451_TaskWorker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).
                ConfigureAppConfiguration((context, builder) =>
                    builder.SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile($"appsettings.json", true, true)
                        .Build()
                )
                .ConfigureServices((hostContext, services) =>
                {
                    var settings = new PusherSettings();
                    hostContext.Configuration.Bind("Pusher", settings);
                    services.AddSingleton<PusherSettings>(settings);

                    var config = new Settings.Settings();
                    hostContext.Configuration.Bind("Configuration", config);
                    services.AddSingleton<Settings.Settings>(config);
                    // Event Log
                    services.AddSingleton<EventLog>();

                    // ServiceBus
                    services.AddSingleton<AddEventHandler>();
                    services.AddSingleton<ClearEventsHandler>();

                    services.AddSingleton<ServiceBusClient>();
                    services.AddHostedService<EndpointInitializer>();

                    services.AddHostedService<Worker>();
                });
    }
}
