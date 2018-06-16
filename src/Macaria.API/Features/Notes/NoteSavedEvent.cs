using System.Threading;
using System.Threading.Tasks;
using Macaria.API.Hubs;
using Macaria.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Macaria.API.Features.Notes
{
    public class NoteSavedEvent
    {
       public class DomainEvent: INotification
        {
            public DomainEvent(Note note) => Note = note;
            
            public Note Note { get; set; }
        }

        public class Handler : INotificationHandler<DomainEvent>
        {
            private IHubContext<AppHub> _hubContext;

            public Handler(IHubContext<AppHub> hubContext)
                => _hubContext = hubContext;

            public async Task Handle(DomainEvent notification, CancellationToken cancellationToken)
            {
                await _hubContext.Clients.All.SendAsync("message", new
                {
                    Type = "[Note] Saved",
                    Payload = new { Note = NoteApiModel.FromNote(notification.Note) }
                }, cancellationToken);
            }
        }
    }
}
