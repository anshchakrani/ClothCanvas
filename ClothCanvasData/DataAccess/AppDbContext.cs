using ClothCanvas.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ClothCanvas.DataAccess;

public class AppDbContext : IdentityDbContext<User>
{
    public DbSet<User> Users { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<CustomDetail> CustomDetails { get; set; }
    public DbSet<Inventory> Inventories { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>().ToTable("Users");
        modelBuilder.Entity<IdentityRole>().ToTable("Roles");
        modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
        modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
        modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
        modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");
        modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
        
        
        modelBuilder.Entity<User>().HasMany(u => u.Addresses).WithOne(a => a.User);
        modelBuilder.Entity<User>().HasMany(u => u.Orders).WithOne(o => o.User);
        modelBuilder.Entity<Order>().HasMany(o => o.OrderItems).WithOne(oi => oi.Order);
        modelBuilder.Entity<Product>().HasMany(p => p.Inventories).WithOne(i => i.Product);
        modelBuilder.Entity<Product>().HasOne(p => p.Category).WithMany(c => c.Products);
        modelBuilder.Entity<Supplier>().HasMany(s => s.Products).WithOne(p => p.Supplier);
        modelBuilder.Entity<Payment>().HasOne(p => p.Order).WithMany(o => o.Payments);
        modelBuilder.Entity<Inventory>().HasOne(i => i.Product).WithMany(p => p.Inventories);
        modelBuilder.Entity<OrderItem>().HasOne(oi => oi.Product);
        modelBuilder.Entity<OrderItem>().HasOne(oi => oi.Order).WithMany(o => o.OrderItems);
        modelBuilder.Entity<Address>().HasOne(a => a.User).WithMany(u => u.Addresses);
        modelBuilder.Entity<Category>().HasMany(c => c.Products).WithOne(p => p.Category);
        modelBuilder.Entity<Payment>().HasOne(p => p.Order).WithMany(o => o.Payments);
        
        
        // Seed Data
        modelBuilder.Entity<Category>().HasData(
            new Category
            {
                CategoryId = 1,
                Name = "T-Shirt",
                Description = "T-Shirt"
            },
            new Category
            {
                CategoryId = 2,
                Name = "Pants",
                Description = "Pants"
            },
            new Category
            {
                CategoryId = 3,
                Name = "Shoes",
                Description = "Shoes"
            }
        );
        
        modelBuilder.Entity<Supplier>().HasData(
            new Supplier
            {
                SupplierId = 1,
                Name = "Nike",
                ContactInfo = "1234567890"
            },
            new Supplier
            {
                SupplierId = 2,
                Name = "Adidas",
                ContactInfo = "0987654321"
            }
        );
        
        modelBuilder.Entity<Product>().HasData(
            new Product
            {
                ProductId = 1,
                CategoryId = 1,
                Name = "Nike T-Shirt",
                Description = "Nike T-Shirt",
                Price = 100,
                ImageUrl = "https://www.nike.com/t-shirt",
                SupplierId = 1
            },
            new Product
            {
                ProductId = 2,
                CategoryId = 2,
                Name = "Adidas Pants",
                Description = "Adidas Pants",
                Price = 200,
                ImageUrl = "https://www.adidas.com/pants",
                SupplierId = 2
            }
        );
        
        modelBuilder.Entity<Inventory>().HasData(
            new Inventory
            {
                InventoryId = 1,
                ProductId = 1,
                Quantity = 100
            },
            new Inventory
            {
                InventoryId = 2,
                ProductId = 2,
                Quantity = 200
            }
        );
        
        
    }
    
    
}