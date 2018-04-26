using Macaria.Infrastructure.Data;
using MediatR;
using System.Threading.Tasks;
using System.Threading;

namespace Macaria.API.Features.Users
{
    public class GetUserByIdQuery
    {
        public class Request : IRequest<Response> {
            public int UserId { get; set; }
        }

        public class Response
        {			
            public UserApiModel User { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IMacariaContext _context;
            public Handler(IMacariaContext context)
            {
                _context = context;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.FindAsync(request.UserId);

                return new Response()
                {
                    User = UserApiModel.FromUser(user)
                };
            }
        }
    }
}
