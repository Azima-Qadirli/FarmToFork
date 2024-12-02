using System.ComponentModel.DataAnnotations.Schema;
using FarmToFork.Models.BaseEntities;

namespace FarmToFork.Models;

public class Author:BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string FileName { get; set; }
    [NotMapped]
    public IFormFile File { get; set; }
    public ICollection<Blog> Blogs { get; set; }
}