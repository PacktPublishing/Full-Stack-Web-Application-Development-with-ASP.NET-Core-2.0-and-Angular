using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Macaria.Core.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using Macaria.Core.DomainEvents;

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
