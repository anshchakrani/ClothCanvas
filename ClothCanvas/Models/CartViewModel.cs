namespace ClothCanvas.Models;

public class CartViewModel
{
    public IList<CartItemViewModel> Items { get; set; } = new List<CartItemViewModel>();
    public decimal TotalPrice { get; set; }

    // 13% tax
    public decimal Tax => TotalPrice * 0.13m;

    public decimal GrandTotal => TotalPrice + Tax;

    public int TotalItems => Items.Sum(x => x.Quantity);


}