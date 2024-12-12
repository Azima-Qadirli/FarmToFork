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
    public DbSet<Message>Messages { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductImage> ProductImages { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem>OrderItems { get; set; }
    public DbSet<Basket>Baskets { get; set; }
    public DbSet<BasketItem>BasketItems { get; set; }
}