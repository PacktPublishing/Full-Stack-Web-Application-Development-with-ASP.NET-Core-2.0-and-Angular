using Macaria.Core.DomainEvents;
using Macaria.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Macaria.API.Features.Notes
{
    public class UndoNoteDeleteCommand
    {
        public class Request : IRequest {
            public Guid NoteId { get; set; }
        }
        
        public class Handler : IRequestHandler<Request>
        {
            private readonly IAppDbContext _context;
            public Handler(IAppDbContext context) => _context = context;

            public async Task Handle(Request request, CancellationToken cancellationToken)
            {
                var note = await _context.Notes
                    .IgnoreQueryFilters()
                    .Where(x => x.IsDeleted)
                    .SingleAsync(x => x.NoteId == request.NoteId);

                note.IsDeleted = false;

                note.RaiseDomainEvent(new NoteSaved(note.NoteId));

                await _context.SaveChangesAsync(cancellationToken);                
            }
        }
    }
}
