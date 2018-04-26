using FluentValidation;
using Macaria.Core.Entities;
using Macaria.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Macaria.API.Features.Notes
{
    public class AddTagCommand
    {
        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(request => request.NoteId).NotEqual(default(int));
                RuleFor(request => request.TagId).NotEqual(default(int));
            }
        }

        public class Request : IRequest
        {
            public int TagId { get; set; }
            public int NoteId { get; set; }
        }
        
        public class Handler : IRequestHandler<Request>
        {
            private readonly IMacariaContext _context;
            public Handler(IMacariaContext context)
            {
                _context = context;
            }

            public async Task Handle(Request request, CancellationToken cancellationToken)
            {
                var note = await _context.Notes
                    .Include(x => x.NoteTags)
                    .Include("NoteTags.Tag")
                    .SingleAsync(x => x.NoteId == request.NoteId);

                var tag = await _context.Tags.FindAsync(request.TagId);

                var noteTag = new NoteTag()
                {
                    NoteId = request.NoteId,
                    TagId = request.TagId,
                    Note = note,
                    Tag = tag
                };

                if(note.NoteTags.SingleOrDefault( x=> x.NoteId == request.NoteId && x.TagId == request.TagId) == null) {
                    note.NoteTags.Add(noteTag);
                    await _context.SaveChangesAsync(cancellationToken);
                }
            }
        }
    }
}
