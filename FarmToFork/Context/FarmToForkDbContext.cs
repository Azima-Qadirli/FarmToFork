using FarmToFork.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FarmToFork.Context;

public class FarmToForkDbContext:IdentityDbContext<AppUser>
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