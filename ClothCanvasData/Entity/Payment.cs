namespace ClothCanvas.Entity;

public class Payment
{
    public int PaymentId { get; set; }
    public int OrderId { get; set; }
    public decimal PaidAmount { get; set; }
    public string PaymentMethod { get; set; }
    public string PaymentStatus { get; set; }
    public DateTime PaymentDate { get; set; }
    
    public Order Order { get; set; }
}