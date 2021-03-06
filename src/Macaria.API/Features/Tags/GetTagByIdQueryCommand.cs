using FluentValidation;
using Macaria.Core.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Macaria.API.Features.Tags
{
    public class GetTagByIdQuery
    {
        public class Request : IRequest<Response> {
            public Guid TagId { get; set; }
        }

        public class Response
        {
            public TagDto Tag { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IAppDbContext _context;

            public Handler(IAppDbContext context) => _context = context;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
                => new Response()
                {
                    Tag = TagDto.FromTag(await _context.Tags.FindAsync(request.TagId))
                };
        }
    }
}
