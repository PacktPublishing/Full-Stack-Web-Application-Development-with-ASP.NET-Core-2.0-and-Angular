using System.Net.Http;
using System.Net.Http.Headers;


namespace TestUtilities
{
    public class HttpClientFactory
    {
        public static HttpClient Get(int userId = default(int), string accessToken = default(string))
        {
            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            
            if (accessToken != default(string))
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            
            return httpClient;
        }
    }
}
