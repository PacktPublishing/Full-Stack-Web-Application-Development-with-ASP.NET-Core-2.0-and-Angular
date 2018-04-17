using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Macaria.Infrastructure.Data;
using FluentValidation;

namespace Macaria.API.Features.Tenants
{
    public class RemoveTenantCommand
    {
        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(request => request.TenantId).NotEqual(0);
            }
        }
        public class Request : IRequest
        {
            public int TenantId { get; set; }
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
                _context.Tenants.Remove(await _context.Tenants.FindAsync(request.TenantId));
                await _context.SaveChangesAsync(cancellationToken);
            }

        }
    }
}
