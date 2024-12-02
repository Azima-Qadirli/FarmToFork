using FarmToFork.Models;
using Microsoft.EntityFrameworkCore;

namespace FarmToFork.Context;

public class FarmToForkDbContext:DbContext
{
    public FarmToForkDbContext(DbContextOptions<FarmToForkDbContext> options):base(options)
    {
        
    }
    public DbSet<Feature>Features { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<BlogTag>BlogTags { get; set; }
}