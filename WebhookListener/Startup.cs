using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IO;

namespace WebhookListener
{
    public class Startup
    {       
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            services.AddMvcCore()
                    .AddJsonFormatters();

            services.AddSignalR()
                    .AddJsonProtocol();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseDefaultFiles();
            app.UseFileServer();

            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseSignalR(builder =>
            {
                builder.MapHub<EventsHub>("/events");
            });

            app.UseMvc();
        }
    }
}
