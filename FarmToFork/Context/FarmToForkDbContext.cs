using FarmToFork.Models;
using Microsoft.EntityFrameworkCore;

namespace FarmToFork.Context;

public class FarmToForkDbContext:DbContext
{
    public FarmToForkDbContext(DbContextOptions<FarmToForkDbContext> options):base(options)
    {
        
    }
    public DbSet<Feature>Features { get; set; }
    
}