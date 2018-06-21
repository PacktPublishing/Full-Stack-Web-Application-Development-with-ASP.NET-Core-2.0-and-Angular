using Macaria.Core.Models;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System.Threading;
using System.Threading.Tasks;

namespace Macaria.API.Features.Notes
{
    public class NoteSavedEvent
    {
        public class DomainEvent : INotification
        {
            public DomainEvent(Note note) => Note = note;
            public Note Note { get; set; }
        }

        public class Handler : INotificationHandler<DomainEvent>
        {
            private readonly IHubContext<IntegrationEventsHub> _hubContext;

            public Handler(IHubContext<IntegrationEventsHub> hubContext)
                => _hubContext = hubContext;

            public async Task Handle(DomainEvent notification, CancellationToken cancellationToken) {
                await _hubContext.Clients.All.SendAsync("message", new {
                    Type = "[Note] Saved",
                    Payload = new { Note = NoteApiModel.FromNote(notification.Note) }
                }, cancellationToken);
            }
        }
    }
}
