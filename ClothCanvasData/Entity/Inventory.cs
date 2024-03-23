namespace ClothCanvas.Entity;

public class Inventory
{
    public int InventoryId { get; set; }
    public int ProductId { get; set; }
    
    public int Quantity { get; set; }
    public int RestockThreshold { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public Product Product { get; set; }
    
}