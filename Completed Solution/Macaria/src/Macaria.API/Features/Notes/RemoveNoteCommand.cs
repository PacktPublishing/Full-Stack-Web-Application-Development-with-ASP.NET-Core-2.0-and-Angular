using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Macaria.Infrastructure.Data;
using FluentValidation;

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
        public class Request : IRequest
        {
            public int NoteId { get; set; }
        }

        public class Handler : IRequestHandler<Request>
        {
            public IMacariaContext _context { get; set; }
            public Handler(IMacariaContext context)
            {
                _context = context;
            }

            public async Task Handle(Request request, CancellationToken cancellationToken)
            {
                _context.Notes.Remove(await _context.Notes.FindAsync(request.NoteId));
                await _context.SaveChangesAsync(cancellationToken);
            }

        }
    }
}
