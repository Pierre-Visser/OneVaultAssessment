using Microsoft.AspNetCore.SignalR;

namespace BlazorWasm.Shared.Hubs
{
    public interface IEchoHub
    {
        Task SendMessage(string message);
        Task ReceiveMessage(string message);
    }

    public class EchoHub : Hub<IEchoHub>
    {
        public async Task SendMessage(string message)
        {
            await Clients.All.ReceiveMessage(message);
        }
    }
}
