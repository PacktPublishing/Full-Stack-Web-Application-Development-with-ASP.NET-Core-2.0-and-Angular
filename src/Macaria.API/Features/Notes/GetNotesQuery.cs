using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Macaria.Infrastructure.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Macaria.API.Features.Notes
{
    public class GetNotesQuery
    {
        public class Request : IRequest<Response> { }

        public class Response
        {
            public IEnumerable<NoteApiModel> Notes { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            public IMacariaContext _context { get; set; }
            public Handler(IMacariaContext context)
            {
                _context = context;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
                => new Response()
                {
                    Notes = await _context.Notes.Select(x => NoteApiModel.FromNote(x, true)).ToListAsync()
                };
        }
    }
}
