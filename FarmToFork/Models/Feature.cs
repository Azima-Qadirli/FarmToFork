using FarmToFork.Models.BaseEntities;

namespace FarmToFork.Models;

public class Feature:BaseEntity
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Icon { get; set; }
}