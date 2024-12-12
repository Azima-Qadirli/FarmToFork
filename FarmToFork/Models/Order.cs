using FarmToFork.Models.BaseEntities;

namespace FarmToFork.Models;

public class Order:BaseEntity
{
    public string AppUserId { get; set; }
    public AppUser AppUser { get; set; }
    public List<OrderItem> OrderItems { get; set; }
}