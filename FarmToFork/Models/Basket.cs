using FarmToFork.Models.BaseEntities;

namespace FarmToFork.Models;

public class Basket:BaseEntity
{
    public string AppUserId { get; set; }
    public AppUser AppUser { get; set; }
    public List<BasketItem> BasketItems { get; set; } = new List<BasketItem>();
}