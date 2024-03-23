using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ClothCanvas.Entity;

public class Order
{
    public int OrderId { get; set; }

    [ValidateNever]
    public string UserId { get; set; }
    public decimal TotalPrice { get; set; }
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; }

    [ValidateNever]
    public User User { get; set; }

    [ValidateNever]
    public List<OrderItem> OrderItems { get; set; }

    [ValidateNever]
    public List<Payment> Payments { get; set; }
}