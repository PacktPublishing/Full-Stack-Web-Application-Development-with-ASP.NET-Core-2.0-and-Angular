using Macaria.API.Hubs;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System.Threading;
using System.Threading.Tasks;

namespace Macaria.API.Features.Tags
{
    public class TagRemovedEvent
    {
        public class DomainEvent : INotification
        {
            public DomainEvent(int tagId) => TagId = tagId;
            public int TagId { get; set; }
        }

        public class Handler : INotificationHandler<DomainEvent>
        {
            private readonly IHubContext<AppHub> _hubContext;

            public Handler(IHubContext<AppHub> hubContext)
                => _hubContext = hubContext;

            public async Task Handle(DomainEvent notification, CancellationToken cancellationToken) {
                await _hubContext.Clients.All.SendAsync("message", new {
                    Type = "[Tag] Removed",
                    Payload = new { tagId = notification.TagId }
                }, cancellationToken);
            }
        }
    }
}
