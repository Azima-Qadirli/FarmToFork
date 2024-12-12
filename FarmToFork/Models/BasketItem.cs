using FarmToFork.Models.BaseEntities;

namespace FarmToFork.Models;

public class BasketItem:BaseEntity
{
    public int BasketId { get; set; }
    public Basket Basket { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }
    public int Count { get; set; }
}