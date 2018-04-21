using FluentValidation;
using Macaria.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Macaria.API.Features.Notes
{
    public class RemoveNoteTagCommand
    {
        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {

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
                    .SingleAsync(x => x.NoteId == request.NoteId);

                var noteTag = note.NoteTags.Where(x => x.TagId == request.TagId).SingleOrDefault();
                
                if (noteTag != null)
                {
                    note.NoteTags.Remove(noteTag);
                    await _context.SaveChangesAsync(cancellationToken);
                }                
            }
        }
    }
}
