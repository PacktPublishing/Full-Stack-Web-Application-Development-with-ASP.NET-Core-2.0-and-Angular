using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Macaria.API.Hubs
{
    public class AppHub: Hub
    {
        public async Task Semd(object message)
            => await Clients.All.SendAsync("message", message);
    }
}
