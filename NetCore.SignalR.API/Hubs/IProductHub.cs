using NetCore.SignalR.API.Models;

namespace NetCore.SignalR.API.Hubs
{
    public interface IProductHub
    {
        Task ReceiveProduct(Product p);
    }
}
