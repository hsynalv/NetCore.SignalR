namespace NetCore.SignalR.API.Models;

public class Team
{

    public Team()
    {
        Users = new List<User>();
    }

    public int Id { get; set; }
    public int Name { get; set; }

    public virtual ICollection<User> Users { get; set; }
}