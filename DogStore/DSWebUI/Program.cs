using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DSWebUI
{
    public class Program
    {
        public static IConfiguration Config { get; set; }
        public static void Main(string[] args)
        {
            Config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", true,
                    true)
                .AddCommandLine(args)
                .AddEnvironmentVariables()
                .Build();

            // configure serilog
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Config)
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .CreateLogger();
            Log.Information("Initial build starting");
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                    .UseConfiguration(Config)
                    .UseSerilog();
                });
    }
}
