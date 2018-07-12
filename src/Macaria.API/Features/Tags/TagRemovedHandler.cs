using Macaria.Core;
using Macaria.Core.ApiModels;
using Macaria.Core.DomainEvents;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System.Threading;
using System.Threading.Tasks;

namespace Macaria.API.Features.Tags
{
    public class TagRemovedHandler : INotificationHandler<TagRemoved>
    {
        private readonly IHubContext<IntegrationEventsHub> _hubContext;

        public TagRemovedHandler(IHubContext<IntegrationEventsHub> hubContext)
            => _hubContext = hubContext;

        public async Task Handle(TagRemoved @event, CancellationToken cancellationToken)
        {
            await _hubContext.Clients.All
            .SendAsync("events",
            new
            {
                type = @event.EventType,
                payload = new { tag = @event.Payload }
            }
            , cancellationToken);
        }
    }
}
