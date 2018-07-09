using System.Linq;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace WebhookListener
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var urls = Enumerable.Range(5000, 20).Select(port => $"http://*:{port}").ToArray();
            CreateWebHostBuilder(args, urls).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args, string[] urls) =>
            WebHost.CreateDefaultBuilder(args)
                .UseUrls(urls)
                .UseStartup<Startup>();
    }
}
