using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RealTimeBroker.Web.Hubs;
using RealTimeBroker.Web.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RealTimeBroker.Web.HostedServices
{
    public class UpdateStockPriceHostedService : IHostedService, IDisposable
    {
        private Timer _timer;
        public IServiceProvider Services { get; }
        private readonly List<string> _stocks;
        public UpdateStockPriceHostedService(IServiceProvider services)
        {
            Services = services;
            _stocks = new List<string>
            {
                "ITSA4",
                "TAEE11",
                "PETR4"
            };
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(UpdatePrices, null, 0, 2000);

            return Task.CompletedTask;
        }

        private void UpdatePrices(object state)
        {
            using (var scope = Services.CreateScope())
            {
                // Obtenho a instância do IHubContext, para permitir interagir com os Hubs e as conexões dos grupos.
                var hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<BrokerHub>>();

                // Para cada ação da lista eu gero um número aleatório entre 5 e 30, e então notifico o grupo do Hub dessa ação sobre o novo objeto que contém o valor.
                foreach(var stock in _stocks)
                {
                    var stockPrice = GetRandomNumber(5, 30);

                    hubContext.Clients.Group(stock).SendAsync("UpdatePrice", new StockPrice(stock, stockPrice));
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        private double GetRandomNumber(double minimum, double maximum)
        {
            var random = new Random();
            return random.NextDouble() * (maximum - minimum) + minimum;
        }
    }
}
