using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Linq;

namespace Macaria.Infrastructure.Extensions
{
    public static class HttpRequestExtensions
    {
        public static string GetHeaderValue(this HttpRequest request, string name)
        {
            StringValues values;
            var found = request.Headers.TryGetValue(name, out values);
            if (found)
            {
                return values.FirstOrDefault();
            }

            return null;
        }
    }
}
