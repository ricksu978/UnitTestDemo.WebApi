using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks.Dataflow;
using UnitTestDemo.WebApi;

namespace UnitTestDemo.WebApiTest.Common
{
    internal class WebApiTestServer : WebApplicationFactory<Program>
    {
        public HttpClient WebApiClient { get; init; }
        public HubConnection WebSocketClient { get; init; }
        public BufferBlock<string> MessageBuffer { get; init; }

        public WebApiTestServer()
        {
            MessageBuffer = new();
            WebApiClient = CreateClient();
            WebSocketClient = CreateWebSocketClient();
            WebSocketClient.On<string>(nameof(EchoHub.Echo), msg => MessageBuffer.Post(msg));
        }

        private HubConnection CreateWebSocketClient()
        {
            var url = new UriBuilder(Server.BaseAddress) { Path = "/socket" }.ToString();

            var builder = new HubConnectionBuilder().WithUrl(url, opt => opt.HttpMessageHandlerFactory = _ => Server.CreateHandler());

            return builder.Build();
        }

        internal Task StartAsync()
        {
            if(WebSocketClient.State == HubConnectionState.Disconnected)
            {
                return WebSocketClient.StartAsync();
            }

            return Task.CompletedTask;
        }

        public T GetRequiredService<T>()
            where T : notnull
        {
            return Server.Services.CreateScope().ServiceProvider.GetRequiredService<T>();
        }

    }
}
