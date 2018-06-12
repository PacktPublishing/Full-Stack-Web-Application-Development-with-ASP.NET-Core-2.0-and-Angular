using Macaria.API.Hubs;
using Macaria.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System.Threading;
using System.Threading.Tasks;

namespace Macaria.API.Features.Tags
{
    public class TagSavedEvent
    {
        public class DomainEvent : INotification
        {
            public DomainEvent(Tag tag) => Tag = tag;
            public Tag Tag { get; set; }
        }

        public class Handler : INotificationHandler<DomainEvent>
        {
            private readonly IHubContext<AppHub> _hubContext;

            public Handler(IHubContext<AppHub> hubContext)
                => _hubContext = hubContext;

            public async Task Handle(DomainEvent notification, CancellationToken cancellationToken) {
                await _hubContext.Clients.All.SendAsync("message", new {
                    Type = "[Tag] Saved",
                    Payload = new { Tag = TagApiModel.FromTag(notification.Tag) }
                }, cancellationToken);
            }
        }
    }
}
