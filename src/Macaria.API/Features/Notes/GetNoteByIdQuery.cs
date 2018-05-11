using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Macaria.Infrastructure.Data;
using FluentValidation;

namespace Macaria.API.Features.Notes
{
    public class GetNoteByIdQuery
    {
        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(request => request.NoteId).NotEqual(0);
            }
        }

        public class Request : IRequest<Response> {
            public int NoteId { get; set; }
        }

        public class Response
        {
            public NoteApiModel Note { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            public IMacariaContext _context { get; set; }
            public Handler(IMacariaContext context) => _context = context;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
                => new Response()
                {
                    Note = NoteApiModel.FromNote(await _context.Notes.FindAsync(request.NoteId))
                };
        }
    }
}
