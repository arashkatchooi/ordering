using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using OrderCommon;
using System;
using Microsoft.Extensions.DependencyInjection;
using Order.Common.Models;
using Microsoft.Extensions.Configuration;

namespace Supervisor
{
    public class Program
    {
        private static IConfiguration Configuration { get; } = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", true, true)
            .Build();
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.ConfigureServices((context, services) =>
                    {
                        services.AddSingleton<QueueService, QueueService>();
                        services.Configure<AppSettings>(Configuration);
                        services.AddDistributedMemoryCache();
                        services.AddSession(options =>
                        {
                            options.Cookie.Name = ".Orders.Session";
                            options.IdleTimeout = TimeSpan.FromSeconds(120);
                            options.Cookie.IsEssential = true;
                        });
                    });
                });
          };
}
