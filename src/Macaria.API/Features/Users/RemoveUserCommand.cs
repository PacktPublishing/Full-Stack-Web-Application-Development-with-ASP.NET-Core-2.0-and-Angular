using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Macaria.Infrastructure.Data;
using FluentValidation;

namespace Macaria.API.Features.Users
{
    public class RemoveUserCommand
    {
        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(request => request.UserId).NotEqual(0);
            }
        }

        public class Request : IRequest
        {
            public int UserId { get; set; }
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
                _context.Users.Remove(await _context.Users.FindAsync(request.UserId));
                await _context.SaveChangesAsync(cancellationToken);
            }

        }
    }
}
