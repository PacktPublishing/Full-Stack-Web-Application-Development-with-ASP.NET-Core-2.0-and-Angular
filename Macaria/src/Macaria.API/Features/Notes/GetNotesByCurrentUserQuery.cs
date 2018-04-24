using Macaria.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using System.Linq;

namespace Macaria.API.Features.Notes
{
    public class GetGetNotesByCurrentUserQuery
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
                    Notes = await _context.Notes
                    .Where(x => x.CreatedBy == _context.Username)
                    .Select(x => NoteApiModel.FromNote(x)).ToListAsync()
                };
        }
    }
}
