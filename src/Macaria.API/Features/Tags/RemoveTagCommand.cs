using FluentValidation;
using Macaria.Core.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Macaria.API.Features.Tags
{
    public class RemoveTagCommand
    {
        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(request => request.TagId).NotEqual(0);
            }
        }
        public class Request : IRequest<Response>
        {
            public int TagId { get; set; }
        }

        public class Response { }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IAppDbContext _context;

            public Handler(IAppDbContext context) => _context = context;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var tag = await _context.Tags.FindAsync(request.TagId);
                _context.Tags.Remove(tag);
                tag.RaiseDomainEvent(new TagRemovedEvent.DomainEvent(tag.TagId));
                await _context.SaveChangesAsync(cancellationToken);
                return new Response() { };
            }
        }
    }
}
