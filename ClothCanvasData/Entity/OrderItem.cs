using System.ComponentModel.DataAnnotations.Schema;

namespace ClothCanvas.Entity;

public class OrderItem
{
    public int OrderItemId { get; set; }
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    
    public decimal PriceEach { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public Order Order { get; set; }
    
    public Product Product { get; set; }
    
    public string? CustomDetailURL { get; set; }
    
}