using Microsoft.EntityFrameworkCore;

namespace PRACTICA2.Models;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {
    }

    public DbSet<Dish> dishes{ get; set; } = null!;

    public DbSet<User> Users { get; set; } = null!;

    public DbSet<Order> orders { get; set; } = null!;



    
}