using Macaria.API.Hubs;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System.Threading;
using System.Threading.Tasks;

namespace Macaria.API.Features.Notes
{
    public class NoteRemovedEvent
    {
        public class DomainEvent : INotification
        {
            public DomainEvent(int noteId) => NoteId = noteId;
            public int NoteId { get; set; }
        }

        public class Handler : INotificationHandler<DomainEvent>
        {
            private readonly IHubContext<AppHub> _hubContext;

            public Handler(IHubContext<AppHub> hubContext)
                => _hubContext = hubContext;

            public async Task Handle(DomainEvent notification, CancellationToken cancellationToken) {
                await _hubContext.Clients.All.SendAsync("message", new {
                    Type = "[Note] Removed",
                    Payload = new { notification.NoteId }
                }, cancellationToken);
            }
        }
    }
}
