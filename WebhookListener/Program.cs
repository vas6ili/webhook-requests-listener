﻿using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace WebhookListener
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var httpUrls = Enumerable.Range(5000, 10).Select(port => $"http://*:{port}");
            var httpsUrls = Enumerable.Range(54430, 10).Select(port => $"https://*:{port}");

            CreateWebHostBuilder(args, httpUrls.Concat(httpsUrls).ToArray())
                .Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args, string[] urls) =>
            WebHost.CreateDefaultBuilder(args)
                .UseUrls(urls)
                .ConfigureLogging(logging => logging.SetMinimumLevel(LogLevel.Warning))
                .UseStartup<Startup>();
    }
}
