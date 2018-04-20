using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Macaria.API.Hubs
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class Hub: Microsoft.AspNetCore.SignalR.Hub
    {
        public Task Send(string message)
        {
            return Clients.All.SendAsync("Send", message);
        }
    }
}
