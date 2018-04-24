using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Macaria.Infrastructure.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Macaria.API.Features.Tenants
{
    public class GetTenantsQuery
    {
        public class Request : IRequest<Response> { }

        public class Response
        {
            public IEnumerable<TenantApiModel> Tenants { get; set; }
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
                    Tenants = await _context.Tenants.Select(x => TenantApiModel.FromTenant(x)).ToListAsync()
                };
        }
    }
}
