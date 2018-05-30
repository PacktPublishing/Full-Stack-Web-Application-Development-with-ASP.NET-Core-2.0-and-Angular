using Macaria.API.Features.Notes;
using Macaria.API.Features.Tags;
using Macaria.API.Hubs;
using Macaria.Core.Interfaces;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System.Threading;
using System.Threading.Tasks;

namespace Macaria.API.Behaviors
{
    public class EntityChangedNotificationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        
    {
        private readonly IHubContext<AppHub> _hubContext;
        private readonly IAppDbContext _context;

        public EntityChangedNotificationBehavior(IHubContext<AppHub> hubContext, IAppDbContext context)
        {
            _hubContext = hubContext;
            _context = context;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var response = await next();

            if (request is SaveNoteCommand.Request)
                return await HandleSaveNoteCommand(request, response);

            if (request is RemoveNoteCommand.Request)
                return await HandleRemoveNoteCommand(request,  response);

            if (request is SaveTagCommand.Request)
                return await HandleSaveTagCommand(request,  response);

            if (request is RemoveTagCommand.Request)
                return await HandleRemoveTagCommand(request,  response);

            return response;
        }

        private async Task<dynamic> HandleSaveNoteCommand(dynamic request, dynamic response)
        {
            var note = await _context.Notes.FindAsync(response.NoteId);

            await _hubContext.Clients.All.SendAsync("message", new
            {
                Type = "[Note] Saved",
                Payload = new { note = NoteApiModel.FromNote(note) }
            });

            return response;
        }

        private async Task<dynamic> HandleRemoveNoteCommand(dynamic request, dynamic response)
        {
            await _hubContext.Clients.All.SendAsync("message", new
            {
                Type = "[Note] Removed",
                Payload = new { noteId = request.NoteId }
            });

            return response;
        }

        private async Task<dynamic> HandleSaveTagCommand(dynamic request, dynamic response)
        {
            var tag = await _context.Tags.FindAsync(response.TagId);

            await _hubContext.Clients.All.SendAsync("message", new
            {
                Type = "[Tag] Saved",
                Payload = new { tag = TagApiModel.FromTag(tag) }
            });

            return response;
        }

        private async Task<dynamic> HandleRemoveTagCommand(dynamic request, dynamic response)
        {
            await _hubContext.Clients.All.SendAsync("message", new
            {
                Type = "[Tag] Removed",
                Payload = new { tagId = request.TagId }
            });

            return response;
        }
    } 
}
