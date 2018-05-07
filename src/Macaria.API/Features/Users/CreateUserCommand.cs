using FluentValidation;
using Macaria.Infrastructure.Data;
using Macaria.Core.Entities;
using Macaria.Infrastructure.Identity;
using MediatR;
using System.Threading.Tasks;
using System.Threading;

namespace Macaria.API.Features.Users
{
    public class CreateUserCommand
    {
        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(x => x.Username).NotEmpty().EmailAddress();
                
                RuleFor(x => x.Password).Must(password => PasswordValidatorRule(password));
            }

            private bool PasswordValidatorRule(string password)
                => password.Length > 4;
        }

        public class Request : IRequest<Response>
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        public class Response
        {
            public int UserId { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            public IMacariaContext _context { get; set; }
            public IPasswordHasher _passwordHasher { get; set; }
            public Handler(IMacariaContext context, IPasswordHasher passwordHasher)
            {
                _context = context;
                _passwordHasher = passwordHasher;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var user = new User
                {
                    Username = request.Username
                };

                user.Password = _passwordHasher.HashPassword(user.Salt, request.Password);

                _context.Users.Add(user);

                await _context.SaveChangesAsync(cancellationToken);

                return new Response() { UserId = user.UserId };
            }
        }
    }
}
