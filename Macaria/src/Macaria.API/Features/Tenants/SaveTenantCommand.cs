using MediatR;
using System.Threading.Tasks;
using System.Threading;
using FluentValidation;
using Macaria.Infrastructure.Data;
using Macaria.Core.Entities;
using System;

namespace Macaria.API.Features.Tenants
{
    public class SaveTenantCommand
    {
        public class Validator: AbstractValidator<Request> {
            public Validator()
            {
                RuleFor(request => request.Tenant.TenantId).NotEqual(default(Guid));
            }
        }

        public class Request : IRequest<Response> {
            public TenantApiModel Tenant { get; set; }
        }

        public class Response
        {			
            public Guid TenantId { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            public IMacariaContext _context { get; set; }
            public Handler(IMacariaContext context)
            {
                _context = context;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var tenant = await _context.Tenants.FindAsync(request.Tenant.TenantId);

                if (tenant == null) _context.Tenants.Add(tenant = new Tenant());

                tenant.Name = request.Tenant.Name;

                await _context.SaveChangesAsync(cancellationToken);

                return new Response() { TenantId = tenant.TenantId };
            }
        }
    }
}
