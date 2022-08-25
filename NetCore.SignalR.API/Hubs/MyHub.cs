using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using NetCore.SignalR.API.Models;

namespace NetCore.SignalR.API.Hubs
{
    public class MyHub : Hub
    {

        private static List<string> Names { get; set; } = new();
        private static int ClientCount { get; set; } = 0;
        public static int TeamCount { get; set; } = 7;

        private readonly AppDbContext _context;

        public MyHub(AppDbContext context)
        {
            _context = context;
        }
        
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

        //Groups

        public async Task AddToGroup(string teamName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, teamName);
        }

        public async Task SendNameByGroup(string name, string teamName)
        {
            var team = _context.Teams.Where(x => x.Name == teamName).FirstOrDefault();
            if (team != null)
            {
                team.Users.Add(new User{Name = name});
            }
            else
            {
                var newTeam = new Team() { Name = teamName };
                newTeam.Users.Add(new User{Name = name});
                _context.Teams.Add(newTeam);
            }

            await _context.SaveChangesAsync();

            await Clients.Groups(teamName).SendAsync("ReceiveNameByGroup",name,team.Id);
        }

        public async Task GetNamesByGroup()
        {
            var teams = _context.Teams.Include(team => team.Users).ToList();


            await Clients.All.SendAsync("ReceiveAllNamesByGroup", teams);
        }

        public async Task LeftToGroup(string teamName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, teamName);
        }

        public async Task SendProduct(Product product)
        {
           await  Clients.All.SendAsync("ReceiveProduct", product);
        }
    }
}
