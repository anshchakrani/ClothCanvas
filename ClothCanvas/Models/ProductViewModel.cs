using ClothCanvas.Entity;
using System.ComponentModel.DataAnnotations;

namespace ClothCanvas.Models;

public class ProductViewModel
{
    public int ProductId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public bool IsCustom { get; set; }
    public DateTime CreatedAt { get; set; }
    public string ImageUrl { get; set; }
    public string CategoryName { get; set; }
    public string SupplierName { get; set; }


    [Required(ErrorMessage = "Please enter quantity")]
    public int Quantity { get; set; } = 1;

    public ProductViewModel(Product product)
    {
        ProductId = product.ProductId;
        Name = product.Name;
        Description = product.Description;
        Price = product.Price;
        IsCustom = product.IsCustom;
        CreatedAt = product.CreatedAt;
        ImageUrl = product.ImageUrl;
        CategoryName = product.Category.Name;
        SupplierName = product.Supplier.Name;
    }

}