using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Macaria.API.Features
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class IntegrationEventsHub: Hub { }
}
