using Macaria.Core.ApiModels;
using Macaria.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
            private readonly IAppDbContext _context;

            public Handler(IAppDbContext context) => _context = context;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
                => new Response()
                {
                    Notes = await _context.Notes.Select(x => NoteApiModel.FromNote(x, true)).ToListAsync()
                };
        }
    }
}
