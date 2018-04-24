using Macaria.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;

namespace Macaria.API.Features.Notes
{
    public class GetGetNoteByUsernameAndTitleQuery
    {
        public class Request : IRequest<Response> {
            public string Title { get; set; }
        }

        public class Response
        {
            public NoteApiModel Note { get; set; }
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
                    Note = NoteApiModel.FromNote(await _context.Notes
                        .Include(x => x.NoteTags)
                        .Include("NoteTags.Tag")
                        .Where(x => x.CreatedBy == _context.Username
                        && x.Title == request.Title).SingleOrDefaultAsync())
                };
        }
    }
}
