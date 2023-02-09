using Microsoft.AspNetCore.SignalR;

namespace UnitTestDemo.WebApi
{
    public class EchoHub : Hub
    {
        private readonly IHubContext<EchoHub> _hubContext;

        public EchoHub(IHubContext<EchoHub> hubContext)
        {
            _hubContext = hubContext;
        }


        public Task Echo(string rawRequest, CancellationToken cancellationToken = default)
        {
            return _hubContext.Clients.All.SendAsync(nameof(Echo), rawRequest, cancellationToken);
        }
    }
}
