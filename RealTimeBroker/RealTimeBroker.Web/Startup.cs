using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RealTimeBroker.Web.HostedServices;
using RealTimeBroker.Web.Hubs;

namespace RealTimeBroker.Web
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddSignalR();

            services
                .AddHostedService<UpdateStockPriceHostedService>();

            services.AddCors(options =>
                {
                    options.AddPolicy("CorsPolicyForDashboard", builder => 
                        builder
                            .WithOrigins("http://localhost:4200")
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials());
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("CorsPolicyForDashboard");

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<BrokerHub>("/brokerhub");
            });
        }
    }
}
