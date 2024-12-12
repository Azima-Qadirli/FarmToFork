using FarmToFork.Models.BaseEntities;

namespace FarmToFork.Models;

public class OrderItem:BaseEntity
{
    public int OrderId { get; set; }
    public Order Order { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }
    public int Count { get; set; }
}