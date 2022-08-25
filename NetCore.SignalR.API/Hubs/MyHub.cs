using Microsoft.AspNetCore.SignalR;

namespace NetCore.SignalR.API.Hubs
{
    public class MyHub : Hub
    {
        private static List<string> Names { get; set; } = new();
        private static int ClientCount { get; set; } = 0;

        public static int TeamCount { get; set; } = 7;
        
        public async Task SendName(string name)
        {
            if (Names.Count >= TeamCount)
            {
                await Clients.Caller.SendAsync("Error", $"Takım en fazla {TeamCount} kişi olabilir. ");
                return;
            }
            Names.Add(name);
            await Clients.All.SendAsync("ReceiveName", name);
        }

        public async Task GetNames()
        {
            await Clients.All.SendAsync("ReceiveNames", Names);
        }

        public async override Task OnConnectedAsync()
        {
            ClientCount++;
            await Clients.All.SendAsync("ReceiveClientCount", ClientCount);

            await base.OnConnectedAsync();
        }

        public async override Task OnDisconnectedAsync(Exception? exception)
        {
            ClientCount--;
            await Clients.All.SendAsync("ReceiveClientCount", ClientCount);
            await base.OnDisconnectedAsync(exception);
        }
    }
}
