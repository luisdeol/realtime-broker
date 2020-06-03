using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RealTimeBroker.Web.Hubs;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RealTimeBroker.Web.HostedServices
{
    public class UpdateStockPriceHostedService : IHostedService, IDisposable
    {
        private Timer _timer;
        public IServiceProvider Services { get; }
        public UpdateStockPriceHostedService(IServiceProvider services)
        {
            Services = services;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(UpdatePrices, null, 0, 5000);

            return Task.CompletedTask;
        }

        private void UpdatePrices(object state)
        {
            using (var scope = Services.CreateScope())
            {
                var hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<BrokerHub>>();

                var randomDouble = new Random().NextDouble();

                hubContext.Clients.Group("ITSA4").SendAsync("UpdatePrice", randomDouble);
            }
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }
    }
}
