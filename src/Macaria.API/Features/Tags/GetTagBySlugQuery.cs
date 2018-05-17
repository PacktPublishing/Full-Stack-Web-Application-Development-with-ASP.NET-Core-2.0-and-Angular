using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Macaria.Core.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;

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
                        .Include(x =>x.NoteTags)
                        .Include("NoteTags.Note")
                        .Where(x => x.Slug == request.Slug)
                        .SingleAsync())
                };
        }
    }
}
