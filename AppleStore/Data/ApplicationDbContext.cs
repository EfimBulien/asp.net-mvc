using AppleStore.Models;
using Microsoft.EntityFrameworkCore;

namespace AppleStore.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Product>().ToTable("products");
        modelBuilder.Entity<Category>().ToTable("categories");
        modelBuilder.Entity<User>().ToTable("users");
        modelBuilder.Entity<Role>().ToTable("roles");
        /*
        modelBuilder.Entity<Role>().HasData(
            new Role { IDRole = 1, RoleName = "Администратор" },
            new Role { IDRole = 2, RoleName = "Менеджер" },
            new Role { IDRole = 3, RoleName = "Покупатель" }
        );
        */
    }
}