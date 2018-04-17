using Macaria.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Macaria.API.Features.Tenants
{
    public class VerifyTenantCommand
    {
        public class Request: IRequest
        {
            public Guid TenantId { get; set; }
        }

        public class Handler : IRequestHandler<Request>
        {
            private readonly IMacariaContext _context;

            public Handler(IMacariaContext context)
            {
                _context = context;
            }

            public async Task Handle(Request request, CancellationToken cancellationToken)
            {
                if ((await _context.Tenants.Where(x => x.TenantId == request.TenantId)
                    .SingleOrDefaultAsync()) == null)
                    throw new Exception();
            }
        }
    }
}
