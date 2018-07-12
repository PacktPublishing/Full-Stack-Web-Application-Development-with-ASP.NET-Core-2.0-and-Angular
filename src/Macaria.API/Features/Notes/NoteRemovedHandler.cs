using Macaria.Core;
using Macaria.Core.ApiModels;
using Macaria.Core.DomainEvents;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System.Threading;
using System.Threading.Tasks;

namespace Macaria.API.Features.Notes
{
    public class NoteRemovedHandler : INotificationHandler<NoteRemoved>
    {
        private readonly IHubContext<IntegrationEventsHub> _hubContext;

        public NoteRemovedHandler(IHubContext<IntegrationEventsHub> hubContext)
            => _hubContext = hubContext;

        public async Task Handle(NoteRemoved @event, CancellationToken cancellationToken)
        {
            await _hubContext.Clients.All
            .SendAsync("events",
            new
            {
                type = @event.EventType,
                payload = new { note = @event.Payload }
            }
            , cancellationToken);
        }
    }
}
