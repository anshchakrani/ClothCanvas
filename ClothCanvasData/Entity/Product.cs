using System.ComponentModel.DataAnnotations;

namespace ClothCanvas.Entity;

public class Product
{
    public int ProductId { get; set; }
    
    public int CategoryId { get; set; }
    
    public int SupplierId { get; set; }
    
    public string Name { get; set; }
    
    [Required]
    public string Description { get; set; }

    public double MinimumQuantity { get; set; }
    
    [Required]
    [Range(0, 1000000)]
    public decimal Price { get; set; }
    
    public bool IsCustom { get; set; } = false;
    
    public DateTime CreatedAt { get; set; }
    
    public string ImageUrl { get; set; }
    
    public Category Category { get; set; }
    
    public List<Inventory> Inventories { get; set; }
    
    public Supplier Supplier { get; set; }
    
    public float Discount { get; set; } = 0;
}
