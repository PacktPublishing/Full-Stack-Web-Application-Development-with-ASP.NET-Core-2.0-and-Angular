using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Macaria.Infrastructure.Data;
using FluentValidation;

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
        public class Request : IRequest
        {
            public int TagId { get; set; }
        }

        public class Handler : IRequestHandler<Request>
        {
            public IMacariaContext _context { get; set; }
            public Handler(IMacariaContext context)
            {
                _context = context;
            }

            public async Task Handle(Request request, CancellationToken cancellationToken)
            {
                _context.Tags.Remove(await _context.Tags.FindAsync(request.TagId));
                await _context.SaveChangesAsync(cancellationToken);
            }

        }
    }
}
