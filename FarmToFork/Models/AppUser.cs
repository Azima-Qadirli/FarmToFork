using Microsoft.AspNetCore.Identity;

namespace FarmToFork.Models;

public class AppUser:IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public List<Message> Messages { get; set; } = new List<Message>();
    public List<Basket> Baskets { get; set; } = new List<Basket>();
    public List<Order> Orders { get; set; } = new List<Order>();
}