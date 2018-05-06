using Macaria.Infrastructure.Data;
using Macaria.Infrastructure.Identity;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using Macaria.Core.Entities;
using Macaria.Infrastructure.Services;

namespace Macaria.API.Features.Users
{
    public class AuthenticateCommand
    {
        public class Request : IRequest<Response>
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        public class Response
        {
            public string AccessToken { get; set; }
            public int UserId { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IMacariaContext _context;
            private readonly IPasswordHasher _passwordHasher;
            private readonly ITokenProvider _tokenProvider;

            public Handler(IMacariaContext context, ITokenProvider tokenProvider, IPasswordHasher passwordHasher)
            {
                _context = context;
                _tokenProvider = tokenProvider;
                _passwordHasher = passwordHasher;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var user = await _context.Users
                    .SingleOrDefaultAsync(x => x.Username.ToLower() == request.Username.ToLower());

                if (user == null)
                    throw new System.Exception();

                if (!ValidateUser(user, _passwordHasher.HashPassword(request.Password)))
                    throw new System.Exception();

                return new Response()
                {
                    AccessToken = _tokenProvider.Get(request.Username),
                    UserId = user.UserId
                };
            }

            public bool ValidateUser(User user, string transformedPassword)
            {
                if (user == null || transformedPassword == null)
                    return false;

                return user.Password == transformedPassword;
            }
        }
    }
}
