using Microsoft.EntityFrameworkCore;

namespace PRACTICA2.Models;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {
    }

    public DbSet<Book> Books{ get; set; } = null!;

    public DbSet<User> Users { get; set; } = null!;

    public DbSet<Reserved> Reserveds { get; set; } = null!;
    
}