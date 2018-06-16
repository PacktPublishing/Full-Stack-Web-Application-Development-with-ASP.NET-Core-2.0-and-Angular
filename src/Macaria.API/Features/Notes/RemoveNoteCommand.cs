using FluentValidation;
using Macaria.Core.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Macaria.API.Features.Notes
{
    public class RemoveNoteCommand
    {
        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(request => request.NoteId).NotEqual(0);
            }
        }
        public class Request : IRequest<Response>
        {
            public int NoteId { get; set; }
        }

        public class Response { }

        public class Handler : IRequestHandler<Request,Response>
        {
            private readonly IAppDbContext _context;

            public Handler(IAppDbContext context) => _context = context;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var note = await _context.Notes.FindAsync(request.NoteId);
                _context.Notes.Remove(note);
                note.RaiseDomainEvent(new NoteRemovedEvent.DomainEvent(note.NoteId));
                await _context.SaveChangesAsync(cancellationToken);
                return new Response() { };
            }
        }
    }
}
