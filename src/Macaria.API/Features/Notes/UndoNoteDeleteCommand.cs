using Macaria.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Macaria.API.Features.Notes
{
    public class UndoNoteDeleteCommand
    {
        public class Request : IRequest
        {
            public int NoteId { get; set; }
        }
        
        public class Handler : IRequestHandler<Request>
        {
            public IAppDbContext _context { get; set; }
            public Handler(IAppDbContext context) => _context = context;

            public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                var note = await _context.Notes
                    .IgnoreQueryFilters()
                    .Where(x => x.IsDeleted)
                    .SingleAsync(x => x.NoteId == request.NoteId);

                note.IsDeleted = false;

                note.RaiseDomainEvent(new NoteSavedEvent.DomainEvent(note));

                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}