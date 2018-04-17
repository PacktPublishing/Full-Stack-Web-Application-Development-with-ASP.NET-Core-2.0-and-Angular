using System;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTests
{
    public class ServerFixture<TStartup> : IDisposable
        where TStartup : class
    {
        private IWebHost _host;

        private IApplicationLifetime _lifetime;

        public string WebSocketsUrl => Url.Replace("http", "ws");

        public string Url { get; private set; }

        private HttpClient _client;

        public HttpClient CreateClient() {
            _client = new HttpClient
            {
                BaseAddress = new Uri(Url)
            };
            return _client;
        }
        public ServerFixture()
        {
            Url = "https://localhost:" + GetNextPort();
            StartServer(Url);
        }
        private void StartServer(string url)
        {
            try
            {                
                _host = new WebHostBuilder()
                    .UseStartup(typeof(TStartup))
                    .UseKestrel()
                    .UseUrls(url)
                    .UseConfiguration(TestUtilities.ConfigurationProvider.Get())
                    .ConfigureAppConfiguration((builderContext, config) =>
                    {                        
                        config
                        .AddJsonFile("settings.json");
                    })
                    .Build();

                var t = Task.Run(() => _host.Start());

                _lifetime = _host.Services.GetRequiredService<IApplicationLifetime>();
                if (!_lifetime.ApplicationStarted.WaitHandle.WaitOne(TimeSpan.FromSeconds(5)))
                {
                    if (t.IsFaulted)
                    {
                        throw t.Exception.InnerException;
                    }
                    throw new TimeoutException("Timed out waiting for application to start.");
                }


                _lifetime.ApplicationStopped.Register(() =>
                {

                });
            }catch(Exception e)
            {
                throw e;
            }
        }

        public void Dispose()
        {
            _host.Dispose();
        }

        private static int GetNextPort()
        {
            using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                socket.Bind(new IPEndPoint(IPAddress.Loopback, 0));
                return ((IPEndPoint)socket.LocalEndPoint).Port;
            }
        }
    }
}
