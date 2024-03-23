using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ClothCanvas.Entity;

public class Supplier
{
    public int SupplierId { get; set; }
    public string Name { get; set; }
    public string ContactInfo { get; set; }
    public DateTime CreatedAt { get; set; }

    [ValidateNever]
    public List<Product> Products { get; set; }

    [ValidateNever]
    public List<Inventory> Inventories { get; set; }

    [ValidateNever]
    public List<Order> Orders { get; set; }
}