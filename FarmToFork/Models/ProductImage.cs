using FarmToFork.Models.BaseEntities;

namespace FarmToFork.Models;

public class ProductImage:BaseEntity
{
    public bool IsMain { get; set; }
    public string? Image { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }
}