using System.ComponentModel.DataAnnotations;

namespace ClothCanvas.Entity;

public class CustomDetail
{
    public int CustomDetailId { get; set; }
    public int OrderItemId { get; set; }
    public string Customizations { get; set; }
    
    [DataType(DataType.Date)]
    public DateTime CreatedAt { get; set; } = DateTime.Today;
    
    public OrderItem? OrderItem { get; set; }
    
}