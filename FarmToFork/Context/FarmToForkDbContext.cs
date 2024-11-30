using FarmToFork.Models;
using Microsoft.EntityFrameworkCore;

namespace FarmToFork.Context;

public class FarmToForkDbContext:DbContext
{
    public DbSet<Feature>Features { get; set; }
    
}