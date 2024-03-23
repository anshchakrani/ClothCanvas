namespace ClothCanvas.Models;

public class CartItemViewModel
{
    public int ProductId { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public decimal Total => Price * Quantity;
    public string ImageUrl { get; set; }
    
    public string? CustomProductURL { get; set; }
}
