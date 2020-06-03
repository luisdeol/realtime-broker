using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace RealTimeBroker.Web.Hubs
{
    public class BrokerHub : Hub
    {
        public Task ConnectToStock(string symbol)
        {
            Groups.AddToGroupAsync(Context.ConnectionId, symbol);

            return Task.CompletedTask;
        }
    }
}
