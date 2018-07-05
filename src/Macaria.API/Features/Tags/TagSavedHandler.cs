using Macaria.Core;
using Macaria.Core.DomainEvents;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System.Threading;
using System.Threading.Tasks;

namespace Macaria.API.Features.Tags
{
    public class TagSavedHandler : INotificationHandler<TagSaved>
    {
        private readonly IHubContext<IntegrationEventsHub> _hubContext;

        public TagSavedHandler(IHubContext<IntegrationEventsHub> hubContext)
            => _hubContext = hubContext;

        public async Task Handle(TagSaved @event, CancellationToken cancellationToken)
        {
            await _hubContext.Clients.All
            .SendAsync("events",
            new
            {
                type = @event.EventType,
                payload = new { tag = TagApiModel.FromTag(@event.Payload) }
            }
            , cancellationToken);
        }
    }
}
