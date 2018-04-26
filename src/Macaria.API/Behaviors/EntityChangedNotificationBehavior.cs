using Macaria.API.Features.Notes;
using Macaria.API.Features.Tags;
using Macaria.API.Hubs;
using Macaria.Infrastructure.Data;
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
        private readonly IMacariaContext _context;

        public EntityChangedNotificationBehavior(IHubContext<AppHub> hubContext, IMacariaContext context)
        {
            _hubContext = hubContext;
            _context = context;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (typeof(TRequest) == typeof(SaveNoteCommand.Request))
                return await (HandleSaveNoteCommand(request as SaveNoteCommand.Request, cancellationToken, next as RequestHandlerDelegate<SaveNoteCommand.Response>) as Task<TResponse>);

            if (typeof(TRequest) == typeof(RemoveNoteCommand.Request))
                return await (HandleRemoveNoteCommand(request as RemoveNoteCommand.Request, cancellationToken, next as RequestHandlerDelegate<RemoveNoteCommand.Response>) as Task<TResponse>);

            if (typeof(TRequest) == typeof(SaveTagCommand.Request))
                return await (HandleSaveTagCommand(request as SaveTagCommand.Request, cancellationToken, next as RequestHandlerDelegate<SaveTagCommand.Response>) as Task<TResponse>);

            if (typeof(TRequest) == typeof(RemoveTagCommand.Request))
                return await (HandleRemoveTagCommand(request as RemoveTagCommand.Request, cancellationToken, next as RequestHandlerDelegate<RemoveTagCommand.Response>) as Task<TResponse>);

            return await next();
        }

        public async Task<SaveNoteCommand.Response> HandleSaveNoteCommand(SaveNoteCommand.Request request, CancellationToken cancellationToken, RequestHandlerDelegate<SaveNoteCommand.Response> next)
        {
            var response = await next();

            var note = await _context.Notes.FindAsync(response.NoteId);

            await _hubContext.Clients.All.SendAsync("message", new
            {
                Type = "[Note] Saved",
                Payload = new { note = NoteApiModel.FromNote(note) }
            });

            return response;
        }

        public async Task<RemoveNoteCommand.Response> HandleRemoveNoteCommand(RemoveNoteCommand.Request request, CancellationToken cancellationToken, RequestHandlerDelegate<RemoveNoteCommand.Response> next)
        {
            var response = await next();

            await _hubContext.Clients.All.SendAsync("message", new
            {
                Type = "[Note] Removed",
                Payload = new { noteId = request.NoteId }
            });

            return response;
        }

        public async Task<SaveTagCommand.Response> HandleSaveTagCommand(SaveTagCommand.Request request, CancellationToken cancellationToken, RequestHandlerDelegate<SaveTagCommand.Response> next)
        {
            var response = await next();

            var tag = await _context.Tags.FindAsync(response.TagId);

            await _hubContext.Clients.All.SendAsync("message", new
            {
                Type = "[Tag] Saved",
                Payload = new { tag = TagApiModel.FromTag(tag) }
            });

            return response;
        }

        public async Task<RemoveTagCommand.Response> HandleRemoveTagCommand(RemoveTagCommand.Request request, CancellationToken cancellationToken, RequestHandlerDelegate<RemoveTagCommand.Response> next)
        {
            var response = await next();

            await _hubContext.Clients.All.SendAsync("message", new
            {
                Type = "[Tag] Removed",
                Payload = new { tagId = request.TagId }
            });

            return response;
        }
    } 
}
