using Macaria.API.Features.Notes;
using Macaria.API.Features.Tags;
using Macaria.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Macaria.Infrastructure.Extensions;
using Newtonsoft.Json;

namespace Macaria.API.Behaviors
{
    public class EntityChangedNotificationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IHubContext<Hub> _hubContext;
        private readonly IMacariaContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public EntityChangedNotificationBehavior(IHubContext<Hub> hubContext, IMacariaContext context, IHttpContextAccessor httpContextAccessor)
        {
            _hubContext = hubContext;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var response = await next();

            if (typeof(TResponse) == typeof(SaveNoteCommand.Request) || typeof(TResponse) == typeof(SaveTagCommand.Request))
                await SendSavedNotification(request, response);

            if (typeof(TResponse) == typeof(RemoveNoteCommand.Request) || typeof(TRequest) == typeof(RemoveNoteCommand.Request))
                await SendRemovedNotification(request);

            return response;           
        }

        public async Task SendSavedNotification(TRequest request, TResponse response)
        {
            var notification = default(object);

            if (request as SaveNoteCommand.Request != null)
            {
                var note = _context.Notes.FindAsync((response as SaveNoteCommand.Response).NoteId);

                notification = new
                {
                    Action = "[Note] Saved",
                    Payload = new { note }
                };
            }

            if (request as SaveTagCommand.Request != null)
            {
                var tag = _context.Tags.FindAsync((response as SaveTagCommand.Response).TagId);

                notification = new
                {
                    Action = "[Tag] Saved",
                    Payload = new { tag }
                };
            }

            await SendNotification(notification);
        }

        public async Task SendRemovedNotification(TRequest request)
        {            
            var notification = default(object);

            if (request as RemoveNoteCommand.Request != null)
            {
                notification = new
                {
                    Action = "[Note] Removed",
                    Payload = new { (request as RemoveNoteCommand.Request).NoteId }
                };
            }

            if (request as RemoveTagCommand.Request != null)
            {
                notification = new
                {
                    Action = "[Tag] Removed",
                    Payload = new { (request as RemoveTagCommand.Request).TagId }
                };
            }
            
            await SendNotification(notification);
        }

        public async Task SendNotification(object message) {

            var tenantId = _httpContextAccessor.HttpContext.Request.GetHeaderValue("TenantId");

            await _hubContext.Clients.Group(tenantId).SendAsync("message", JsonConvert.SerializeObject(message));
        }
    }
}
