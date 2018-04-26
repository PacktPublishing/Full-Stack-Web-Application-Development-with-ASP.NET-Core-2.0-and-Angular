using Macaria.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Macaria.API.Hubs
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class AppHub: Microsoft.AspNetCore.SignalR.Hub
    {
        private readonly IMacariaContext _context;

        public AppHub(IMacariaContext context) => _context = context;
        
        public async Task Send(string message) {                        
            await Clients.All.SendAsync("message", message);
        }
    }
}
