using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Macaria.API.Hubs
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class AppHub: Hub
    {        
        public async Task Send(string message) {                        
            await Clients.All.SendAsync("message", message);
        }
    }
}
