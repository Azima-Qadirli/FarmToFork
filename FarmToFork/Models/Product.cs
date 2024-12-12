using FarmToFork.Models.BaseEntities;

namespace FarmToFork.Models;

public class Product:BaseEntity
{
    public required string Name { get; set; }
    public required decimal Price { get; set; }
    public required string Image   { get; set; }
}