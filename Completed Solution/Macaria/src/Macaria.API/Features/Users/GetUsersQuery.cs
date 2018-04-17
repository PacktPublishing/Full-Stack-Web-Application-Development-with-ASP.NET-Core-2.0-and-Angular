using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Macaria.Infrastructure.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Macaria.API.Features.Users
{
    public class GetUsersQuery
    {
        public class Request : IRequest<Response> { }

        public class Response
        {			
            public IEnumerable<UserApiModel> Users { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IMacariaContext _context;
            public Handler(IMacariaContext context)
            {
                _context = context;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
                => new Response() {
                    Users = await _context.Users.Select(x => UserApiModel.FromUser(x)).ToListAsync()
                };
        }
    }
}
