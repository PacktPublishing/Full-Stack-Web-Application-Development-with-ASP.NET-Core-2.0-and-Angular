using Macaria.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Macaria.API.Features.Notes
{
    public class GetDeletedNotesQuery
    {
        public class Request : IRequest<Response> { }

        public class Response
        {
            public IEnumerable<NoteApiModel> Notes { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            public IAppDbContext _context { get; set; }
            public Handler(IAppDbContext context) => _context = context;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
                => new Response()
                {
                    Notes = await _context.Notes
                    .IgnoreQueryFilters()
                    .Where(x => x.IsDeleted)
                    .Select(x => NoteApiModel.FromNote(x, false))
                    .ToListAsync()
                };
        }
    }
}
