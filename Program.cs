using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;

namespace Project
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder().
                AddJsonFile($"appsettings.json", optional: false, reloadOnChange: true).Build();

            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(config).CreateLogger();
            try
            {
                Log.Information("Application starting");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {

                Log.Fatal(ex, "Failed to start");
            }
            finally
            {
                Log.CloseAndFlush();
            }
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
