using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Macaria.Infrastructure.Data;
using FluentValidation;
using System;

namespace Macaria.API.Features.Tenants
{
    public class GetTenantByIdQuery
    {
        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(request => request.TenantId).NotEqual(default(Guid));
            }
        }

        public class Request : IRequest<Response> {
            public Guid TenantId { get; set; }
        }

        public class Response
        {
            public TenantApiModel Tenant { get; set; }
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
                    Tenant = TenantApiModel.FromTenant(await _context.Tenants.FindAsync(request.TenantId))
                };
        }
    }
}
