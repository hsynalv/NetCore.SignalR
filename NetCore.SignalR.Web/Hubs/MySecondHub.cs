using Microsoft.AspNetCore.SignalR;

namespace NetCore.SignalR.Web.Hubs 
{
    public class MySecondHub : Hub
    {
        public async Task sendMessage(string message)
        {
            await Clients.All.SendAsync("ReceiverMessage", message);
        }
    }
}
