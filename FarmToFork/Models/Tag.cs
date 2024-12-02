using FarmToFork.Models.BaseEntities;

namespace FarmToFork.Models;

public class Tag:BaseEntity
{
    public string Name { get; set; }
    public ICollection<BlogTag>? BlogTags { get; set; }
}