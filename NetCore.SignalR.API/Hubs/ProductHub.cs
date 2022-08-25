using Microsoft.AspNetCore.SignalR;
using NetCore.SignalR.API.Models;

namespace NetCore.SignalR.API.Hubs;

public class ProductHub : Hub<IProductHub>
{
    public async Task SendProduct(Product p)
    {
        await Clients.All.ReceiveProduct(p);
    }
}