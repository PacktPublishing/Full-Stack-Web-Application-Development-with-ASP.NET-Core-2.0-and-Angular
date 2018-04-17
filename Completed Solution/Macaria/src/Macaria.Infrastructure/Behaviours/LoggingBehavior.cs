using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Macaria.Infrastructure.Behaviours
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        public LoggingBehavior()
        {

        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {

            var response = await next();

            return response;
        }
    }
}
