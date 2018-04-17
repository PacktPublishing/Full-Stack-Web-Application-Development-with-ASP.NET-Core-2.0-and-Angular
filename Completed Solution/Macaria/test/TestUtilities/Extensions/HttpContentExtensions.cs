using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace TestUtilities.Extensions
{
    public static class HttpContentExtensions
    {
        public static async Task<T> ReadAsAsync<T>(this HttpContent httpContent) {
            var responseString = await httpContent.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseString);
        }
    }
}
