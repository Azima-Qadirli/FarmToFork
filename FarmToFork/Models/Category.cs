using FarmToFork.Models.BaseEntities;

namespace FarmToFork.Models;

public class Category:BaseEntity
{
    public string Name { get; set; }
    public ICollection<Blog>? Blogs { get; set; }
}