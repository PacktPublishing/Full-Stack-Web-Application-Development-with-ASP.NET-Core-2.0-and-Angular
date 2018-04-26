using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Macaria.Infrastructure.Data;
using Macaria.Infrastructure.Services;

namespace Macaria.API.Features.Users
{
    public class UserChangePasswordCommand
    {
        public class Request :  IRequest { 
            public int UserId { get; set; }
            public string Password { get; set; }
            public string ConfirmPassword { get; set; }
        }
        
        public class Handler : IRequestHandler<Request>
        {
            private readonly IEncryptionService _encryptionService;
            private readonly IMacariaContext _context;

            public Handler(IMacariaContext context, IEncryptionService encryptionService)
            {
                _context = context;
                _encryptionService = encryptionService;
            }

            public async Task Handle(Request request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.FindAsync(request.UserId);
                user.Password = _encryptionService.TransformPassword(request.Password);                
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
