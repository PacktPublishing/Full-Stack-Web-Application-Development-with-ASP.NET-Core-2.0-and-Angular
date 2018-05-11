using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Macaria.Infrastructure.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Macaria.API.Features.Tags
{
    public class GetTagsQuery
    {
        public class Request : IRequest<Response> { }

        public class Response
        {
            public IEnumerable<TagApiModel> Tags { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            public IMacariaContext _context { get; set; }
            public Handler(IMacariaContext context) => _context = context;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
                => new Response()
                {
                    Tags = await _context.Tags.Select(x => TagApiModel.FromTag(x)).ToListAsync()
                };
        }
    }
}
