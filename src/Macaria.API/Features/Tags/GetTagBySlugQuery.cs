using Macaria.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Macaria.API.Features.Tags
{
    public class GetTagBySlugQuery
    {
        public class Request : IRequest<Response> {
            public string Slug { get; set; }
        }

        public class Response
        {
            public TagApiModel Tag { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IAppDbContext _context;

            public Handler(IAppDbContext context) => _context = context;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
                => new Response()
                {
                    Tag = TagApiModel.FromTag(await _context.Tags
                        .Include(x => x.NoteTags)
                        .ThenInclude(x => x.Note)
                        .SingleAsync(x => x.Slug == request.Slug))
                };
        }
    }
}
